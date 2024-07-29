using System.Collections;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    private bool canBeController = false;
    private int countHeart = 3;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    bool canDoubleJump;

    [Header("Collision")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [Space]
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private Transform enemyCheck;
    [SerializeField] private float enemyCheckRadius;
    public bool isGround;
    bool isAir;
    bool isWall;

    [Header("Coyote jump")]
    [SerializeField] private float coyoteJumpWindow = .5f;
    private float coyoteJumpActived = -1;

    [Header("Wall interactions")]
    [SerializeField] private float wallJumpDuration = .6f;
    [SerializeField] private Vector2 wallJumpForce;
    bool isWallJumping;

    [Header("Knockback")]
    [SerializeField] private float knockbackDuration = 1;
    [SerializeField] private Vector2 knockbackPower;
    private bool isKnocked;

    [Header("Player VFX")]
    [SerializeField] private GameObject deathVfx;
    [SerializeField] private AnimatorOverrideController[] animators;
    [SerializeField] private int skinIndex;

    float xInput;
    float yInput;
    bool facingRight = true;
    int facingDir = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }
    private void Start() {
        ChoosePlayerSkin();
    }
    void Update()
    {
        UpdateAirStatus();
        if (canBeController == false) {
            HandleCollision();
            HandleAnimations();
            return;
        }
        
        if (isKnocked)
            return;

        HandleEnemyDetection();
        HandleInput();
        HandleMovement();
        HandleWallSlide();
        HandleFlip();
        HandleCollision();
        HandleAnimations();
    }
    public void ChoosePlayerSkin() {
        SkinManager skinManager = SkinManager.instance;
        if (skinManager == null)
            return;
        anim.runtimeAnimatorController = animators[skinManager.GetSkinId()];
    }

    private void HandleEnemyDetection()
    {
        /* 1. Nếu người chơi đang nhảy thì bỏ qua 
           2. Phát hiện enemy bằng cách tìm kiếm các collider tiếp xúc trong phạm vi
           3. Nếu là enemy thì tiêu diệt */
        if (rb.velocity.y >=0)
            return;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemyCheck.position, enemyCheckRadius, whatIsEnemy);
        foreach (var enemy in colliders) {
            Enemy newEnemy = enemy.GetComponent<Enemy>();
            if (newEnemy != null) {
                AudioManager.instance.PlaySFX(1);
                newEnemy.Die();
                Jump(false);
            }
        }
    }

    public void Knockback(float knockBackSourcePosition) {
        // Knockback player khi va chạm với kẻ thủ và trừ 1 máu
        float knockBackDir = 1;
        if (transform.position.x < knockBackSourcePosition) {
            knockBackDir = -1;
        }
        if (isKnocked)
            return;
        countHeart -= 1;
        UI_Ingame.instance.UpdateHeart(countHeart);
        if (countHeart == 0) {
            Die();
            PlayerManager.instance.RespawnPlayer();
            UI_Ingame.instance.ResetHeart();
        }
        StartCoroutine(KnockbackRoutine());
        rb.velocity = new Vector2(knockbackPower.x * knockBackDir, knockbackPower.y);
        CameraManager.instance.CameraShake();
        AudioManager.instance.PlaySFX(9);
    }
    private IEnumerator KnockbackRoutine() {
        isKnocked = true;
        anim.SetBool("knockBack", true);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
        anim.SetBool("knockBack", false);
    }
    public void Die() {
        // Phá hủy người chơi khi hết máu hoặc rơi vào deadzone
        Destroy(gameObject);
        AudioManager.instance.PlaySFX(0);
        UI_Ingame.instance.ResetHeart();
        GameObject newDie = Instantiate(deathVfx, transform.position, Quaternion.identity);
    }
    public void Push(Vector2 direction, float duration = 0) {
        StartCoroutine(PushCoroutine(direction, duration));
    }
    private IEnumerator PushCoroutine(Vector2 direction, float duration) {
        // Đẩy player (phục vụ cho trampoline trap)
        canBeController = false;
        rb.velocity = Vector2.zero;
        rb.AddForce(direction, ForceMode2D.Impulse);
        yield return new WaitForSeconds(duration);
        canBeController = true;
    }
    public void RespawnFinished(bool finished) {
        // Hồi sinh và cho phép điều khiển player
        if (finished) {
            canBeController = true;
            AudioManager.instance.PlaySFX(11);
        }
        else {
            canBeController = false;
        }
    }
    private void HandleWallSlide()
    {   
        // Thiết lập di chuyên kiểu wallSlide
        bool canWallSlide = isWall && rb.velocity.y < 0;
        if (canWallSlide) canDoubleJump = false;
        float yModifier = yInput == -1 ? 1 : 0.3f;
        if (canWallSlide == false)
            return;

        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * yModifier);
    }
    void WallJump() { 
        // Thiết lập di chuyển theo kiểu wall jump
        canDoubleJump = false;
        rb.velocity = new Vector2(wallJumpForce.x * -facingDir, wallJumpForce.y);
        Flip();
        StopAllCoroutines();
        StartCoroutine(WallJumpRoutine());
        AudioManager.instance.PlaySFX(12);
    }
    private IEnumerator WallJumpRoutine() {
        isWallJumping = true;
        yield return new WaitForSeconds(wallJumpDuration);
        isWallJumping = false;
    }
    void UpdateAirStatus()
    {   
        if (isGround && isAir)
        {
            HandleDoubleJump();
        }
        if (!isGround && !isAir)
        {
            BecomeAir();
        }
    } 
    void BecomeAir() {
        isAir = true;
        // Nếu người chơi đang rơi thì gọi ActivateCoyoteJump()
        if (rb.velocity.y < 0)
            ActivateCoyoteJump();
    }
    void HandleInput()
    {
        // Lấy input của người chơi để điều khiển player
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space))
            JumpButton();
        if (Input.GetKeyDown(KeyCode.W)) {
            if (isWall && !isGround)
                WallJump();
        }
    }
    #region Coyote Jump
    void ActivateCoyoteJump() => coyoteJumpActived = Time.time;
    void CancelCoyoteJump() => coyoteJumpActived = Time.time - 1;
    #endregion
    void HandleAnimations()
    {
        // Điều khiển animaion
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGround", isGround);
        anim.SetBool("isWall", isWall);
    }
    void HandleMovement()
    {   
        // Điều khiển người chơi di chuyển trái phải, nếu phát hiện tường hoặc trong trạng thái wall jump thì bỏ qua
        if (isWall)
            return;
        if (isWallJumping)
            return;
        rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
    }
    void HandleCollision()
    {   
        // thiết lập isGround và isWall
        isGround = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWall = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }
    void HandleDoubleJump()
    {
        isAir = false;
        canDoubleJump = true;
    }
    void JumpButton()
    {
        bool coyoteJumpAvailable = Time.time < coyoteJumpActived + coyoteJumpWindow;
        if (isGround || coyoteJumpAvailable)
        {
            Jump(true);
        }
        else if (canDoubleJump)
        {
            DoubleJump();
        }
        else if (isWall && !isGround) {
            SlideWall();
        }
        CancelCoyoteJump();
    }

    void SlideWall() {
        canDoubleJump = false;
        rb.velocity = new Vector2(0, wallJumpForce.y);
        Flip();
        AudioManager.instance.PlaySFX(12);
    }
    void DoubleJump() {
        isWallJumping = false;
        canDoubleJump = false;
        AudioManager.instance.PlaySFX(3);
        rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
    }
    void Jump(bool hasAudio) {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        if (hasAudio)
            AudioManager.instance.PlaySFX(3);
    }
    void HandleFlip()
    {   
        // Khi Player di chuyển sang trái và sprite đang quay về bên phải thì lật 
        if (xInput < 0 && facingRight || xInput > 0 && !facingRight)
        {
            Flip();
        }
    }
    void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir *= -1;
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (facingDir * wallCheckDistance), transform.position.y));
        Gizmos.DrawWireSphere(enemyCheck.position, enemyCheckRadius);
    }
}

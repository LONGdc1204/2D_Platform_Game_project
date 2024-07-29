using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected BoxCollider2D[] cols;
    protected Transform playerTrans;
    private SpriteRenderer sr => GetComponent<SpriteRenderer>();

    [Header("Collision detail")]
    [SerializeField] protected float groundCheckDistance = 1f;
    [SerializeField] protected float wallCheckDistance = .75f;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float playerDetectionRange = 15f;
    protected bool IsPlayerDetected;

    [Header("Death detail")]
    [SerializeField] protected float deathImpact;
    [SerializeField] protected float deathRotationSpeed;
    protected float deathDirectionRotation = 1;
    protected bool isDead;

    [Header("Move detail")]
    [SerializeField] protected float moveSpeed;
    protected int facingDir = -1;
    protected bool facingRight = false;
    protected bool isGroundDetected;
    protected bool isWallDetected;
    protected bool isGroundNormal;
    protected bool canMove = true;
    
    protected virtual void Awake() {
        cols = GetComponentsInChildren<BoxCollider2D>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    protected virtual void Start() {
        InvokeRepeating(nameof(UpdatePlayerTransform), 0, .5f);
        if (sr.flipX == true && !facingRight) {
            sr.flipX = false;
            Flip();
        }
    }
    private void UpdatePlayerTransform() {
        if (playerTrans == null) {
            playerTrans = PlayerManager.instance.player.transform;
        }
    }
    protected virtual void Update() {
        HandleAnimator();
        HandleCollision();
        if(isDead)
            HandleDeathRotation();
    }
    protected virtual void HandleFlip(float xValue) {
        if (xValue < transform.position.x && facingRight || xValue > transform.position.x && !facingRight) {
            Flip();
        }
    }
    protected virtual void HandleAnimator() {
        anim.SetFloat("xVelocity", rb.velocity.x);
    }
    protected virtual void Flip() {
        facingDir *= -1;
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }
    public virtual void Die() {
        foreach (var col in cols) {
            col.enabled = false;
        }
        anim.SetTrigger("hit"); 
        rb.velocity = new Vector2(rb.velocity.x, deathImpact);
        if (Random.Range(1, 100) < 50) {
            deathDirectionRotation *= -1;
        }
        isDead = true;
        Destroy(gameObject, 10f);
    }

    [ContextMenu("Flip Enemy Facing")]
    public void FlipEnemyFacing() {
        sr.flipX = !sr.flipX;
    }
    private void HandleDeathRotation() {
        transform.Rotate(0, 0, deathDirectionRotation * deathRotationSpeed * Time.deltaTime);
    }
    protected virtual void HandleCollision() {
        isGroundNormal = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isGroundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
        IsPlayerDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, playerDetectionRange, whatIsPlayer);
    }
    protected virtual void OnDrawGizmos() {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (facingDir * wallCheckDistance), transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (facingDir * playerDetectionRange), transform.position.y));
    }
}

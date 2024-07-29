using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Snail : Enemy
{
    [Header("Snail details")] 
    [SerializeField] private Enemy_Snail_Body bodyPrefab;
    [SerializeField] private float shellSpeed = 10f;
    private bool hasBody = true;
    protected override void Update() {
        base.Update();
        if (isDead)
            return;

        HandleMovement();

        if (isGroundNormal)
            HandleGround();
    }
    private void HandleGround() {
        if (!isGroundDetected || isWallDetected) {
            Flip();
        }
    }
    public override void Die()
    {
        if (hasBody) {
            canMove = false;
            hasBody = false;
            anim.SetTrigger("hit");
            rb.velocity = Vector2.zero;
        }
        else if (canMove == false && hasBody == false){
            anim.SetTrigger("hit");
            canMove = true;
            moveSpeed = shellSpeed;
        }
        else{
            base.Die();
        }
    }
    private void HandleMovement() {
        if (canMove == false)
            return;
        rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y);
    }
    private void CreateBody() {
        Enemy_Snail_Body newBody = Instantiate(bodyPrefab, transform.position, Quaternion.identity); 
        if (Random.Range(1, 100) < 50) {
            deathDirectionRotation = deathDirectionRotation * -1;
        }
        newBody.SetupBody(deathImpact, deathRotationSpeed * deathDirectionRotation, facingDir); 
        Destroy(newBody.gameObject, 10f);
    }
    protected override void Flip()
    {
        base.Flip();
        if (hasBody == false) {
            anim.SetTrigger("wallHit");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Chicken : Enemy
{
    [Header("Chicken detail")]
    [SerializeField] private float aggroDuration;
    private float aggroTimer;
    private bool canFlip = true;

    protected override void Update() {
        base.Update();
        aggroTimer -= Time.deltaTime;

        if (isDead)
            return;
        if (IsPlayerDetected) {
            canMove = true;
            aggroTimer = aggroDuration;
        }
        if (aggroTimer < 0) {
            canMove = false;
        }

        HandleMovement();
        HandleGround();
    }
    private void HandleGround() {
        if (!isGroundDetected || isWallDetected) {
            Flip();
            canMove = false;
        }
    }
    private void HandleMovement() {
        if (canMove == false)
            return;

        HandleFlip(playerTrans.position.x);
        rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y);
    }
    protected override void HandleFlip(float xValue)
    {
        if (xValue < transform.position.x && facingRight || xValue > transform.position.x && !facingRight) {
            if (canFlip) {
                canFlip = false;
                Invoke(nameof(Flip), .3f);
            }
        }
    }
    protected override void Flip()
    {
        base.Flip();
        canFlip = true;
    }
}

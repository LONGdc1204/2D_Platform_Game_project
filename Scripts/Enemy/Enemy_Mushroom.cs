using UnityEngine;

public class Enemy_Mushroom : Enemy
{
    protected override void Update() {
        base.Update();
        if (isDead)
            return;

        HandleMovement();

        if (isGroundNormal)
            HandleTurnAround();
    }
    private void HandleTurnAround() {
        if (!isGroundDetected || isWallDetected) {
            Flip();
        }
    }
    private void HandleMovement() {
        rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y);
    }
}

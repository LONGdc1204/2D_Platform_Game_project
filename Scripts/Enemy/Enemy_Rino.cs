using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rino : Enemy
{
    [Header("Rino detail")]
    [SerializeField] private float defaultSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private Vector2 wallPower;

    protected override void Start()
    {
        base.Start();
        canMove = false;
        moveSpeed = defaultSpeed;
    }
    protected override void Update()
    {
        base.Update(); 
        if (isDead)
            canMove = false;
        HandleMovement();
    }
    private void HandleMovement()
    {
        if (canMove == false)
            return;

        HandleSpeedUp();
        rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y);

        if (!isGroundDetected)
        {
            TurnAround();
        }
        if (isWallDetected)
        {
            WallHit();
        }
    }

    private void HandleSpeedUp()
    {
        moveSpeed = moveSpeed + Time.deltaTime;
        if (moveSpeed > maxSpeed)
        {
            moveSpeed = maxSpeed;
        }
    }

    private void TurnAround()
    {
        canMove = false;
        rb.velocity = Vector2.zero;
        ResetSpeed();
        Flip();
    }

    private void ResetSpeed()
    {
        moveSpeed = defaultSpeed;
    }

    private void WallHit() {
        anim.SetBool("hitWall", true);
        rb.velocity = new Vector2(wallPower.x * -facingDir, wallPower.y);
        ResetSpeed();
    }
    private void HitWallOver() {
        canMove = false;
        anim.SetBool("hitWall", false);
        Invoke(nameof(Flip), 1);
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();
        if (IsPlayerDetected)
            canMove = true;
    }
}

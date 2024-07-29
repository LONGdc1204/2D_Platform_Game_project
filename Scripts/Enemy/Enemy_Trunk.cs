using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Trunk : Enemy
{
    [Header("Trunk detais")]
    [SerializeField] private float attackCooldown = 1.2f;
    [SerializeField] private float bulletSpeed = 12f;
    [SerializeField] Transform gunPoint;
    [SerializeField] private Enemy_Bullet bulletPrefab;
    private float lasttimeAttacked;
    private float attackToIdleTime;
    [SerializeField] private float attackToIdleDuration;
    protected override void Update()
    {   
        base.Update();
        attackToIdleTime -= Time.deltaTime;
        if (isDead)
            return;
        HandleMoveMent();
        if (isGroundNormal) {
            HandleGround();
        }
        // Hoi chieu cua tan cong
        bool canAttack = Time.time > lasttimeAttacked + attackCooldown;
        if (IsPlayerDetected && canAttack)
        {
            Attack();
        }
        if (!IsPlayerDetected) {
            canMove = true;
        }
    }
    private void SetupBullet() {
        Enemy_Bullet newbullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity);
        Vector2 bulletVelocity = new Vector2(bulletSpeed * facingDir, 0);
        newbullet.SetVelocity(bulletVelocity);
        if (facingRight) {
            newbullet.FlipBullet();
        }
        Destroy(newbullet.gameObject, 10f);
    }

    // Tan cong nguoi choi
    private void Attack()
    {
        canMove = false;
        attackToIdleTime = attackToIdleDuration;
        lasttimeAttacked = Time.time;
        anim.SetTrigger("attack");
    }
    private void HandleGround() {
        if (!isGroundDetected || isWallDetected) {
            Flip();
        }
    }
    private void HandleMoveMent()
    {
        if (canMove && attackToIdleTime < 0)
            rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y);
    }
}

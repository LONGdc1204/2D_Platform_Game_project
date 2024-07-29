using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Plant : Enemy
{
    [Header("Plant detais")]
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float bulletSpeed = 12f;
    [SerializeField] Transform gunPoint;
    [SerializeField] private Enemy_Bullet bulletPrefab;
    private float lasttimeAttacked;
    protected override void Update()
    {
        base.Update();
        // Hoi chieu cua tan cong
        bool canAttack = Time.time > lasttimeAttacked + attackCooldown;
        if (IsPlayerDetected && canAttack)
        {
            Attack();
        }
    }
    private void SetupBullet() {
        Enemy_Bullet newbullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity);
        Vector2 bulletVelocity = new Vector2(bulletSpeed * facingDir, 0);
        newbullet.SetVelocity(bulletVelocity);
        Destroy(newbullet.gameObject, 10f);
    }

    // Tan cong nguoi choi
    private void Attack()
    {
        lasttimeAttacked = Time.time;
        anim.SetTrigger("attack");
    }
    protected override void HandleAnimator()
    {
        // Keep it emty
    }
}

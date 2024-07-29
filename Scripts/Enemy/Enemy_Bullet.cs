using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    public void FlipBullet() {
        sr.flipX = !sr.flipX;
    }
    public void SetVelocity(Vector2 bulletVelocity) => rb.velocity = bulletVelocity;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            other.GetComponent<Player>().Knockback(transform.position.x);
            Destroy(gameObject);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            Destroy(gameObject, 0.1f);
        }
    }
}
 
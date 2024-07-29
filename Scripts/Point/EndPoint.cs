using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private void OnTriggerEnter2D(Collider2D other) {
        Player player = other.GetComponent<Player>();
        if (player != null) {
            AudioManager.instance.PlaySFX(2);
            anim.SetTrigger("endActivate");
            GameManager.instance.LevelFinish();
        }
    }
}

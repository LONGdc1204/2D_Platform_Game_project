using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Checkpoint : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private bool activate;

    // Kich hoat checkpoint 1 lan khi player cham vao
    private void OnTriggerEnter2D(Collider2D other) {
        if (activate)
            return;
        Player player = other.GetComponent<Player>();
        if (player != null) {
            ActivateCheckpoint();
            activate = false;
        }
    }

    // Kich hoat animation checkpoint
    private void ActivateCheckpoint() {
        activate = true;
        anim.SetTrigger("activate");
        PlayerManager.instance.UpdateRepawnPosition(transform);
    }
}

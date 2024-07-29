using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrapButton : MonoBehaviour
{
    private Animator anim;
    private FireTrap fireTrap;

    private void Awake() {
        anim = GetComponent<Animator>();
        fireTrap = GetComponentInParent<FireTrap>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null) {
            anim.SetTrigger("active");
            fireTrap.SwitchOffFire();
        }
    }
}

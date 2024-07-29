using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineTrap : MonoBehaviour
{
    [SerializeField] private float pushPower;
    [SerializeField] private float duration = .5f;
    protected Animator anim => GetComponent<Animator>();
    private void OnTriggerEnter2D(Collider2D other) {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null) {
            player.Push(transform.up * pushPower, duration);
            anim.SetTrigger("active");
        }
    }
}

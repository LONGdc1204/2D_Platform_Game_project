using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    private Animator anim;
    private CapsuleCollider2D fireCollider;
    private bool isActivated;
    [SerializeField] private float offDuration;
    private void Awake() {
        anim = GetComponent<Animator>();
        fireCollider = GetComponent<CapsuleCollider2D>();
    }
    private void Start() {
        SetFire(true);
    }
    public void SwitchOffFire() {
        if (isActivated == false)
            return;
        StartCoroutine(FireCoroutine());
    }
    private IEnumerator FireCoroutine() {
        SetFire(false);
        yield return new WaitForSeconds(offDuration);
        SetFire(true);
    }
    private void SetFire(bool active) {
        anim.SetBool("active", active);
        fireCollider.enabled = active;
        isActivated = active;
    }
}

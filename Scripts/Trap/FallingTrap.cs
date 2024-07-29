using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FallingTrap : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D[] colliders;
    private Animator anim;  

    [SerializeField] private FallingTrapManager fallingManager;
    [SerializeField] private float positionX;
    private int index;
    private float positionXValue = 59.38344f;

    private float speed;
    [SerializeField] private float distance;
    private Vector3[] waypoints;
    [SerializeField] private int addPositionX = 0;
    private bool canMove = true;

    [Header("Falling details")]
    private float fallingDelay = .5f;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        colliders = GetComponents<BoxCollider2D>();
        anim = GetComponent<Animator>();
        positionX = transform.position.x;
    }
    void Start()
    {   
        if (positionX == positionXValue) {
            addPositionX = 0;
        }
        else if (positionX == positionXValue + 5) {
            addPositionX = 5;
        }
        else {
            addPositionX = 10;
        }

        SetupWaypoints();
        
        if (fallingManager == null) {
            fallingManager = GameObject.Find("Falling Trap Manager").GetComponent<FallingTrapManager>();
        }
    }
    private void Update() {
        HandleMovement();
    }

    private void SetupWaypoints() {
        waypoints = new Vector3[2];
        distance = Random.Range(1f, 2f);
        speed = Random.Range(1f, 1.5f);
        float yOffset = distance / 2;
        waypoints[0] = transform.position + new Vector3(0, yOffset, 0);
        waypoints[1] = transform.position + new Vector3(0, -yOffset, 0);
    }
    private void HandleMovement() {
        if (canMove == false)
            return;
        transform.position = Vector2.MoveTowards(transform.position, waypoints[index], speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, waypoints[index]) < .1f) {
            index++;
            if (index >= waypoints.Length) {
                index = 0;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null) {
            Invoke(nameof(FallingActivate), fallingDelay);
            fallingManager.ResetFallingTrap(addPositionX);
        }
    }
    private void FallingActivate() {
        canMove = false;
        anim.SetTrigger("deActive");
        rb.isKinematic = false;
        rb.gravityScale = 3.5f;
        rb.drag = .5f;
        foreach(BoxCollider2D collider in colliders) {
            collider.enabled = false;
        }
        Destroy(gameObject, 5f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum FruitType {Apple, Bananas, Cherries, Kiwi, Melon, Orange, Pineapple, Strawberry}

public class Fruit : MonoBehaviour
{
    [SerializeField] private FruitType fruitType;
    [SerializeField] private GameObject pickupVfx;
    private GameManager gameManager;
    private Animator anim;
    private void Awake() {
        anim = GetComponentInChildren<Animator>();
    }
    private void Start() {
        gameManager = GameManager.instance;
        UpdateFruitVisual();
    }
    // Destroy fruit va tang diem khi player cham vao fruit
    private void OnTriggerEnter2D(Collider2D other) {
        Player player = other.GetComponent<Player>();
        if (player != null) {
            gameManager.AddFruit();
            Destroy(gameObject);
            GameObject newVfx = Instantiate(pickupVfx, transform.position, Quaternion.identity); // KO thay doi goc quay
            AudioManager.instance.PlaySFX(8, true);
        }
    }
    // Tao animation cho fruit
    private void UpdateFruitVisual() => anim.SetFloat("fruitIndex", (int)fruitType);
}

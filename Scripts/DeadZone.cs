using UnityEngine;

public class DeadZone : MonoBehaviour
{   
    // Tieu diet player khi cham vao deadzone
    private void OnTriggerEnter2D(Collider2D other) {
        Player player = other.GetComponent<Player>();
        if (player != null) {
            player.Die();
            PlayerManager.instance.RespawnPlayer();
        }
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.Die();
        }
    }
}

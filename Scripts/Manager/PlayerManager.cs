using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    [Header("Player")]
    [SerializeField] private GameObject playerRefabs;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnDelay;
    public Player player;

    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start() {
        if (respawnPoint == null) {
            respawnPoint = FindObjectOfType<StartPoint>().transform;
        }
        if (player == null) {
            player = FindObjectOfType<Player>();
        }
    }

    // Hoi sinh player sau 1 khoang time (sau khi bi tieu diet)
    private IEnumerator RespawnPlayerRoutine() {
        yield return new WaitForSeconds(respawnDelay);
        GameObject newPlayer = Instantiate(playerRefabs, respawnPoint.position, Quaternion.identity);
        player = newPlayer.GetComponent<Player>();
    }
    public void UpdateRepawnPosition(Transform newRespawnPoint) => respawnPoint = newRespawnPoint;

    public void RespawnPlayer() {
        StartCoroutine(RespawnPlayerRoutine());
    }
}

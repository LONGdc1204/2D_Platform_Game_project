using System.Collections;
using UnityEngine;

public class FallingTrapManager : MonoBehaviour
{
    [SerializeField] GameObject fallingTrapPrefab;
    private void Start() {
        Instantiate(fallingTrapPrefab, new Vector3(transform.position.x + 0, transform.position.y, transform.position.z), Quaternion.identity);
        Instantiate(fallingTrapPrefab, new Vector3(transform.position.x + 5, transform.position.y, transform.position.z), Quaternion.identity);
        Instantiate(fallingTrapPrefab, new Vector3(transform.position.x + 10, transform.position.y, transform.position.z), Quaternion.identity);
    }
    public void ResetFallingTrap(float addPositionX) {
        StartCoroutine(ResetFallingTrapRoutine(addPositionX));
    }
    private IEnumerator ResetFallingTrapRoutine(float addPositionX) {
        yield return new WaitForSeconds(5f);
        Instantiate(fallingTrapPrefab, new Vector3(transform.position.x + addPositionX, transform.position.y, transform.position.z), Quaternion.identity);
    }

}

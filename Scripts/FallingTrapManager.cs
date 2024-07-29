using System.Collections;
using UnityEngine;

public class FallingTrapManager : MonoBehaviour
{
    [SerializeField] GameObject fallingTrapPrefab;
    
    public void ResetFallingTrap() {
        StartCoroutine(ResetFallingTrapRoutine());
    }
    private IEnumerator ResetFallingTrapRoutine() {
        yield return new WaitForSeconds(6f);
        Instantiate(fallingTrapPrefab, transform.position, Quaternion.identity);
    }

}

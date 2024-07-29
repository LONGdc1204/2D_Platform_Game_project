using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBallTrap : MonoBehaviour
{
    [SerializeField] private float pushVector;
    [SerializeField] private Transform trans;
    [SerializeField] private float rotationZ;
    private float valueRotation;

    private void Start() {
        valueRotation = pushVector;
    }
    private void Update() {
        RotationSpikeBall();
    }
    private void RotationSpikeBall() {
        if (trans.rotation.z >= rotationZ) {
            valueRotation = -pushVector;
        }
        if (trans.rotation.z < -rotationZ) {
            valueRotation = pushVector;
        }
        trans.Rotate(0, 0, valueRotation * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LevelCamera : MonoBehaviour
{
    private CinemachineVirtualCamera cineMachineCamera;
    [SerializeField] private Transform playerTrans;

    private void Awake() {
        cineMachineCamera = GetComponent<CinemachineVirtualCamera>();
    }
    private void Start() {
        InvokeRepeating(nameof(SetupCamera), 0, 0.5f);
    }
    private void SetupCamera() {
        if (playerTrans == null) {
            playerTrans = PlayerManager.instance.player.transform;
        }
        cineMachineCamera.Follow = playerTrans;
    } 
}
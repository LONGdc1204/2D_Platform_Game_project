using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    private CinemachineImpulseSource impulseSource;
    [SerializeField] private Vector2 shakeVelocity;

    private void Start() {
        instance = this;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    public void CameraShake() {
        impulseSource.m_DefaultVelocity = new Vector2(shakeVelocity.x, shakeVelocity.y);
        impulseSource.GenerateImpulse();
    }
}

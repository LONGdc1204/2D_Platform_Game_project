using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BackgroundType {Blue, Brown, Gray, Green, Pink, Purple, Yellow}

public class AnimatedBackground : MonoBehaviour
{
    [SerializeField] private Vector2 movementDirection;
    private MeshRenderer mesh;
    [Header("Color")]
    [SerializeField] BackgroundType backgroundType;
    [SerializeField] Texture2D[] textures;

    private void Awake() {
        mesh = GetComponent<MeshRenderer>();
        UpdateBackground();
    }
    private void Update() {
        // Di chuyen texture
        mesh.material.mainTextureOffset += movementDirection * Time.deltaTime;
    }
    [ContextMenu("Update Background")]
    // Thay doi backgrounds 
    private void UpdateBackground() {
        if (mesh == null) {
            mesh = GetComponent<MeshRenderer>();
        }
        mesh.sharedMaterial.mainTexture = textures[(int)backgroundType];
    }

}

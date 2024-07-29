using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Credits : MonoBehaviour
{
    private UI_FadeEffect fadeEffect;
    [SerializeField] private RectTransform recT;
    [SerializeField] private float scrollSpeed = 200f;
    [SerializeField] private bool ScriptsSkipped; 
    [SerializeField] private float offScreenPosition = 1800f;

    private void Awake() {
        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
        fadeEffect.ScreenFade(0, 1); 
    }
    private void Update() {
        recT.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        if (recT.anchoredPosition.y > offScreenPosition)
            GotoMainMenu();
    }
    public void SkipCredits() {
        if (ScriptsSkipped == false) {
            scrollSpeed *= 10;
            ScriptsSkipped = true;
        }
        else
        {
            GotoMainMenu();
        }
    }
    private void GotoMainMenu() => fadeEffect.ScreenFade(1, 1.5f, GoToMenuScreen);
    // Quay ve Menu Screen
    private static void GoToMenuScreen()
    {
        SceneManager.LoadScene("MenuScene");
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Ingame : MonoBehaviour
{
    public static UI_Ingame instance;
    public UI_FadeEffect fadeEffect {get; private set;} //read only 
    private bool isPaused;
    [SerializeField] private TextMeshProUGUI fruitText;
    [SerializeField] private GameObject Pause_UI;
    [SerializeField] private GameObject[] hearts_UI;
    private void Awake() {
        instance = this;
        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    }
    private void Update() {
        // Neu nhan P thi pause/resume game
        if(Input.GetKeyDown(KeyCode.P)) {
            PauseButton();
        }
    }
    private void Start() {
        fadeEffect.ScreenFade(0, 1);
    }

    // Setup Pause game button
    public void PauseButton() {
        if (isPaused) {
            isPaused = false;
            Pause_UI.SetActive(isPaused);
            Time.timeScale = 1;
        }
        else {
            isPaused = true;
            Pause_UI.SetActive(isPaused);
            Time.timeScale = 0;
        }
    }
    public void GoToMainMenu() {
        SceneManager.LoadScene(0);
    }

    // Hien fruit thu duoc
    public void UpdateFruitsCollected(int fruitsCollected, int totalFruits) {
        fruitText.text = fruitsCollected + "/" + totalFruits;
    }

    // Hien thi thanh mau (heart)
    public void UpdateHeart(int countHeart) {
        hearts_UI[countHeart].SetActive(false);
    }

    // Reset thanh mau
    public void ResetHeart() {
        foreach(GameObject heart in hearts_UI) {
            heart.SetActive(true);
        }
    }
}

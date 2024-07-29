using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Level Manager")]
    [SerializeField] private int currentLevelIndex;
    private int nextLevelIndex;

    [Header("Fruits Management")]
    public int fruitsCollected;
    public int totalFruits;

    [Header("Manager")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private PlayerManager playerManager; 

    // Khoi tao instance
    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start() {
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        nextLevelIndex = currentLevelIndex + 1;
        CollectedFruitInfo();
        CreateAudioManager();
        CreatePlayerManager();
    }

    private void CreateAudioManager() {
        if (AudioManager.instance == null) {
            Instantiate(audioManager);
        }
    }
    private void CreatePlayerManager() {
        if (PlayerManager.instance == null) {
            Instantiate(playerManager);
        }
    }
    private void CollectedFruitInfo() {
        Fruit[] fruits = FindObjectsOfType<Fruit>();
        totalFruits = fruits.Length;
        UI_Ingame.instance.UpdateFruitsCollected(fruitsCollected, totalFruits);
    }

    // Tang diem khi an fruit
    public void AddFruit() {
        fruitsCollected ++;
        UI_Ingame.instance.UpdateFruitsCollected(fruitsCollected, totalFruits);
    }

    private void LoadNextLevel() {
        SceneManager.LoadScene("SceneLv" + nextLevelIndex);
    }
    private void LoadTheEndScene() => SceneManager.LoadScene("TheEnd");
    public void LevelFinish()
    {
        LoadNextScene();
        SaveLevelProgression();
        SaveFruitCollected();
    }

    private void SaveFruitCollected() {
        // Lay collected fruit da luu truoc do
        int fruitsCollectedBefore = PlayerPrefs.GetInt("Level" + currentLevelIndex + "collected fruits");
        // Neu thu thap duoc nhieu fruit hon thi luu lai gia tri moi
        if (fruitsCollected > fruitsCollectedBefore) {
            PlayerPrefs.SetInt("Level" + currentLevelIndex + "collected fruits", fruitsCollected);
        }
        PlayerPrefs.SetInt("Level" + currentLevelIndex + "total fruits", totalFruits);
    }
    private void SaveLevelProgression()
    {
        PlayerPrefs.SetInt("Level" + nextLevelIndex + "Unlocked", 1);
        if (NoMoreLevels() == false)
        {
            PlayerPrefs.SetInt("ContinueLevelNumber", nextLevelIndex);
        }
    }

    private void LoadNextScene() {
        if (NoMoreLevels())
            UI_Ingame.instance.fadeEffect.ScreenFade(1, 1.5f, LoadTheEndScene);
        else {
            UI_Ingame.instance.fadeEffect.ScreenFade(1, 1.5f, LoadNextLevel);
        }
    }
    private bool NoMoreLevels() {
        bool noMoreLevels = currentLevelIndex + 2 == SceneManager.sceneCountInBuildSettings;
        return noMoreLevels;
    }
}

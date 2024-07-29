using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LevelSelection : MonoBehaviour
{
    [SerializeField] private UI_LevelButton buttonPrefab;
    [SerializeField] Transform content;
    [SerializeField] private bool[] levelsUnlocked;

    private void Start() {
        LoadLevelInfo();
        CreateLevelButton();
    }

    // Tao level button = so luong scene (ngoai tru endScene va MenuScene)
    private void CreateLevelButton() {
        int levelAmount = SceneManager.sceneCountInBuildSettings;
        for (int i = 1; i < levelAmount - 1; i++) {
            // Neu level chua duoc choi thi khong tao button
            if (IsLevelUnlocked(i) == false)
                return;
            // Tao button cho level
            UI_LevelButton newButton = Instantiate(buttonPrefab, content);
            newButton.SetupButton(i); 
        }
    }

    // Lay thong tin level da luu trong levelsUnlocked
    private bool IsLevelUnlocked(int levelIndex) => levelsUnlocked[levelIndex];
    
    // Luu thong tin level cua nguoi choi truoc do
    private void LoadLevelInfo() {
        int levelAmount = SceneManager.sceneCountInBuildSettings;
        levelsUnlocked = new bool[levelAmount];
        for (int i = 1; i < levelAmount; i++) {
            bool levelUnlocked = PlayerPrefs.GetInt("Level" + i + "Unlocked", 0) == 1;
            if (levelUnlocked) {
                levelsUnlocked[i] = true;
            } 
        }
        levelsUnlocked[1] = true;
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LevelButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelNumberText;
    [SerializeField] TextMeshProUGUI fruitButtonText;
    private string sceneLevelName;
    private int levelIndex;

    // Load level khi nhan button
    public void LoadLevel() {
        SceneManager.LoadScene(sceneLevelName);
        AudioManager.instance.PlaySFX(4);
    }
    // Setup Button
    public void SetupButton(int newLevelIndex) {
        this.levelIndex = newLevelIndex;
        levelNumberText.text = "Level " + levelIndex;
        fruitButtonText.text = FruitInfoText();
        sceneLevelName = "SceneLv" + levelIndex;
    }
    private string FruitInfoText() {
        int totalFruits = PlayerPrefs.GetInt("Level" + levelIndex + "total fruits", 0);
        string totalFruitsText = totalFruits == 0 ? "?" : totalFruits.ToString();
        int collectedFruits = PlayerPrefs.GetInt("Level" + levelIndex + "collected fruits");
        return "Fruits: " + collectedFruits + " / " + totalFruitsText;
    }
}

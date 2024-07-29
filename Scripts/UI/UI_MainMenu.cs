using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{ 
    [SerializeField] string sceneName;
    [SerializeField] private GameObject[] uiElements;
    [SerializeField] private GameObject continueButton;
    private UI_FadeEffect fadeEffect;
    private void Awake() {
        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    } 
    private void Start() {
        fadeEffect.ScreenFade(0, 1.5f);
        if (HasLevelProgression()) {
            continueButton.SetActive(true);
        }
    }
    public void SwitchUI(GameObject uiToEnable) {
        foreach (GameObject ui in uiElements) {
            ui.SetActive(false);
        }
        uiToEnable.SetActive(true);
        AudioManager.instance.PlaySFX(4);
    }
    private bool HasLevelProgression() {
        bool hasLevelProgression = PlayerPrefs.GetInt("ContinueLevelNumber", 0) > 0;
        return hasLevelProgression;
    }
    public void ContinueButton() {
        AudioManager.instance.PlaySFX(4);
        int levelToLoad = PlayerPrefs.GetInt("ContinueLevelNumber");
        SceneManager.LoadScene("SceneLv" + levelToLoad);
    }
}
 
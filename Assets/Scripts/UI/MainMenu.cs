using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button settingsButton;
    [SerializeField]
    private Button quitButton;
    [SerializeField]
    private SettingsMenu settingMenu;
    [SerializeField]
    private SaveFileMenu saveFileMenu;
    [SerializeField]
    private PlayMenu playMenu;

    private void Awake() {
        playButton.onClick.AddListener(() => {
            saveFileMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });
        settingsButton.onClick.AddListener(() => {
            settingMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });
        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });
        if(GameState.instance.IsGameSaveDataLoaded()) {
            playMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}

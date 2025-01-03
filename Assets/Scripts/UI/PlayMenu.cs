using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour {
    [SerializeField]
    private Button goBackButton;
    [SerializeField]
    private Button shopMenuButton;
    [SerializeField]
    private MainMenu mainMenu;
    [SerializeField]
    private SaveFileMenu saveFileMenu;
    [SerializeField]
    private ShopMenu shopMenu;
    [SerializeField]
    private SceneLoadingScreen sceneLoadingScreen;

    private LevelSelectButton[] levelSelectButtons;

    private void Awake() {
        goBackButton.onClick.AddListener(() => {
            GameState.instance.ClearSaveData();
            saveFileMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });
        shopMenuButton.onClick.AddListener(() => {
            shopMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });
        levelSelectButtons = GetComponentsInChildren<LevelSelectButton>();
        foreach(var button in levelSelectButtons) {
            button.button.onClick.AddListener(() => {
                StartCoroutine(LoadSceneAsync(button.levelName));
            });
        }
    }

    private void OnEnable() {
        if(!GameState.instance.IsGameSaveDataLoaded()) {
            mainMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
            return;
        }
        bool firstLevelCompleted = false;
        foreach(var button in levelSelectButtons) {
            var data = GameState.instance.GetLevelData(button.levelName);
            if(button.levelName == "Level1") {
                firstLevelCompleted = data.completed;
                button.button.interactable = true;
                button.starImage.enabled = firstLevelCompleted;
            }
            else if(!firstLevelCompleted) {
                button.button.interactable = false;
                button.starImage.enabled = false;
            }
            else {
                button.button.interactable = true;
                button.starImage.enabled = data.completed;
            }
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName) {
        var operation = SceneManager.LoadSceneAsync(sceneName);
        sceneLoadingScreen.gameObject.SetActive(true);
        sceneLoadingScreen.SetLevelName(sceneName);
        while(!operation.isDone) {
            sceneLoadingScreen.SetProgress(operation.progress);
            yield return null;
        }
    }
}
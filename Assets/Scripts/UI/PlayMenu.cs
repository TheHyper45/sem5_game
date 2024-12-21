using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour {
    [Serializable]
    public struct LevelButton {
        public Button button;
        public string levelName;
    }

    [SerializeField]
    private Button goBackButton;
    [SerializeField]
    private MainMenu mainMenu;
    [SerializeField]
    private SceneLoadingScreen sceneLoadingScreen;
    [SerializeField]
    private LevelButton[] levels;

    private void Awake() {
        goBackButton.onClick.AddListener(() => {
            mainMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });
        foreach(var item in levels) {
            item.button.onClick.AddListener(() => {
                StartCoroutine(LoadSceneAsync(item.levelName));
            });
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
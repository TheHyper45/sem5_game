using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelCompletedMenu : MonoBehaviour {
    [SerializeField]
    private Button goBackButton;

    private void Awake() {
        goBackButton.onClick.AddListener(() => {
            Time.timeScale = 1f;
            GameState.instance.MarkLevelCompleted(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("MainMenu");
        });
    }
}
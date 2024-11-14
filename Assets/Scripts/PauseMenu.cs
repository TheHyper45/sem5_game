using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private Button resumeButton;
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button leaveButton;

    private void Awake() {
        pauseMenu.SetActive(false);
        resumeButton.onClick.AddListener(() => {
            pauseMenu.SetActive(false);
        });
        restartButton.onClick.AddListener(() => {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        leaveButton.onClick.AddListener(() => {
            SceneManager.LoadScene("LevelSelectionMenu");
        });
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }
}

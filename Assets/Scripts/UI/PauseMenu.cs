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
            Time.timeScale = 1.0f;
            Cursor.lockState = CursorLockMode.Locked;
        });
        restartButton.onClick.AddListener(() => {
            Time.timeScale = 1.0f;
            Cursor.lockState = CursorLockMode.Locked;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        leaveButton.onClick.AddListener(() => {
            Time.timeScale = 1.0f;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("MainMenu");
        });
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            Time.timeScale = pauseMenu.activeSelf ? 0.0f : 1.0f;
            Cursor.lockState = pauseMenu.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}

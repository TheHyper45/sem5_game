using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectionMenu : MonoBehaviour {
    [SerializeField]
    private Button returnButton;
    [SerializeField]
    private Button level1Button;

    private void Awake() {
        returnButton.onClick.AddListener(() => {
            SceneManager.LoadScene("MainMenu");
        });
        level1Button.onClick.AddListener(() => {
            SceneManager.LoadScene("Level1");
        });
    }
}

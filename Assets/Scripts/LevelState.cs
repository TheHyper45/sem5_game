using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelState : MonoBehaviour {
    [SerializeField]
    private string levelName;

    private void Start() {
        SceneManager.LoadScene("GameUI",LoadSceneMode.Additive);
    }
}

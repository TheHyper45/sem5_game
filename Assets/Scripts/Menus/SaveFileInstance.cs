using TMPro;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveFileInstance : MonoBehaviour {
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private Button selectButton;
    [SerializeField]
    private Button deleteButton;
    private string filePath;

    private void Awake() {
        selectButton.onClick.AddListener(() => {
            GameState.instance.currentSaveFilePath = filePath;
            SceneManager.LoadScene("GameMenu");
        });
        deleteButton.onClick.AddListener(() => {
            Utils.Noexcept(() => { File.Delete(filePath); });
            Destroy(gameObject);
        });
    }

    public void Init(string _filePath) {
        filePath = _filePath;
        nameText.text = $"{Path.GetFileNameWithoutExtension(filePath)}\n{File.GetCreationTime(filePath)}";
    }
}

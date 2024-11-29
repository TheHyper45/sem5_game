using TMPro;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LevelSaveElement : MonoBehaviour {
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private Button selectButton;
    [SerializeField]
    private Button deleteButton;
    private string filePath;
    private SaveFileMenu saveFileMenu;
    private LevelSelectionMenu levelSelectionMenu;

    private void Awake() {
        selectButton.onClick.AddListener(() => {
            saveFileMenu.gameObject.SetActive(false);
            levelSelectionMenu.gameObject.SetActive(true);
        });
        deleteButton.onClick.AddListener(() => {
            File.Delete(filePath);
            Destroy(gameObject);
        });
    }

    public void Init(string _filePath,SaveFileMenu _saveFileMenu,LevelSelectionMenu _levelSelectionMenu) {
        filePath = _filePath;
        saveFileMenu = _saveFileMenu;
        levelSelectionMenu = _levelSelectionMenu;
        nameText.text = $"{Path.GetFileNameWithoutExtension(filePath)}\n{File.GetCreationTime(filePath)}";
    }
}

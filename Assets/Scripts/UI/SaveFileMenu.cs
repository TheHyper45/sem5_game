using TMPro;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveFileMenu : MonoBehaviour {
    [SerializeField]
    private MainMenu mainMenu;
    [SerializeField]
    private PlayMenu playMenu;
    [SerializeField]
    private Button goBackToMainMenuButton;
    [SerializeField]
    private Button createNewSaveFileButton;
    [SerializeField]
    private TMP_InputField newSaveFileNameInput;
    [SerializeField]
    private Transform saveFileInstanceUIParent;
    [SerializeField]
    private SaveFileInstanceUI saveFileInstanceUIPrefab;

    private void Awake() {
        goBackToMainMenuButton.onClick.AddListener(() => {
            mainMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });
        createNewSaveFileButton.onClick.AddListener(() => {
            var text = newSaveFileNameInput.text;
            if(string.IsNullOrEmpty(text)) return;
            var filePath = $"{Application.persistentDataPath}/{text}.json";
            SaveFile.CreateNew(filePath);
            InstantiateSaveFileInstanceUI(filePath);
            newSaveFileNameInput.text = "";
        });
    }

    private void OnEnable() {
        newSaveFileNameInput.text = "";
        foreach(var element in saveFileInstanceUIParent.GetComponentsInChildren<SaveFileInstanceUI>()) {
            Destroy(element.gameObject);
        }
        foreach(var filePath in Directory.EnumerateFiles(Application.persistentDataPath)) {
            InstantiateSaveFileInstanceUI(filePath);
        }
    }

    private void InstantiateSaveFileInstanceUI(string filePath) {
        var instance = Instantiate(saveFileInstanceUIPrefab,saveFileInstanceUIParent);
        instance.saveFileNameText.text = Path.GetFileNameWithoutExtension(filePath);
        instance.loadSaveFileButton.onClick.AddListener(() => {
            SaveFile.Load(filePath);
            playMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });
        instance.deleteSaveFileButton.onClick.AddListener(() => {
            File.Delete(filePath);
            Destroy(instance.gameObject);
        });
    }
}
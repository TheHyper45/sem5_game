using TMPro;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveFileMenu : MonoBehaviour {
    [SerializeField]
    private MainMenu mainMenu;
    [SerializeField]
    private Button goBackButton;
    [SerializeField]
    private Button createNewSaveButton;
    [SerializeField]
    private TMP_InputField saveFileNameInput;
    [SerializeField]
    private SaveFileInstance saveFileInstancePrefab;
    [SerializeField]
    private Transform levelSaveElementParent;
    [SerializeField]
    private TMP_Text errorText;

    private void CreateNewSaveFile(string fileName) {
        var filePath = Path.Combine(Application.persistentDataPath,$"{fileName}.json");
        File.WriteAllText(filePath,"{}");
        SaveFileInstance element = Instantiate(saveFileInstancePrefab,levelSaveElementParent);
        try {
            element.Init(filePath);
        }
        catch(Exception) {
            Destroy(element);
            throw new Exception("File doesn't exist or filesystem error.");
        }
    }

    private void Awake() {
        goBackButton.onClick.AddListener(() => {
            mainMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });
        createNewSaveButton.onClick.AddListener(() => {
            errorText.text = "";
            if(string.IsNullOrEmpty(saveFileNameInput.text)) {
                errorText.text = "File name is empty.";
                return;
            }
            if(File.Exists(saveFileNameInput.text)) {
                errorText.text = "File already exists.";
                return;
            }
            try {
                CreateNewSaveFile(saveFileNameInput.text);
            }
            catch(Exception error) {
                errorText.text = error.Message;
            }
            saveFileNameInput.text = "";
        });
    }

    private void OnEnable() {
        errorText.text = "";
        saveFileNameInput.text = "";
        for(int i = 0;i < levelSaveElementParent.childCount;i += 1) {
            Destroy(levelSaveElementParent.GetChild(i).gameObject);
        }
        foreach(var filePath in Directory.EnumerateFiles(Application.persistentDataPath)) {
            if(Path.GetExtension(filePath) == ".json") {
                var element = Instantiate(saveFileInstancePrefab,levelSaveElementParent);
                try {
                    element.Init(filePath);
                }
                catch(Exception) {
                    Destroy(element);
                    errorText.text = "File doesn't exist or filesystem error.";
                }
            }
        }
    }
}

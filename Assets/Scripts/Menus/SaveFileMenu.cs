using TMPro;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveFileMenu : MonoBehaviour {
    [SerializeField]
    private MainMenu mainMenu;
    [SerializeField]
    private LevelSelectionMenu levelSelectionMenu;
    [SerializeField]
    private Button goBackButton;
    [SerializeField]
    private Button createNewSaveButton;
    [SerializeField]
    private TMP_InputField saveFileNameInput;
    [SerializeField]
    private LevelSaveElement levelSaveElementPrefab;
    [SerializeField]
    private Transform levelSaveElementParent;
    [SerializeField]
    private TMP_Text errorText;

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
            var fileName = $"{saveFileNameInput.text}.json";
            var filePath = Path.Combine(Application.persistentDataPath,fileName);
            try {
                File.WriteAllText(filePath,"{}");
            }
            catch(ArgumentException) {
                errorText.text = "Invalid file name.";
                return;
            }
            catch(PathTooLongException) {
                errorText.text = "File name too long.";
                return;
            }
            catch(NotSupportedException) {
                errorText.text = "Invalid file name.";
                return;
            }
            catch(IOException) {
                errorText.text = "Invalid file name.";
                return;
            }
            Instantiate(levelSaveElementPrefab,levelSaveElementParent).Init(filePath,this,levelSelectionMenu);
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
                var element = Instantiate(levelSaveElementPrefab,levelSaveElementParent);
                element.Init(filePath,this,levelSelectionMenu);
            }
        }
    }
}

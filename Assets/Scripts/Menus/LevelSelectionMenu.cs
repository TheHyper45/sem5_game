using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionMenu : MonoBehaviour {
    [SerializeField]
    private MainMenu mainMenu;
    [SerializeField]
    private Button goBackButton;

    private void Awake() {
        goBackButton.onClick.AddListener(() => {
            mainMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });
    }
}

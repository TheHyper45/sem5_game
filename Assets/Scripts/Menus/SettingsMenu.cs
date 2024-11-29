using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {
    [SerializeField]
    private Button goBackButton;
    [SerializeField]
    private GameObject previousMenu;

    private void Awake() {
        goBackButton.onClick.AddListener(() => {
            previousMenu.SetActive(true);
            gameObject.SetActive(false);
        });
    }
}
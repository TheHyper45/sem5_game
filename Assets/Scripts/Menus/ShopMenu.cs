using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour {
    [SerializeField]
    private Button goToLevelSelectionMenuButton;
    [SerializeField]
    private LevelSelectionMenu levelSelectionMenu;

    private void Awake() {
        goToLevelSelectionMenuButton.onClick.AddListener(() => {
            levelSelectionMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });
    }
}

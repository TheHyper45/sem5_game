using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectionMenu : MonoBehaviour {
    [SerializeField]
    private Button goBackButton;
    [SerializeField]
    private Button showShopButton;
    [SerializeField]
    private ShopMenu shopMenu;

    private void Awake() {
        goBackButton.onClick.AddListener(() => {
            GameState.instance.currentSaveFilePath = "";
            SceneManager.LoadScene("MainMenu");
        });
        showShopButton.onClick.AddListener(() => {
            shopMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });
    }
}

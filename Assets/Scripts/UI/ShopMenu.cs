using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class ShopMenu : MonoBehaviour {
    [SerializeField]
    private MainMenu mainMenu;
    [SerializeField]
    private PlayMenu playMenu;
    [SerializeField]
    private Button goBackToPlayMenuButton;
    [SerializeField]
    private TMP_Text moneyText;

    private void Awake() {
        goBackToPlayMenuButton.onClick.AddListener(() => {
            playMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });
        if(!GameState.instance.IsGameSaveDataLoaded()) {
            mainMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
            return;
        }
    }

    private void OnEnable() {
        var moneyString = GameState.instance.GetMoney().ToString(CultureInfo.InvariantCulture);
        moneyText.text = $"Money: {moneyString}";
    }
}
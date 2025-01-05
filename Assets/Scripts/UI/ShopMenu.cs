using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour {
    [SerializeField]
    private MainMenu mainMenu;
    [SerializeField]
    private PlayMenu playMenu;
    [SerializeField]
    private Button goBackToPlayMenuButton;
    [SerializeField]
    private TMP_Text moneyText;
    [SerializeField]
    private Transform shopUpgradeInstanceUIParent;
    [SerializeField]
    private ShopUpgradeInstanceUI shopUpgradeInstanceUIPrefab;

    private void Awake() {
        goBackToPlayMenuButton.onClick.AddListener(() => {
            playMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });
    }

    private void OnEnable() {
        moneyText.text = $"Money: ${SaveFile.GetMoney()}";
        foreach(var element in shopUpgradeInstanceUIParent.GetComponentsInChildren<ShopUpgradeInstanceUI>()) {
            Destroy(element.gameObject);
        }
        foreach(var upgrade in ShopUpgrades.upgrades) {
            InstantiateShopUpgradeInstanceUI(upgrade);
        }
    }

    private void InstantiateShopUpgradeInstanceUI(ShopUpgradeDescription shopUpgrade) {
        var instance = Instantiate(shopUpgradeInstanceUIPrefab,shopUpgradeInstanceUIParent);
        instance.Init(shopUpgrade.label);
        instance.buyButton.onClick.AddListener(() => {
            SaveFile.BuyUpgrade(shopUpgrade.label);
            instance.Init(shopUpgrade.label);
            moneyText.text = $"Money: ${SaveFile.GetMoney()}";
        });
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUpgradeInstanceUI : MonoBehaviour {
    public TMP_Text nameText;
    public TMP_Text costText;
    public TMP_Text levelText;
    public Button buyButton;

    public void Init(string upgradeName) {
        var description = ShopUpgrades.GetUpgradeDescription(upgradeName);
        nameText.text = description.label;

        var currentLevel = SaveFile.GetUpgradeLevel(upgradeName);
        levelText.text = $"Level: {currentLevel}";
        if(currentLevel == description.levels.Length) {
            costText.text = "Bought Out";
            buyButton.interactable = false;
            return;
        }

        costText.text = $"Cost: ${description.levels[currentLevel].cost}";
        buyButton.interactable = true;
    }
}
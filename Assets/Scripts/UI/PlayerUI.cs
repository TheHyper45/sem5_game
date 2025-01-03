using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private TMP_Text collectedMoneyText;

    private float currentValue = 1f;
    private float targetValue = 1f;

    private int cachedCollectedMoney = 0;

    private void Update() {
        if(cachedCollectedMoney != PlayerTank.instance.collectedMoney) {
            cachedCollectedMoney = PlayerTank.instance.collectedMoney;
            collectedMoneyText.text = $"Money: ${cachedCollectedMoney}";
        }
        currentValue = Mathf.Lerp(currentValue,targetValue,Time.deltaTime * 5f);
        healthBar.value = currentValue;
    }

    public void SetHealthPercent(float percent) {
        targetValue = Mathf.Clamp01(percent);
    }
}
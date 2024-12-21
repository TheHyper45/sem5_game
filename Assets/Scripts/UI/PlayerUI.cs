using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    [SerializeField]
    private Slider healthBar;

    private float currentValue = 1f;
    private float targetValue = 1f;

    private void Update() {
        currentValue = Mathf.Lerp(currentValue,targetValue,Time.deltaTime * 3f);
        healthBar.value = currentValue;
    }

    public void SetHealthPercent(float percent) {
        targetValue = Mathf.Clamp01(percent);
    }
}
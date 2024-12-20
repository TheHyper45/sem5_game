using UnityEngine;

public class HealthBar : MonoBehaviour {
    [SerializeField]
    private SpriteRenderer healthBar;
    private readonly Quaternion rotationAdjust = Quaternion.Euler(0f,0f,180f);

    private float initialSizeX;
    private float currentValue = 1f;
    private float targetValue = 1f;

    private void Awake() {
        initialSizeX = healthBar.transform.localScale.x;
    }

    private void Update() {
        transform.rotation = Camera.main.transform.rotation * rotationAdjust;
        currentValue = Mathf.Lerp(currentValue,targetValue,Time.deltaTime * 5f);
        var newPos = healthBar.transform.localPosition;
        newPos.x = Mathf.Lerp(0f,initialSizeX / 2f,1f - currentValue);
        healthBar.transform.localPosition = newPos;
        var newScale = healthBar.transform.localScale;
        newScale.x = Mathf.Lerp(0f,initialSizeX,currentValue);
        healthBar.transform.localScale = newScale;
    }

    public void SetValue(float percent) {
        targetValue = Mathf.Clamp01(percent);
    }
}
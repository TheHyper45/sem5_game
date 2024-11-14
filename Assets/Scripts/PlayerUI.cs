using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour {
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private TMP_Text healthText;
    private HealthComponent playerHealthComponent = null;

    private void Awake() {
        var obj = FindFirstObjectByType<PlayerTank>();
        playerHealthComponent = (obj != null) ? obj.GetComponent<HealthComponent>() : null;
    }

    private int _healthCache = 0;
    private void Update() {
        if(playerHealthComponent == null) {
            return;
        }
        if(_healthCache != playerHealthComponent.health) {
            healthText.text = playerHealthComponent.health.ToString();
            _healthCache = playerHealthComponent.health;
        }
    }
}

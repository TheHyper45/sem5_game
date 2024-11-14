using UnityEngine;

public class HealthComponent : MonoBehaviour {
    [SerializeField]
    private int maxHealth;
    public int health;

    private void Awake() {
        health = maxHealth;
    }

    private void Update() {
        if(health <= 0) {
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class Eagle : MonoBehaviour {
    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private int maxHealth;

    private int health;

    private void Start() {
        health = maxHealth;
        healthBar.SetValue((float)health / maxHealth);
    }

    public void Hit(int damage) {
        if(health <= 0) return;
        health -= damage;
        healthBar.SetValue((float)health / maxHealth);
        if(health > 0) return;
        Destroy(gameObject);
    }
}
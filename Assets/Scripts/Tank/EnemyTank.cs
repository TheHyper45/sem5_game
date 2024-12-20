using UnityEngine;

public class EnemyTank : MonoBehaviour {
    [SerializeField]
    private Transform tower,gun;
    [SerializeField]
    private Transform bulletSpawnPoint;
    [SerializeField]
    private Bullet bulletPrefab;
    [SerializeField]
    private Animator rightTreadAnimation,leftTreadAnimation;
    [SerializeField]
    private GameObject ragdollPrefab;
    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private int maxHealth;
    public int health;

    private void Awake() {
        health = maxHealth;
    }

    private void Update() {
        healthBar.SetValue((float)health / maxHealth);
        if(health <= 0) {
            Instantiate(ragdollPrefab,transform.position,transform.rotation);
            Destroy(gameObject);
        }
    }
}
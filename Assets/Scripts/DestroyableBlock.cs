using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DestroyableBlock : MonoBehaviour {
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private SimpleRagdoll destroyedPrefab;
    [SerializeField]
    private Pickup dropPickupPrefab;

    public int health;

    private float timer = 0f;

    private void Start() {
        health = maxHealth;
        if(!healthBar) return;
        healthBar.SetValue((float)health / maxHealth);
        healthBar.gameObject.SetActive(false);
    }

    private void FixedUpdate() {
        if(!healthBar) return;
        if(!healthBar.gameObject.activeSelf) return;
        timer -= Time.fixedDeltaTime * Time.timeScale;
        if(timer > 0f) return;
        timer = 0f;
        healthBar.gameObject.SetActive(false);
    }

    public void Hit(int damage,Vector3 direction,float force) {
        if(health <= 0) return;
        health -= damage;
        if(healthBar) {
            healthBar.SetValue((float)health / maxHealth);
            healthBar.gameObject.SetActive(true);
            timer = 5f;
        }
        if(health > 0) return;
        Instantiate(destroyedPrefab,transform.position,Quaternion.identity).Init(5f,direction,force);
        if(dropPickupPrefab) Instantiate(dropPickupPrefab,transform.position + new Vector3(0f,0.5f,0f),Quaternion.identity);
        Destroy(gameObject);
    }
}
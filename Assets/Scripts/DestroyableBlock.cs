using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DestroyableBlock : MonoBehaviour {
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private SimpleRagdoll destroyedPrefab;
    [SerializeField]
    private Pickup dropPickupPrefab;

    public int health;

    private void Awake() {
        health = maxHealth;
    }

    public void Hit(int damage,Vector3 direction,float force) {
        if(health <= 0) return;
        health -= damage;
        if(health > 0) return;
        Instantiate(destroyedPrefab,transform.position,Quaternion.identity).Init(5f,direction,force);
        if(dropPickupPrefab) Instantiate(dropPickupPrefab,transform.position + new Vector3(0f,0.5f,0f),Quaternion.identity);
        Destroy(gameObject);
    }
}
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DestroyableBlock : MonoBehaviour {
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private SimpleRagdoll destroyedPrefab;

    public int health;

    private void Awake() {
        health = maxHealth;
    }

    public void Hit(Vector3 direction,float force) {
        health -= 1;
        if(health > 0) return;
        Instantiate(destroyedPrefab,transform.position,Quaternion.identity).Init(direction,force);
        Destroy(gameObject);
    }
}
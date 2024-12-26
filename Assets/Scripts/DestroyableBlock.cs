using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class DestroyableBlock : MonoBehaviour {
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private SimpleRagdoll destroyedPrefab;
    [SerializeField]
    private Pickup dropPickupPrefab;

    public int health;

    public static readonly List<DestroyableBlock> allBlocks = new();

    private void Awake() {
        allBlocks.Add(this);
        health = maxHealth;
    }

    private void OnDestroy() {
        allBlocks.Remove(this);
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
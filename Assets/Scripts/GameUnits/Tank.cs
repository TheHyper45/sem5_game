using System;
using UnityEngine;

public class Tank : MonoBehaviour {
    public float moveSpeed;
    public int maxHealth;

    [SerializeField]
    protected Animator rightTreadAnimation,leftTreadAnimation;
    [SerializeField]
    private GameObject[] ragdollDestroyObjects;
    [SerializeField]
    private GameObject[] ragdollAddRigidbodyObjects;
    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private Transform gunSpawnPoint;

    [NonSerialized,HideInInspector]
    public int health;

    public TankGun CurrentGun { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public Collider[] BulletIgnoreColliders { get; private set; }
    public Collider[] BulletGunIgnoreColliders { get; private set; }

    public readonly Quaternion gunFixRotation = Quaternion.Euler(90f,0f,0f);

    protected virtual void Awake() {
        Rigidbody = GetComponent<Rigidbody>();
        BulletIgnoreColliders = GetComponentsInChildren<Collider>(true);
        SwitchGun(GameState.instance.machineGunPrefab);
        BulletGunIgnoreColliders = CurrentGun.GetComponentsInChildren<Collider>(true);
    }

    protected virtual void Start() {
        health = maxHealth;
        healthBar.SetValue((float)health / maxHealth);
    }

    protected virtual void FixedUpdate() {

    }

    public void SwitchGun(TankGun gunPrefab) {
        if(CurrentGun) Destroy(CurrentGun);
        CurrentGun = Instantiate(gunPrefab,gunSpawnPoint.position,gunFixRotation,gunSpawnPoint);
        BulletGunIgnoreColliders = CurrentGun.GetComponentsInChildren<Collider>(true);
    }

    public void Hit(int damage) {
        if(health <= 0) return;
        health -= damage;
        healthBar.SetValue((float)health / maxHealth);
        if(health > 0) return;

        foreach(var obj in ragdollDestroyObjects) {
            Destroy(obj);
        }
        foreach(var obj in ragdollAddRigidbodyObjects) {
            obj.AddComponent<Rigidbody>().mass = 500;
        }
        if(CurrentGun) CurrentGun.gameObject.AddComponent<Rigidbody>().mass = 500;

        gameObject.AddComponent<SimpleRagdoll>().Init(1.5f);
        Destroy(Rigidbody);
        Destroy(this);
    }

    protected virtual void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.TryGetComponent(out Pickup pickup)) {
            pickup.Collect(this);
        }
    }
}
using System;
using UnityEngine;
using System.Runtime.CompilerServices;

public class Tank : MonoBehaviour {
    public float moveSpeed;
    public int maxHealth;
    public int damage;
    public float roundsPerSecond;

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
    [NonSerialized,HideInInspector]
    public TankGun currentGun;

    public Rigidbody Rigidbody { get; private set; }
    public Collider[] BulletIgnoreColliders { get; private set; }
    public float ShootCooldown { get; private set; } = float.MaxValue;

    public readonly Quaternion gunFixRotation = Quaternion.Euler(90f,0f,0f);

    protected virtual void Awake() {
        Rigidbody = GetComponent<Rigidbody>();
        BulletIgnoreColliders = GetComponentsInChildren<Collider>(true);
        SwitchGun(GameState.instance.singleBarrelTowerPrefab);
    }

    protected virtual void Start() {
        health = maxHealth;
        healthBar.SetValue((float)health / maxHealth);
    }

    protected virtual void FixedUpdate() {
        ShootCooldown = Mathf.Min(ShootCooldown + Time.fixedDeltaTime * Time.timeScale,1f / roundsPerSecond);
    }

    public void SwitchGun(TankGun gunPrefab) {
        if(currentGun) Destroy(currentGun);
        currentGun = Instantiate(gunPrefab,gunSpawnPoint.position,gunFixRotation,gunSpawnPoint);
    }

    public void Shoot() {
        if(ShootCooldown < 1f / roundsPerSecond) return;
        currentGun.SpawnBullets();
        ShootCooldown = 0f;
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
        if(currentGun) currentGun.gameObject.AddComponent<Rigidbody>().mass = 500;

        gameObject.AddComponent<SimpleRagdoll>().Init(5f);
        Destroy(Rigidbody);
        Destroy(this);
    }

    protected virtual void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.TryGetComponent(out Pickup pickup)) {
            pickup.Collect(this);
        }
    }
}
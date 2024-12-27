using System;
using UnityEngine;

public class Tank : MonoBehaviour {
    public float moveSpeed;
    public int maxHealth;
    public int damage;
    public float roundsPerSecond;
    public TankGun singleBarrelGun;
    public TankGun doubleBarrelGun;

    [SerializeField]
    private Animator rightTreadAnimation,leftTreadAnimation;
    [SerializeField]
    private GameObject[] ragdollDestroyObjects;
    [SerializeField]
    private GameObject[] ragdollAddRigidbodyObjects;
    [SerializeField]
    private HealthBar healthBar;

    [NonSerialized,HideInInspector]
    public int health;
    [NonSerialized,HideInInspector]
    public TankGun currentGun;

    public Rigidbody Rigidbody { get; private set; }
    public Collider[] BulletIgnoreColliders { get; private set; }
    public float ShootCooldown { get; private set; } = float.MaxValue;

    protected virtual void Awake() {
        Rigidbody = GetComponent<Rigidbody>();
        BulletIgnoreColliders = GetComponentsInChildren<Collider>(true);
        SwitchGun(singleBarrelGun);
    }

    protected virtual void Start() {
        health = maxHealth;
        healthBar.SetValue((float)health / maxHealth);
    }

    protected virtual void FixedUpdate() {
        ShootCooldown = Mathf.Min(ShootCooldown + Time.fixedDeltaTime,1f / roundsPerSecond);
        /*float rightThreadAnimationSpeed = moveDirection * moveSpeed * 1.15f;
        float leftThreadAnimationSpeed = moveDirection * moveSpeed * 1.15f;
        rightTreadAnimation.SetFloat("MoveSpeed",rightThreadAnimationSpeed);
        leftTreadAnimation.SetFloat("MoveSpeed",leftThreadAnimationSpeed);*/
    }

    public void SwitchGun(TankGun gun) {
        singleBarrelGun.gameObject.SetActive(ReferenceEquals(singleBarrelGun,gun));
        doubleBarrelGun.gameObject.SetActive(ReferenceEquals(doubleBarrelGun,gun));
        currentGun = gun;
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
using System;
using UnityEngine;

public class DrivableGameUnit : MonoBehaviour {
    public float moveSpeed,turnSpeed;
    public float towerRotateSpeed;
    public int maxHealth;
    public int damage;
    public float roundsPerSecond;
    public GameUnitGun singleBarrelGun;
    public GameUnitGun doubleBarrelGun;

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
    public GameUnitGun currentGun;
    [NonSerialized,HideInInspector]
    public float moveDirection = 0f;
    [NonSerialized,HideInInspector]
    public float turnDirection = 0f;

    public Rigidbody Rigidbody { get; private set; }
    public Collider[] BulletIgnoreColliders { get; private set; }
    public float ShootCooldown { get; private set; } = float.MaxValue;

    private void Awake() {
        Rigidbody = GetComponent<Rigidbody>();
        BulletIgnoreColliders = GetComponentsInChildren<Collider>(true);
        SwitchGun(singleBarrelGun);
    }

    private void Start() {
        health = maxHealth;
        healthBar.SetValue((float)health / maxHealth);
    }

    private void Update() {
        ShootCooldown = Mathf.Min(ShootCooldown + Time.deltaTime,1f / roundsPerSecond);
    }

    private void FixedUpdate() {
        float step = Time.fixedDeltaTime * Time.timeScale;
        Rigidbody.MovePosition(transform.position + moveDirection * moveSpeed * step * transform.forward);
        var angle = turnDirection * turnSpeed * Mathf.Rad2Deg * step;
        Rigidbody.MoveRotation(Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y + angle,transform.rotation.eulerAngles.z));

        float rightThreadAnimationSpeed = 0f;
        float leftThreadAnimationSpeed = 0f;
        if(moveDirection == 0f && turnDirection != 0f) {
            rightThreadAnimationSpeed = -turnDirection * turnSpeed * Mathf.PI;
            leftThreadAnimationSpeed = +turnDirection * turnSpeed * Mathf.PI;
        }
        else if(moveDirection != 0f && turnDirection == 0f) {
            rightThreadAnimationSpeed = moveDirection * moveSpeed * 1.15f;
            leftThreadAnimationSpeed = moveDirection * moveSpeed * 1.15f;
        }
        else if(moveDirection != 0f && turnDirection != 0f) {
            rightThreadAnimationSpeed = (turnDirection > 0f ? 0.625f : 1.15f) * moveDirection * moveSpeed;
            leftThreadAnimationSpeed = (turnDirection < 0f ? 0.625f : 1.15f) * moveDirection * moveSpeed;
        }
        rightTreadAnimation.SetFloat("MoveSpeed",rightThreadAnimationSpeed);
        leftTreadAnimation.SetFloat("MoveSpeed",leftThreadAnimationSpeed);
    }

    public void SwitchGun(GameUnitGun gun) {
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
            var rigidbody = obj.AddComponent<Rigidbody>();
            rigidbody.mass = 500;
        }

        gameObject.AddComponent<SimpleRagdoll>().Init(5f);
        Destroy(Rigidbody);
        Destroy(this);
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.TryGetComponent(out Pickup pickup)) {
            pickup.Collect(this);
        }
    }
}
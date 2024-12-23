using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DrivableTank : MonoBehaviour {
    [SerializeField]
    protected TankTower tower;
    [SerializeField]
    private SimpleRagdoll ragdollPrefab;
    [SerializeField]
    private float moveSpeed,turnSpeed;
    [SerializeField]
    protected float towerRotateSpeed;
    [SerializeField]
    private Animator rightTreadAnimation,leftTreadAnimation;
    [SerializeField]
    protected int maxHealth;
    [NonSerialized,HideInInspector]
    public Collider[] bulletIgnoreColliders;
    public int bulletDamage;

    private new Rigidbody rigidbody;
    protected float moveDirection = 0f;
    protected float turnDirection = 0f;
    protected int health;
    private int pickupLayer;

    protected virtual void Awake() {
        pickupLayer = LayerMask.NameToLayer("Pickup");
        health = maxHealth;
        rigidbody = GetComponent<Rigidbody>();
        bulletIgnoreColliders = GetComponentsInChildren<Collider>();
    }

    protected virtual void FixedUpdate() {
        rigidbody.MovePosition(transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime * Time.timeScale * transform.forward);
        var angle = turnDirection * turnSpeed * Mathf.Rad2Deg * Time.fixedDeltaTime * Time.timeScale;
        rigidbody.MoveRotation(Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y + angle,transform.rotation.eulerAngles.z));

        float rightThreadAnimationSpeed = 0f;
        float leftThreadAnimationSpeed = 0f;
        if(moveDirection == 0f && turnDirection != 0f) {
            rightThreadAnimationSpeed = -turnDirection * turnSpeed * Mathf.PI;
            leftThreadAnimationSpeed = +turnDirection * turnSpeed * Mathf.PI;
        }
        else if(moveDirection != 0f && turnDirection == 0f) {
            rightThreadAnimationSpeed = moveDirection * moveSpeed;
            leftThreadAnimationSpeed = moveDirection * moveSpeed;
        }
        else if(moveDirection != 0f && turnDirection != 0f) {
            rightThreadAnimationSpeed = (turnDirection > 0f ? 0.75f : 1.0f) * moveDirection * moveSpeed;
            leftThreadAnimationSpeed = (turnDirection < 0f ? 0.75f : 1.0f) * moveDirection * moveSpeed;
        }
        rightTreadAnimation.SetFloat("MoveSpeed",rightThreadAnimationSpeed);
        leftTreadAnimation.SetFloat("MoveSpeed",leftThreadAnimationSpeed);
    }

    public void Hit(int amount) {
        if(health <= 0) return;
        health -= amount;
        if(health > 0) return;
        Instantiate(ragdollPrefab,transform.position,transform.rotation);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.layer != pickupLayer) return;
        collision.gameObject.GetComponent<Pickup>().Collect(this);
    }
}
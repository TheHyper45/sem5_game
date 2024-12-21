using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DrivableTank : MonoBehaviour {
    [SerializeField]
    protected Transform tower;
    [SerializeField]
    private Transform bulletSpawnPoint;
    [SerializeField]
    private Bullet bulletPrefab;
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
    [SerializeField]
    private BoxCollider[] bulletIgnoreColliders;
    [SerializeField]
    private ParticleSystem shootEffect;

    private new Rigidbody rigidbody;
    protected float moveDirection = 0f;
    protected float turnDirection = 0f;

    protected int health;

    protected virtual void Awake() {
        health = maxHealth;
        rigidbody = GetComponent<Rigidbody>();
    }

    protected virtual void FixedUpdate() {
        rigidbody.MovePosition(transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime * Time.timeScale * transform.forward);
        var angle = turnDirection * turnSpeed * Mathf.Rad2Deg * Time.fixedDeltaTime * Time.timeScale;
        rigidbody.MoveRotation(Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y + angle,transform.rotation.eulerAngles.z));
    }

    protected void Shoot() {
        var bullet = Instantiate(bulletPrefab,bulletSpawnPoint.position,bulletSpawnPoint.rotation);
        bullet.Init(bulletIgnoreColliders);
        shootEffect.Play();
    }

    public void Hit(int amount) {
        health -= amount;
        if(health > 0) return;
        Instantiate(ragdollPrefab,transform.position,transform.rotation);
        Destroy(gameObject);
    }
}
/*
float rightThreadAnimationSpeed = 0f;
float leftThreadAnimationSpeed = 0f;
if(inputMoveDirection == 0f && inputTurnDirection != 0f) {
    rightThreadAnimationSpeed = -inputTurnDirection * turnSpeed * Mathf.PI;
    leftThreadAnimationSpeed = +inputTurnDirection * turnSpeed * Mathf.PI;
}
else if(inputMoveDirection != 0f && inputTurnDirection == 0f) {
    rightThreadAnimationSpeed = inputMoveDirection * moveSpeed;
    leftThreadAnimationSpeed = inputMoveDirection * moveSpeed;
}
else if(inputMoveDirection != 0f && inputTurnDirection != 0f) {
    rightThreadAnimationSpeed = (inputTurnDirection > 0f ? 0.75f : 1.0f) * inputMoveDirection * moveSpeed;
    leftThreadAnimationSpeed = (inputTurnDirection < 0f ? 0.75f : 1.0f) * inputMoveDirection * moveSpeed;
}
rightTreadAnimation.SetFloat("MoveSpeed",rightThreadAnimationSpeed);
leftTreadAnimation.SetFloat("MoveSpeed",leftThreadAnimationSpeed);
*/
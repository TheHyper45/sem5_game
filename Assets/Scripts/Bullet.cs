using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {
    private int damage;
    private float lifetimeInSeconds;
    private new Rigidbody rigidbody;
    private bool startTimer = false;
    private float timer = 0f;
    public Collider Collider { get; private set; }

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
    }

    private void FixedUpdate() {
        if(!startTimer) return;
        timer += Time.fixedDeltaTime;
        if(timer >= lifetimeInSeconds) Destroy(gameObject);
    }

    public void Init(int _damage,float _lifetimeInSeconds,Collider[] parentIgnoreColliders,Collider[] parentIgnoreColliders2) {
        if(startTimer) return;
        damage = _damage;
        lifetimeInSeconds = _lifetimeInSeconds;
        startTimer = true;
        rigidbody.AddForce(transform.forward * 30f,ForceMode.VelocityChange);
        if(parentIgnoreColliders != null) {
            foreach(var parentCollider in parentIgnoreColliders) {
                Physics.IgnoreCollision(Collider,parentCollider);
            }
        }
        if(parentIgnoreColliders2 != null) {
            foreach(var parentCollider in parentIgnoreColliders2) {
                Physics.IgnoreCollision(Collider,parentCollider);
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        var topParent = collision.gameObject.transform.root;
        if(topParent.TryGetComponent(out Tank drivableGameUnit)) {
            drivableGameUnit.Hit(damage);
        }
        else if(collision.gameObject.TryGetComponent(out EnemySpawnBase spawnBase)) {
            spawnBase.Hit(damage);
        }
        else if(collision.gameObject.TryGetComponent(out DestroyableBlock destroyableBlock)) {
            var dir = transform.forward;
            dir.y = 1f;
            destroyableBlock.Hit(damage,dir,10f);
        }
        Destroy(gameObject);
    }
}
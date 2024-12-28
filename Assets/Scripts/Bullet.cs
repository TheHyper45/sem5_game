using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {
    private int damage;
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
        if(timer >= 0.333f) Destroy(gameObject);
    }

    public void Init(int _damage,Collider[] parentIgnoreColliders) {
        if(startTimer) return;
        damage = _damage;
        startTimer = true;
        rigidbody.AddForce(transform.forward * 30f,ForceMode.VelocityChange);
        foreach(var parentCollider in parentIgnoreColliders) {
            Physics.IgnoreCollision(Collider,parentCollider);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        var topParent = collision.gameObject.transform.root;
        if(topParent.TryGetComponent(out Tank drivableGameUnit)) {
            drivableGameUnit.Hit(damage);
        }
        else if(collision.gameObject.TryGetComponent(out Eagle eagle)) {
            eagle.Hit(damage);
        }
        else if(collision.gameObject.TryGetComponent(out DestroyableBlock destroyableBlock)) {
            var dir = transform.forward;
            dir.y = 0f;
            destroyableBlock.Hit(damage,dir,10f);
        }
        Destroy(gameObject);
    }
}
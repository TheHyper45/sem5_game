using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {
    private new Rigidbody rigidbody;
    public new Collider collider;
    private bool startTimer = false;
    private float timer = 0f;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    private void Update() {
        if(!startTimer) return;
        timer += Time.deltaTime;
        if(timer >= 5f) Destroy(gameObject);
    }

    public void Init(Collider[] parentIgnoreColliders) {
        if(startTimer) return;
        startTimer = true;
        rigidbody.AddForce(transform.forward * 60f,ForceMode.VelocityChange);
        foreach(var parentCollider in parentIgnoreColliders) {
            Physics.IgnoreCollision(collider,parentCollider);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        var topParent = collision.gameObject.transform.root;
        if(topParent.TryGetComponent(out DrivableTank drivableTank)) {
            drivableTank.Hit(1);
        }
        else if(collision.gameObject.TryGetComponent(out DestroyableBlock destroyableBlock)) {
            var dir = collision.gameObject.transform.position - transform.position;
            dir.y = 0f;
            destroyableBlock.Hit(dir,10f);
        }
        Destroy(gameObject);
    }
}
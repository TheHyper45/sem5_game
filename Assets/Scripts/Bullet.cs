using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {
    private DrivableTank parentDrivableTank;
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

    public void Init(DrivableTank tank,Collider[] parentIgnoreColliders) {
        if(startTimer) return;
        parentDrivableTank = tank;
        startTimer = true;
        rigidbody.AddForce(transform.forward * 60f,ForceMode.VelocityChange);
        foreach(var parentCollider in parentIgnoreColliders) {
            Physics.IgnoreCollision(collider,parentCollider);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        var topParent = collision.gameObject.transform.root;
        if(topParent.TryGetComponent(out DrivableTank drivableTank)) {
            drivableTank.Hit(parentDrivableTank.bulletDamage);
        }
        else if(collision.gameObject.TryGetComponent(out DestroyableBlock destroyableBlock)) {
            var dir = transform.forward;
            dir.y = 0f;
            destroyableBlock.Hit(parentDrivableTank.bulletDamage,dir,10f);
        }
        Destroy(gameObject);
    }
}
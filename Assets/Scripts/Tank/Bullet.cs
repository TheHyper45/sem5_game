using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {
    [SerializeField]
    private new ParticleSystem particleSystem;

    private new Rigidbody rigidbody;
    private bool startTimer = false;
    private float timer = 0f;
    private int playerLayer;

    private void Awake() {
        playerLayer = LayerMask.NameToLayer("PlayerBullet");
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update() {
        if(!startTimer) return;
        timer += Time.deltaTime;
        if(timer >= 5f) {
            Destroy(gameObject);
        }
    }

    public void Init() {
        if(startTimer) return;
        startTimer = true;
        rigidbody.AddForce(transform.forward * 80f,ForceMode.VelocityChange);
        particleSystem.Play();
    }

    private void OnCollisionEnter(Collision collision) {
        var topParent = collision.gameObject.transform.root;
        if(collision.gameObject.TryGetComponent(out DestroyableBlock destroyableBlock)) {
            destroyableBlock.Init(transform.position,20f);
        }
        else if(topParent.TryGetComponent(out destroyableBlock)) {
            destroyableBlock.Init(transform.position,20f);
        }
        else if(gameObject.layer == playerLayer && topParent.TryGetComponent(out EnemyTank enemyTank)) {
            enemyTank.health -= 1;
        }
        Destroy(gameObject);
    }
}
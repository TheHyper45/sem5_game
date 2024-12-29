using UnityEngine;

public class SimpleRagdoll : MonoBehaviour {
    private float despawnTime = -1f;
    [SerializeField]
    private Rigidbody[] movingFragments;

    private float timer = 0f;

    private void FixedUpdate() {
        if(despawnTime < 0f) return;
        timer += Time.fixedDeltaTime * Time.timeScale;
        if(timer >= despawnTime) {
            Destroy(gameObject);
        }
    }

    public void Init(float _despawnTime) {
        despawnTime = _despawnTime;
    }

    public void Init(float _despawnTime,Vector3 direction,float force) {
        despawnTime = _despawnTime;
        foreach(var fragment in movingFragments) {
            fragment.AddForce(direction * force,ForceMode.VelocityChange);
        }
    }
}
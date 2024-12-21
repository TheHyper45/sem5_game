using UnityEngine;

public class SimpleRagdoll : MonoBehaviour {
    [SerializeField]
    private float despawnTime;
    [SerializeField]
    private Rigidbody[] movingFragments;

    private float timer = 0f;

    private void Update() {
        if(despawnTime < 0f) return;
        timer += Time.deltaTime;
        if(timer >= despawnTime) {
            Destroy(gameObject);
        }
    }

    public void Init(Vector3 direction,float force) {
        foreach(var fragment in movingFragments) {
            fragment.AddForce(direction * force,ForceMode.VelocityChange);
        }
    }
}
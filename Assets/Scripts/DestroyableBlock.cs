using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DestroyableBlock : MonoBehaviour {
    [SerializeField]
    private GameObject[] fragments;

    private BoxCollider boxCollider;
    private bool startTimer = false;
    private float timer = 0f;

    private void Awake() {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update() {
        if(!startTimer) return;
        timer += Time.deltaTime;
        if(timer >= 5f) {
            Destroy(gameObject);
        }
    }

    public void Init(Vector3 startPoint,float pushForce) {
        if(startTimer) return;
        boxCollider.enabled = false;
        startTimer = true;
        foreach(var fragment in fragments) {
            fragment.GetComponent<BoxCollider>().enabled = true;
            var rigidbody = fragment.AddComponent<Rigidbody>();
            var dir = rigidbody.transform.position - startPoint;
            rigidbody.AddForce(dir * pushForce,ForceMode.VelocityChange);
        }
    }
}
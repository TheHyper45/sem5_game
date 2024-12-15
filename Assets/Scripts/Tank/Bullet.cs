using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {
    public Rigidbody Rigidbody { get; private set; }

    private float timer;

    private void Awake() {
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update() {
        timer += Time.deltaTime;
        while(timer >= 5f) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        Destroy(gameObject);
    }
}
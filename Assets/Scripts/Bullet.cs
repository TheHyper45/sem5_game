using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {
    private float lifetimeTimer = 0.0f;
    [HideInInspector,NonSerialized]
    public GameObject owner;

    private void Update() {
        lifetimeTimer += Time.deltaTime;
        if(lifetimeTimer >= 5.0f) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(ReferenceEquals(collision.gameObject,owner)) {
            return;
        }
        if(collision.gameObject.TryGetComponent(out HealthComponent component)) {
            component.health -= 1;
        }
        Destroy(gameObject);
    }
}

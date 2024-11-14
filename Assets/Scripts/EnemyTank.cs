using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyTank : MonoBehaviour {
    private HealthComponent healthComponent = null;

    private void Awake() {
        TryGetComponent(out healthComponent);
    }
}

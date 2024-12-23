using UnityEngine;

public class Pickup : MonoBehaviour {
    private void Update() {
        transform.Rotate(0f,Time.deltaTime * 0.5f * Mathf.Rad2Deg,0f);
    }

    public virtual void Collect(DrivableTank tank) {
        Destroy(gameObject);
    }
}
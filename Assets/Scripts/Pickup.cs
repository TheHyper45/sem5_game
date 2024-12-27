using UnityEngine;

public class Pickup : MonoBehaviour {
    [SerializeField]
    private float rotateSpeed;

    private void FixedUpdate() {
        transform.Rotate(0f,Time.fixedDeltaTime * rotateSpeed * Mathf.Rad2Deg,0f);
    }

    public virtual void Collect(Tank tank) {
        Destroy(gameObject);
    }
}
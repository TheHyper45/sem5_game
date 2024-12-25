using UnityEngine;

public class Pickup : MonoBehaviour {
    [SerializeField]
    private float rotateSpeed;

    private void Update() {
        transform.Rotate(0f,Time.deltaTime * rotateSpeed * Mathf.Rad2Deg,0f);
    }

    public virtual void Collect(DrivableGameUnit tank) {
        Destroy(gameObject);
    }
}
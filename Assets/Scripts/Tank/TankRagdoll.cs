using UnityEngine;

public class TankRagdoll : MonoBehaviour {
    [SerializeField]
    private float despawnTime;

    private float timer = 0f;

    private void Update() {
        timer += Time.deltaTime;
        if(timer >= despawnTime) {
            Destroy(gameObject);
        }
    }
}
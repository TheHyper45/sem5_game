using UnityEngine;

public class BillboardingSprite : MonoBehaviour {
    private void Update() {
        transform.rotation = Camera.main.transform.rotation;
    }
}
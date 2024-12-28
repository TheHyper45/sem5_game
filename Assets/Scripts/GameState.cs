using UnityEngine;

public class GameState : MonoBehaviour {
    public static GameState instance;

    public TankGun singleBarrelTowerPrefab;
    public TankGun doubleBarrelTowerPrefab;

    private void Awake() {
        if(instance) {
            Debug.LogError("Only one \"GameState\" instance can exist at any given moment.");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
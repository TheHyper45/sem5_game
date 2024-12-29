using UnityEngine;

public class GameState : MonoBehaviour {
    public static GameState instance;

    public TankGun singleBarrelTowerPrefab;
    public TankGun doubleBarrelTowerPrefab;

    private void Awake() {
        if(instance) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
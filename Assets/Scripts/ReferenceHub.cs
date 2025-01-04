using UnityEngine;

public class ReferenceHub : MonoBehaviour {
    public TankGun machineGunPrefab;
    public Bullet bulletPrefab;
    public MoneyPickup moneyPickupPrefab;
    public LevelCompletedMenu levelCompletedMenuPrefab;
    public LevelFailedMenu levelFailedMenuPrefab;

    public static ReferenceHub instance;

    private void Awake() {
        if(instance) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }
}
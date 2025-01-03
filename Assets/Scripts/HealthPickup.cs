using UnityEngine;

public class HealthPickup : Pickup {
    [SerializeField]
    private int healthIncrement;

    public override void Collect(Tank tank) {
        tank.health = Mathf.Clamp(tank.health + healthIncrement,0,tank.baseMaxHealth);
        base.Collect(tank);
    }
}
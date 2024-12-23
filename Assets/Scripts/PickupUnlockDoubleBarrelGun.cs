public class PickupUnlockDoubleBarrelGun : Pickup {
    public override void Collect(DrivableGameUnit tank) {
        tank.SwitchGun(tank.doubleBarrelGun);
        base.Collect(tank);
    }
}
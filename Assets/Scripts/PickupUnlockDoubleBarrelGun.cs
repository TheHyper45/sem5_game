public class PickupUnlockDoubleBarrelGun : Pickup {
    public override void Collect(Tank tank) {
        tank.SwitchGun(tank.doubleBarrelGun);
        base.Collect(tank);
    }
}
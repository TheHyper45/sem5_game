using UnityEngine;

public class PickupUpgradeTank : Pickup {
    [SerializeField]
    private PlayerTankController playerUpgradedTankPrefab;
    [SerializeField]
    private AITankController enemyUpgradedTankPrefab;

    public override void Collect(DrivableTank tank) {
        base.Collect(tank);
        if(tank is PlayerTankController controller) {
            var newTank = Instantiate(playerUpgradedTankPrefab,tank.transform.position,tank.transform.rotation);
            newTank.bulletDamage = tank.bulletDamage;
            newTank.cameraRelativeLookAtPoint = controller.cameraRelativeLookAtPoint;
            newTank.cameraRelativePosition = controller.cameraRelativePosition;
        }
        else if(tank is AITankController) {
            var newTank = Instantiate(enemyUpgradedTankPrefab,tank.transform.position,tank.transform.rotation);
            newTank.bulletDamage = tank.bulletDamage;
        }
        Destroy(tank.gameObject);
    }
}
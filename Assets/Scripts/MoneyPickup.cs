using UnityEngine;

public class MoneyPickup : Pickup {
    public override void Collect(Tank tank) {
        if(tank is not PlayerTank playerTank) return;
        playerTank.collectedMoney += Random.Range(1,9) * 5;
        base.Collect(tank);
    }
}
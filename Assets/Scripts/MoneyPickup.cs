using UnityEngine;

public class MoneyPickup : Pickup {
    public override void Collect(Tank tank) {
        if(tank is not PlayerTank) return;
        GameState.instance.playerCollectedMoney += Random.Range(1,5) * 5;
        base.Collect(tank);
    }
}
using UnityEngine;

public class MoneyPickup : Pickup {
    [SerializeField]
    private int randomMin;
    [SerializeField]
    private int randomMax;
    [SerializeField]
    private int randomMultiplier;

    public override void Collect(Tank tank) {
        if(tank is not PlayerTank) return;
        GameState.instance.playerCollectedMoney += Random.Range(randomMin,randomMax + 1) * randomMultiplier;
        base.Collect(tank);
    }
}
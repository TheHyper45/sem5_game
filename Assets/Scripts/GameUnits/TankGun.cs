using UnityEngine;

public class TankGun : MonoBehaviour {
    [SerializeField]
    protected float baseRoundsPerSecond;
    [SerializeField]
    protected int baseDamage;

    public float ShootCooldown { get; protected set; } = float.MaxValue;
    protected Tank parentDrivableUnit;

    protected virtual void Awake() {
        parentDrivableUnit = GetComponentInParent<Tank>();
    }

    protected virtual void FixedUpdate() {
        ShootCooldown = Mathf.Min(ShootCooldown + Time.fixedDeltaTime * Time.timeScale,1f / baseRoundsPerSecond);
    }

    public void Shoot() {
        if(ShootCooldown < 1f / baseRoundsPerSecond) return;
        ShootCooldown = 0f;
        SpawnBullets();
    }

    protected virtual void SpawnBullets() {

    }
}
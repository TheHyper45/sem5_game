using UnityEngine;

public class TankGun : MonoBehaviour {
    [SerializeField]
    protected float roundsPerSecond;
    [SerializeField]
    protected int damage;

    public float ShootCooldown { get; protected set; } = float.MaxValue;
    protected Tank parentDrivableUnit;

    protected virtual void Awake() {
        parentDrivableUnit = GetComponentInParent<Tank>();
    }

    protected virtual void FixedUpdate() {
        ShootCooldown = Mathf.Min(ShootCooldown + Time.fixedDeltaTime * Time.timeScale,1f / roundsPerSecond);
    }

    public void Shoot() {
        if(ShootCooldown < 1f / roundsPerSecond) return;
        ShootCooldown = 0f;
        SpawnBullets();
    }

    protected virtual void SpawnBullets() {

    }
}
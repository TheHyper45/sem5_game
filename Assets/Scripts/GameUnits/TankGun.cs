using UnityEngine;

public class TankGun : MonoBehaviour {
    [SerializeField]
    protected Bullet bulletPrefab;
    protected Tank parentDrivableUnit;

    protected virtual void Awake() {
        parentDrivableUnit = GetComponentInParent<Tank>();
    }

    public virtual void SpawnBullets() {

    }
}
using UnityEngine;

public class TankGun : MonoBehaviour {
    [SerializeField]
    protected Bullet bulletPrefab;
    protected Tank parentDrivableUnit;

    private void Awake() {
        parentDrivableUnit = GetComponentInParent<Tank>();
    }

    public virtual void SpawnBullets() {

    }
}
using UnityEngine;

public class GameUnitGun : MonoBehaviour {
    [SerializeField]
    protected Bullet bulletPrefab;
    protected DrivableGameUnit parentDrivableUnit;

    private void Awake() {
        parentDrivableUnit = GetComponentInParent<DrivableGameUnit>();
    }

    public virtual void SpawnBullets() {

    }
}
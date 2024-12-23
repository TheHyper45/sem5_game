using UnityEngine;

public class TankTower : MonoBehaviour {
    [SerializeField]
    protected Bullet bulletPrefab;

    protected DrivableTank parentDrivableTank;

    private void Awake() {
        parentDrivableTank = GetComponentInParent<DrivableTank>();
    }

    public virtual void Shoot() {

    }
}
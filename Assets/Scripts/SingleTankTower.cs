using UnityEngine;

public class SingleTankTower : TankTower {
    [SerializeField]
    private Transform bulletSpawnPoint;
    [SerializeField]
    private ParticleSystem bulletSpawnParticleEffect;

    public override void Shoot() {
        base.Shoot();
        var bullet = Instantiate(bulletPrefab,bulletSpawnPoint.position,bulletSpawnPoint.rotation);
        bullet.Init(parentDrivableTank,parentDrivableTank.bulletIgnoreColliders);
        bulletSpawnParticleEffect.Play();
    }
}
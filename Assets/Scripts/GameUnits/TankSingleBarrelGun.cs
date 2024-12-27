using UnityEngine;

public class TankSingleBarrelGun : TankGun {
    [SerializeField]
    private Transform bulletSpawnPoint;
    [SerializeField]
    private ParticleSystem bulletSpawnParticleEffect;

    public override void SpawnBullets() {
        base.SpawnBullets();
        var bullet = Instantiate(bulletPrefab,bulletSpawnPoint.position,bulletSpawnPoint.rotation);
        bullet.Init(parentDrivableUnit.damage,parentDrivableUnit.BulletIgnoreColliders);
        bulletSpawnParticleEffect.Play();
    }
}
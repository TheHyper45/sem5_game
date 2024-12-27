using UnityEngine;

public class TankDoubleBarrelGun : TankGun {
    [SerializeField]
    private Transform rightBulletSpawnPoint;
    [SerializeField]
    private ParticleSystem rightBulletSpawnParticleEffect;
    [SerializeField]
    private Transform leftBulletSpawnPoint;
    [SerializeField]
    private ParticleSystem leftBulletSpawnParticleEffect;

    public override void SpawnBullets() {
        base.SpawnBullets();
        var rightBullet = Instantiate(bulletPrefab,rightBulletSpawnPoint.position,rightBulletSpawnPoint.rotation);
        rightBullet.Init(parentDrivableUnit.damage,parentDrivableUnit.BulletIgnoreColliders);
        rightBulletSpawnParticleEffect.Play();
        var leftBullet = Instantiate(bulletPrefab,leftBulletSpawnPoint.position,leftBulletSpawnPoint.rotation);
        leftBullet.Init(parentDrivableUnit.damage,parentDrivableUnit.BulletIgnoreColliders);
        leftBulletSpawnParticleEffect.Play();
    }
}
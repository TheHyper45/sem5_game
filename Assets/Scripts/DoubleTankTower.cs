using UnityEngine;

public class DoubleTankTower : TankTower {
    [SerializeField]
    private Transform rightBulletSpawnPoint;
    [SerializeField]
    private ParticleSystem rightBulletSpawnParticleEffect;
    [SerializeField]
    private Transform leftBulletSpawnPoint;
    [SerializeField]
    private ParticleSystem leftBulletSpawnParticleEffect;

    public override void Shoot() {
        base.Shoot();
        var rightBullet = Instantiate(bulletPrefab,rightBulletSpawnPoint.position,rightBulletSpawnPoint.rotation);
        rightBullet.Init(parentDrivableTank,parentDrivableTank.bulletIgnoreColliders);
        rightBulletSpawnParticleEffect.Play();
        var leftBullet = Instantiate(bulletPrefab,leftBulletSpawnPoint.position,leftBulletSpawnPoint.rotation);
        leftBullet.Init(parentDrivableTank,parentDrivableTank.bulletIgnoreColliders);
        leftBulletSpawnParticleEffect.Play();
    }
}
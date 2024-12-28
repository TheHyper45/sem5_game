using UnityEngine;

public class TankSingleBarrelGun : TankGun {
    [SerializeField]
    private Transform bulletSpawnPoint;
    [SerializeField]
    private ParticleSystem bulletSpawnParticleEffect;
    [SerializeField]
    private Transform recoilMovePoint;
    [SerializeField]
    private Transform cannon;

    private Vector3 cannonInitialPos;

    protected override void Awake() {
        base.Awake();
        cannonInitialPos = cannon.localPosition;
    }

    private void FixedUpdate() {
        cannon.localPosition = Vector3.Lerp(cannon.localPosition,cannonInitialPos,Time.fixedDeltaTime * 3f);
    }

    public override void SpawnBullets() {
        base.SpawnBullets();
        var bullet = Instantiate(bulletPrefab,bulletSpawnPoint.position,bulletSpawnPoint.rotation);
        bullet.Init(parentDrivableUnit.damage,parentDrivableUnit.BulletIgnoreColliders);
        bulletSpawnParticleEffect.Play();
        cannon.localPosition = recoilMovePoint.localPosition;
    }
}
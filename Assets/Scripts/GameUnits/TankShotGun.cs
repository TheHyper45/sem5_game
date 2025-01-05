using System;
using UnityEngine;

public class TankShotGun : TankGun {
    public Transform[] bulletSpawnPoints;
    public Transform recoilMovePoint;
    public Transform cannon;
    [NonSerialized]
    public Vector3 initialPos;

    protected override void Awake() {
        base.Awake();
        initialPos = cannon.localPosition;
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        cannon.localPosition = Vector3.Lerp(cannon.localPosition,initialPos,Time.fixedDeltaTime * baseRoundsPerSecond / 2f);
    }

    protected override void SpawnBullets() {
        base.SpawnBullets();
        foreach(var spawnPoint in bulletSpawnPoints) {
            var bullet = Instantiate(ReferenceHub.instance.bulletPrefab,spawnPoint.position,spawnPoint.rotation);
            bullet.Init(baseDamage,baseBulletLifetime,parentDrivableUnit.BulletIgnoreColliders,parentDrivableUnit.BulletGunIgnoreColliders);
        }
        cannon.localPosition = recoilMovePoint.localPosition;
    }
}
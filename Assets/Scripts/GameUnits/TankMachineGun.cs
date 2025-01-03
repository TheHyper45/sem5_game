using System;
using UnityEngine;

public class TankMachineGun : TankGun {
    [Serializable]
    private struct Cannon {
        public Transform bulletSpawnPoint;
        public Transform recoilMovePoint;
        public Transform cannon;
        [NonSerialized]
        public Vector3 initialPos;
    }
    [SerializeField]
    private Cannon[] cannons;

    private int currentCannonIndex = 0;

    protected override void Awake() {
        base.Awake();
        for(int i = 0;i < cannons.Length;i += 1) {
            var cannon = cannons[i];
            cannon.initialPos = cannon.cannon.localPosition;
        }
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        for(int i = 0;i < cannons.Length;i += 1) {
            var cannon = cannons[i];
            cannon.cannon.localPosition = Vector3.Lerp(cannon.cannon.localPosition,cannon.initialPos,Time.fixedDeltaTime * baseRoundsPerSecond / 2f);
        }
    }

    protected override void SpawnBullets() {
        base.SpawnBullets();
        var cannon = cannons[currentCannonIndex];
        currentCannonIndex = (currentCannonIndex + 1) % cannons.Length;
        var bullet = Instantiate(GameState.instance.bulletPrefab,cannon.bulletSpawnPoint.position,cannon.bulletSpawnPoint.rotation);
        bullet.Init(baseDamage,parentDrivableUnit.BulletIgnoreColliders,parentDrivableUnit.BulletGunIgnoreColliders);
        cannon.cannon.localPosition = cannon.recoilMovePoint.localPosition;
    }
}
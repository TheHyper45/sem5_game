using UnityEngine;

public class PlayerTank : Tank {
    [SerializeField]
    private PlayerUI playerUI;

    public static PlayerTank instance;

    private readonly Quaternion cameraFixRotation = Quaternion.Euler(90f,0f,0f);
    private readonly RaycastHit[] mouseRaycastHits = new RaycastHit[1];

    private bool setInstanceToNullOnDestruction = true;

    public override void SwitchGun(TankGun gunPrefab) {
        base.SwitchGun(gunPrefab);
        if(string.IsNullOrEmpty(CurrentGun.upgradeName)) return;
        var level = SaveFile.GetUpgradeLevel(CurrentGun.upgradeName);
        if(level <= 0) return;
        var machinGunUpgrade = ShopUpgrades.GetUpgradeDescription(CurrentGun.upgradeName).levels[level - 1];
        var machinGunUpgradeInfo = (ShopMachineGunUpgradeValue)machinGunUpgrade.value;
        CurrentGun.baseDamage += machinGunUpgradeInfo.damage;
        CurrentGun.baseRoundsPerSecond += machinGunUpgradeInfo.roundsPerSecond; 
    }

    protected override void Awake() {
        base.Awake();
        if(instance != null) {
            setInstanceToNullOnDestruction = false;
            Destroy(gameObject);
            Debug.LogError("At most one instance of \"PlayerTank\" can exist at any given moment.");
        }
        else instance = this;
        var speedUpgradeLevel = SaveFile.GetUpgradeLevel("Speed");
        if(speedUpgradeLevel > 0) {
            var speedUpgrade = ShopUpgrades.GetUpgradeDescription("Speed").levels[speedUpgradeLevel - 1];
            baseMoveSpeed += (float)speedUpgrade.value;
        }
        var healthUpgradeLevel = SaveFile.GetUpgradeLevel("Health");
        if(healthUpgradeLevel > 0) {
            var healthUpgrade = ShopUpgrades.GetUpgradeDescription("Health").levels[healthUpgradeLevel - 1];
            baseMaxHealth += (int)healthUpgrade.value;
        }
        SwitchGun(ReferenceHub.instance.machineGunPrefab);
    }

    private void OnDestroy() {
        if(!setInstanceToNullOnDestruction) return;
        instance = null;
    }

    private void Update() {
        playerUI.SetHealthPercent((float)health / baseMaxHealth);
        Vector3 cameraPos = new(CurrentGun.transform.position.x,CurrentGun.transform.position.y + 15f,CurrentGun.transform.position.z);
        Camera.main.transform.SetPositionAndRotation(cameraPos,cameraFixRotation);
        if(Mathf.Abs(Time.timeScale) >= 0.001f) {
            var targetRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.RaycastNonAlloc(targetRay,mouseRaycastHits) > 0) {
                var vector = mouseRaycastHits[0].point - transform.position;
                vector.y = 0f;
                CurrentGun.transform.rotation = Quaternion.LookRotation(vector) * gunFixRotation;
            }
        }
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            SwitchGun(ReferenceHub.instance.machineGunPrefab);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2)) {
            if(SaveFile.GetUpgradeLevel(ReferenceHub.instance.shotGunPrefab.upgradeName) > 0) {
                SwitchGun(ReferenceHub.instance.shotGunPrefab);
            }
        }
        if(Input.GetKey(KeyCode.Mouse0)) CurrentGun.Shoot();
        float moveStep = Time.fixedDeltaTime * Time.timeScale;
        float moveX = 0f;
        float moveZ = 0f;
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            moveX = 1f;
        }
        else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            moveX = -1f;
        }
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            moveZ = 1f;
        }
        else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            moveZ = -1f;
        }
        bool isMoving = Mathf.Abs(moveX) + Mathf.Abs(moveZ) > 0.0001f;
        if(isMoving) {
            var moveVector = (moveX * Camera.main.transform.up + moveZ * Camera.main.transform.right).normalized;
            Rigidbody.MovePosition(transform.position + baseMoveSpeed * moveStep * moveVector);
            Rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(moveVector),moveStep * 4f));
        }
        float treadAnimationSpeed = isMoving ? baseMoveSpeed * 1.2f : 0f;
        rightTreadAnimation.SetFloat("MoveSpeed",treadAnimationSpeed);
        leftTreadAnimation.SetFloat("MoveSpeed",treadAnimationSpeed);
    }
}
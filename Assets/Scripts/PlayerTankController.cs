using UnityEngine;

public class PlayerTankController : DrivableTank {
    [SerializeField]
    private PlayerUI playerUI;
    public Vector3 cameraRelativePosition;
    public Vector3 cameraRelativeLookAtPoint;

    private readonly Quaternion towerAdjustRotation = Quaternion.Euler(90f,0f,0f);

    protected override void Awake() {
        base.Awake();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        playerUI.SetHealthPercent((float)health / maxHealth);
        if(Cursor.lockState == CursorLockMode.Locked) {
            var mouseMovementX = Input.GetAxis("Mouse X") * 100.0f * Time.deltaTime;
            var cameraRotateY = Quaternion.AngleAxis(mouseMovementX,Vector3.up);
            cameraRelativePosition = cameraRotateY * cameraRelativePosition;
            cameraRelativeLookAtPoint = cameraRotateY * cameraRelativeLookAtPoint;
            Camera.main.transform.position = tower.transform.position + cameraRelativePosition;
            Camera.main.transform.LookAt(tower.transform.position + cameraRelativeLookAtPoint);
            var towerTargetRotation = Quaternion.LookRotation(new(cameraRelativeLookAtPoint.x,0f,cameraRelativeLookAtPoint.z),transform.up) * towerAdjustRotation;
            tower.transform.rotation = Quaternion.Slerp(tower.transform.rotation,towerTargetRotation,3f * Time.deltaTime);
            if(Input.GetKeyDown(KeyCode.Mouse0)) tower.Shoot();
            if(Input.GetKeyDown(KeyCode.Space)) Hit(bulletDamage);
        }
    }

    protected override void FixedUpdate() {
        moveDirection = 0f;
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            moveDirection = 1f;
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            moveDirection = -1f;
        }
        turnDirection = 0f;
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            turnDirection = 1f;
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            turnDirection = -1f;
        }
        base.FixedUpdate();
    }
}
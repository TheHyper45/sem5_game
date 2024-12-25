using UnityEngine;

public class PlayerDrivableGameUnitController : MonoBehaviour {
    [SerializeField]
    private DrivableGameUnit drivableGameUnit;
    [SerializeField]
    private PlayerUI playerUI;

    public Vector3 cameraRelativePosition;
    public Vector3 cameraRelativeLookAtPoint;

    private readonly Quaternion towerAdjustRotation = Quaternion.Euler(90f,0f,0f);

    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        if(Cursor.lockState != CursorLockMode.Locked) {
            return;
        }
        if(Input.GetKey(KeyCode.Mouse0)) {
            drivableGameUnit.Shoot();
        }

        playerUI.SetHealthPercent((float)drivableGameUnit.health / drivableGameUnit.maxHealth);
        var mouseMovementX = Input.GetAxis("Mouse X") * 100.0f * Time.deltaTime;
        var cameraRotateY = Quaternion.AngleAxis(mouseMovementX,Vector3.up);
        cameraRelativePosition = cameraRotateY * cameraRelativePosition;
        cameraRelativeLookAtPoint = cameraRotateY * cameraRelativeLookAtPoint;
        Camera.main.transform.position = drivableGameUnit.currentGun.transform.position + cameraRelativePosition;
        Camera.main.transform.LookAt(drivableGameUnit.currentGun.transform.position + cameraRelativeLookAtPoint);
        var towerTargetRotation = Quaternion.LookRotation(new(cameraRelativeLookAtPoint.x,0f,cameraRelativeLookAtPoint.z),transform.up) * towerAdjustRotation;
        drivableGameUnit.currentGun.transform.rotation = Quaternion.Slerp(drivableGameUnit.currentGun.transform.rotation,towerTargetRotation,drivableGameUnit.towerRotateSpeed * Time.deltaTime);
    }

    private void FixedUpdate() {
        drivableGameUnit.moveDirection = 0f;
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            drivableGameUnit.moveDirection = 1f;
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            drivableGameUnit.moveDirection = -1f;
        }
        drivableGameUnit.turnDirection = 0f;
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            drivableGameUnit.turnDirection = 1f;
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            drivableGameUnit.turnDirection = -1f;
        }
    }
}
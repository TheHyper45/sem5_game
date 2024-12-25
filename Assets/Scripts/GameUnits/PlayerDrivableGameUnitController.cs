using UnityEngine;

public class PlayerDrivableGameUnitController : MonoBehaviour {
    [SerializeField]
    private PlayerUI playerUI;

    public Vector3 cameraRelativePosition;
    public Vector3 cameraRelativeLookAtPoint;

    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        if(Cursor.lockState != CursorLockMode.Locked) {
            return;
        }
        if(Input.GetKey(KeyCode.Mouse0)) {
            DrivableGameUnit.player.Shoot();
        }
        playerUI.SetHealthPercent((float)DrivableGameUnit.player.health / DrivableGameUnit.player.maxHealth);

        var mouseMovementX = Input.GetAxis("Mouse X") * 100.0f * Time.deltaTime;
        var cameraRotateY = Quaternion.AngleAxis(mouseMovementX,Vector3.up);
        cameraRelativePosition = cameraRotateY * cameraRelativePosition;
        cameraRelativeLookAtPoint = cameraRotateY * cameraRelativeLookAtPoint;
        Camera.main.transform.position = DrivableGameUnit.player.currentGun.transform.position + cameraRelativePosition;
        Camera.main.transform.LookAt(DrivableGameUnit.player.currentGun.transform.position + cameraRelativeLookAtPoint);

        DrivableGameUnit.player.RotateGunTowardsUpdate(cameraRelativeLookAtPoint.x,cameraRelativeLookAtPoint.z,Time.deltaTime);
    }

    private void FixedUpdate() {
        DrivableGameUnit.player.moveDirection = 0f;
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            DrivableGameUnit.player.moveDirection = 1f;
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            DrivableGameUnit.player.moveDirection = -1f;
        }
        DrivableGameUnit.player.turnDirection = 0f;
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            DrivableGameUnit.player.turnDirection = 1f;
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            DrivableGameUnit.player.turnDirection = -1f;
        }
    }
}
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerTankController : MonoBehaviour {
    [SerializeField]
    private Transform tower,gun;
    [SerializeField]
    private Transform bulletSpawnPoint;
    [SerializeField]
    private Bullet bulletPrefab;
    [SerializeField]
    private float moveSpeed,turnSpeed;
    [SerializeField]
    private float towerRotateSpeed,gunAdjustSpeed;
    [SerializeField]
    private Animator rightTreadAnimation,leftTreadAnimation;
    [SerializeField]
    private float gunRoundsPerSecond;
    [SerializeField]
    private Vector3 cameraRelativePosition;
    [SerializeField]
    private Vector3 cameraRelativeLookAtPoint;

    private new Rigidbody rigidbody;

    private readonly Quaternion towerAdjustRotation = Quaternion.Euler(90f,0f,0f);

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        if(Cursor.lockState == CursorLockMode.Locked) {
            var mouseMovementX = Input.GetAxis("Mouse X") * 100.0f * Time.deltaTime;
            var cameraRotateY = Quaternion.AngleAxis(mouseMovementX,Vector3.up);
            cameraRelativePosition = cameraRotateY * cameraRelativePosition;
            cameraRelativeLookAtPoint = cameraRotateY * cameraRelativeLookAtPoint;
            Camera.main.transform.position = tower.position + cameraRelativePosition;
            Camera.main.transform.LookAt(tower.position + cameraRelativeLookAtPoint);
            var towerTargetRotation = Quaternion.LookRotation(new(cameraRelativeLookAtPoint.x,0f,cameraRelativeLookAtPoint.z),transform.up) * towerAdjustRotation;
            tower.rotation = Quaternion.Slerp(tower.rotation,towerTargetRotation,3f * Time.deltaTime);
        }
        if(Input.GetKeyDown(KeyCode.Mouse0)) {
            Instantiate(bulletPrefab,bulletSpawnPoint.position,bulletSpawnPoint.rotation).Init();
        }
    }

    private void FixedUpdate() {
        float inputMoveDirection = 0f;
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            inputMoveDirection = 1f;
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            inputMoveDirection = -1f;
        }
        rigidbody.MovePosition(transform.position + inputMoveDirection * moveSpeed * Time.fixedDeltaTime * transform.forward);

        float inputTurnDirection = 0f;
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            inputTurnDirection = 1f;
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            inputTurnDirection = -1f;
        }
        rigidbody.MoveRotation(Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y + Time.fixedDeltaTime * inputTurnDirection * turnSpeed * Mathf.Rad2Deg,
            transform.rotation.eulerAngles.z
        ));

        float rightThreadAnimationSpeed = 0f;
        float leftThreadAnimationSpeed = 0f;
        if(inputMoveDirection == 0f && inputTurnDirection != 0f) {
            rightThreadAnimationSpeed = -inputTurnDirection * turnSpeed * Mathf.PI;
            leftThreadAnimationSpeed = +inputTurnDirection * turnSpeed * Mathf.PI;
        }
        else if(inputMoveDirection != 0f && inputTurnDirection == 0f) {
            rightThreadAnimationSpeed = inputMoveDirection * moveSpeed;
            leftThreadAnimationSpeed = inputMoveDirection * moveSpeed;
        }
        else if(inputMoveDirection != 0f && inputTurnDirection != 0f) {
            rightThreadAnimationSpeed = (inputTurnDirection > 0f ? 0.75f : 1.0f) * inputMoveDirection * moveSpeed;
            leftThreadAnimationSpeed = (inputTurnDirection < 0f ? 0.75f : 1.0f) * inputMoveDirection * moveSpeed;
        }
        rightTreadAnimation.SetFloat("MoveSpeed",rightThreadAnimationSpeed);
        leftTreadAnimation.SetFloat("MoveSpeed",leftThreadAnimationSpeed);
    }
}
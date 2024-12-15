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

    private new Rigidbody rigidbody;
    private Vector3 cameraOffset;

    private readonly Quaternion towerAdjustRotation = Quaternion.Euler(90f,0f,0f);
    private readonly RaycastHit[] towerTargetHits = new RaycastHit[1];
    private Vector3 towerTargetPoint = new();

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        cameraOffset = transform.position - 8f * transform.forward + 13f * transform.up + 10f * transform.right;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        float mouseMovementX = Input.GetAxis("Mouse X") * 100.0f * Time.deltaTime;
        cameraOffset = Quaternion.AngleAxis(mouseMovementX,Vector3.up) * cameraOffset;
        Camera.main.transform.position = transform.position + cameraOffset;
        Camera.main.transform.LookAt(transform.position + 8f * transform.up);

        if(!Input.GetKey(KeyCode.LeftShift)) {
            var towerRotation = Quaternion.LookRotation(new Vector3(towerTargetPoint.x,tower.position.y,towerTargetPoint.z) - tower.position);
            tower.rotation = Quaternion.Slerp(tower.rotation,towerRotation * towerAdjustRotation,Time.deltaTime * towerRotateSpeed);
            float gunAngle = Mathf.Tan((gun.position.y - towerTargetPoint.y) / Vector3.Distance(gun.position,towerTargetPoint));
            gunAngle = Mathf.Clamp(gunAngle * Mathf.Rad2Deg,-10f,10f);
            gun.localRotation = Quaternion.Slerp(gun.localRotation,Quaternion.Euler(gunAngle,0f,0f),Time.deltaTime * gunAdjustSpeed);
        }

        /*if(Input.GetKeyDown(KeyCode.Mouse0)) {
            var bullet = Instantiate(bulletPrefab,bulletSpawnPoint.position,Quaternion.identity);
            bullet.Rigidbody.AddForce(bulletSpawnPoint.forward * 100f,ForceMode.VelocityChange);
        }*/
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

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.RaycastNonAlloc(ray,towerTargetHits,200f) > 0) {
            towerTargetPoint = towerTargetHits[0].point;
        }

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
            rightThreadAnimationSpeed = (inputTurnDirection > 0f ? 0.5f : 1.0f) * inputMoveDirection * moveSpeed;
            leftThreadAnimationSpeed = (inputTurnDirection < 0f ? 0.5f : 1.0f) * inputMoveDirection * moveSpeed;
        }
        rightTreadAnimation.SetFloat("MoveSpeed",rightThreadAnimationSpeed);
        leftTreadAnimation.SetFloat("MoveSpeed",leftThreadAnimationSpeed);
    }
}
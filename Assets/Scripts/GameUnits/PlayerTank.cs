using UnityEngine;

public class PlayerTank : Tank {
    [SerializeField]
    private PlayerUI playerUI;

    public static PlayerTank instance;

    private readonly Quaternion cameraFixRotation = Quaternion.Euler(90f,0f,0f);
    private readonly RaycastHit[] mouseRaycastHits = new RaycastHit[1];

    private readonly Quaternion tankWRotation = Quaternion.Euler(0f,0f,0f);
    private readonly Quaternion tankSRotation = Quaternion.Euler(0f,180f,0f);
    private readonly Quaternion tankDRotation = Quaternion.Euler(0f,90f,0f);
    private readonly Quaternion tankARotation = Quaternion.Euler(0f,270f,0f);

    private bool setInstanceToNullOnDestruction = true;

    protected override void Awake() {
        base.Awake();
        if(instance != null) {
            setInstanceToNullOnDestruction = false;
            Destroy(gameObject);
            Debug.LogError("At most one instance of \"PlayerTank\" can be present on scene at any moment.");
        }
        else instance = this;
    }

    private void OnDestroy() {
        if(!setInstanceToNullOnDestruction) return;
        instance = null;
    }

    private void Update() {
        playerUI.SetHealthPercent((float)health / maxHealth);
        Vector3 cameraPos = new(currentGun.transform.position.x,currentGun.transform.position.y + 15f,currentGun.transform.position.z);
        Camera.main.transform.SetPositionAndRotation(cameraPos,cameraFixRotation);
        var targetRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.RaycastNonAlloc(targetRay,mouseRaycastHits) > 0) {
            var vector = mouseRaycastHits[0].point - transform.position;
            vector.y = 0f;
            currentGun.transform.rotation = Quaternion.LookRotation(vector) * gunFixRotation;
        }
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        if(Input.GetKey(KeyCode.Mouse0)) Shoot();
        float moveStep = Time.fixedDeltaTime * Time.timeScale;
        float moveX = 0f;
        float moveZ = 0f;
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            moveX = 1f;
            transform.rotation = Quaternion.Slerp(transform.rotation,tankWRotation,moveStep * 5f);
        }
        else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            moveX = -1f;
            transform.rotation = Quaternion.Slerp(transform.rotation,tankSRotation,moveStep * 5f);
        }
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            moveZ = 1f;
            transform.rotation = Quaternion.Slerp(transform.rotation,tankDRotation,moveStep * 5f);
        }
        else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            moveZ = -1f;
            transform.rotation = Quaternion.Slerp(transform.rotation,tankARotation,moveStep * 5f);
        }
        var moveVector = (moveX * Camera.main.transform.up + moveZ * Camera.main.transform.right).normalized;
        Rigidbody.MovePosition(transform.position + moveSpeed * moveStep * moveVector);
        float threadAnimationSpeed = Mathf.Abs(moveX) + Mathf.Abs(moveZ) > 0.0001f ? moveSpeed * 1.15f : 0f;
        rightTreadAnimation.SetFloat("MoveSpeed",threadAnimationSpeed);
        leftTreadAnimation.SetFloat("MoveSpeed",threadAnimationSpeed);
    }
}
using UnityEngine;

public class PlayerTank : Tank {
    [SerializeField]
    private PlayerUI playerUI;

    private readonly Quaternion cameraRotation = Quaternion.Euler(90f,90f,0f);
    private readonly Quaternion gunFixRotation = Quaternion.Euler(90f,0f,0f);

    private void Update() {
        playerUI.SetHealthPercent((float)health / maxHealth);
        Vector3 cameraPos = new(transform.position.x,transform.position.y + 15f,transform.position.z);
        Camera.main.transform.SetPositionAndRotation(cameraPos,cameraRotation);
        var targetRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(targetRay,out RaycastHit hitInfo)) {
            var vector = hitInfo.point - transform.position;
            vector.y = 0f;
            currentGun.transform.rotation = Quaternion.LookRotation(vector) * gunFixRotation;
        }
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        if(Input.GetKey(KeyCode.Mouse0)) Shoot();
        float step = Time.fixedDeltaTime * Time.timeScale;
        float moveX = 0f;
        float moveZ = 0f;
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            moveX = 1f;
            //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,0f,transform.rotation.eulerAngles.z);
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            moveX = -1f;
        }
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            moveZ = 1f;
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            moveZ = -1f;
        }
        var nextPos = transform.position;
        nextPos += moveSpeed * moveX * step * Camera.main.transform.up;
        nextPos += moveSpeed * moveZ * step * Camera.main.transform.right;
        Rigidbody.MovePosition(nextPos);
    }
}
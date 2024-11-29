using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerTank : MonoBehaviour {
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private Transform cannon;
    [SerializeField]
    private Transform cannonEndPoint;
    [SerializeField]
    private GameObject bulletPrefab;
    private new Rigidbody rigidbody;

    private Camera playerCamera;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        playerCamera = FindFirstObjectByType<Camera>();
    }

    /*private void Update() {
        var camPos = playerCamera.transform.position;
        float camPosLerpFactor = 2.0f * Time.deltaTime;
        playerCamera.transform.position = new(Mathf.Lerp(camPos.x,transform.position.x,camPosLerpFactor),camPos.y,Mathf.Lerp(camPos.z,transform.position.z,camPosLerpFactor));

        var vector = Input.mousePosition - playerCamera.WorldToScreenPoint(transform.position);
        cannon.rotation = Quaternion.Euler(0.0f,Mathf.Atan2(vector.x,vector.y) * Mathf.Rad2Deg,0.0f);

        if(Input.GetKey(KeyCode.W)) {
            rigidbody.AddForce(speed * Time.deltaTime * transform.forward,ForceMode.VelocityChange);
        }
        if(Input.GetKey(KeyCode.S)) {
            rigidbody.AddForce(speed * Time.deltaTime * -transform.forward,ForceMode.VelocityChange);
        }
        if(Input.GetKey(KeyCode.D)) {
            var angles = transform.rotation.eulerAngles;
            angles.y += rotationSpeed * Time.deltaTime * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(angles);
        }
        if(Input.GetKey(KeyCode.A)) {
            var angles = transform.rotation.eulerAngles;
            angles.y -= rotationSpeed * Time.deltaTime * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(angles);
        }
        if(Input.GetKeyDown(KeyCode.Mouse0)) {
            var bullet = Instantiate(bulletPrefab,cannonEndPoint.position + cannon.forward * 0.25f,Quaternion.Euler(Vector3.zero));
            bullet.GetComponent<Bullet>().owner = gameObject;
            bullet.GetComponent<Rigidbody>().AddForce(20.0f * cannon.forward,ForceMode.VelocityChange);
        }
    }*/
}

using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTank : Tank {
    [SerializeField]
    protected float incrementRoundsPerSecond;
    [SerializeField]
    protected int incrementDamage;
    [SerializeField]
    private TankGun currentGunPrefab;

    private NavMeshPath path = null;
    private int currentPathPointIndex = 0;

    public override void SwitchGun(TankGun gunPrefab) {
        base.SwitchGun(gunPrefab);
        CurrentGun.baseRoundsPerSecond += incrementRoundsPerSecond;
        CurrentGun.baseDamage += incrementDamage;
    }

    protected override void Awake() {
        base.Awake();
        SwitchGun(currentGunPrefab ? currentGunPrefab : ReferenceHub.instance.machineGunPrefab);
    }

    protected override void FixedUpdate() {
        if(health <= 0) {
            rightTreadAnimation.SetFloat("MoveSpeed",0f);
            leftTreadAnimation.SetFloat("MoveSpeed",0f);
            Destroy(this);
            return;
        }
        if(!PlayerTank.instance || Vector3.Distance(PlayerTank.instance.transform.position,transform.position) > 15f) {
            path = null;
            rightTreadAnimation.SetFloat("MoveSpeed",0f);
            leftTreadAnimation.SetFloat("MoveSpeed",0f);
            return;
        }

        if(path == null) {
            var gunPosition = CurrentGun.transform.position;
            var raycastDirection = (PlayerTank.instance.CurrentGun.transform.position - gunPosition).normalized;
            if(!Physics.Raycast(gunPosition,raycastDirection,out RaycastHit raycastHit,15f)) return;
            if(ReferenceEquals(raycastHit.transform.gameObject,PlayerTank.instance.gameObject)) path = new();
            return;
        }
        if(path.corners == null || path.corners.Length == 0 ||
           Vector3.Distance(path.corners.Last(),PlayerTank.instance.transform.position) > 10f) {
            currentPathPointIndex = 1;
            path.ClearCorners();
            NavMesh.CalculatePath(transform.position,PlayerTank.instance.transform.position,NavMesh.AllAreas,path);
            return;
        }

        var moveStep = Time.fixedDeltaTime * Time.timeScale;
        if(Vector3.Distance(PlayerTank.instance.transform.position,transform.position) <= 10f) {
            var newGunRotation = Quaternion.LookRotation(PlayerTank.instance.CurrentGun.transform.position - CurrentGun.transform.position) * gunFixRotation;
            CurrentGun.transform.rotation = Quaternion.Slerp(CurrentGun.transform.rotation,newGunRotation,moveStep * 3f);
            CurrentGun.Shoot();
        }

        var treadAnimationSpeed = 0f;
        if(currentPathPointIndex < path.corners.Length) {
            treadAnimationSpeed = baseMoveSpeed * 1.2f;

            var nextPathPoint = path.corners[currentPathPointIndex];
            var newPosition = Vector3.MoveTowards(transform.position,nextPathPoint,baseMoveSpeed * moveStep);
            Rigidbody.MovePosition(newPosition);

            if(Vector3.Distance(newPosition,nextPathPoint) > 0.01f) {
                var newRotation = Quaternion.LookRotation(nextPathPoint - newPosition);
                Rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation,newRotation,moveStep * 4f));
            }
            else currentPathPointIndex += 1;
        }
        rightTreadAnimation.SetFloat("MoveSpeed",treadAnimationSpeed);
        leftTreadAnimation.SetFloat("MoveSpeed",treadAnimationSpeed);
    }
}
using UnityEngine;

public class EnemyTank : Tank {
    [SerializeField]
    private LayerMask playerOverlapSphereTargetMask;
    [SerializeField]
    private LayerMask playerRaycastTargetMask;
    [SerializeField]
    private LayerMask destroyableBlockMask;

    private PlayerTank targetPlayerTank = null;
    private enum State { Idle,Chase,Attack }
    private State state = State.Idle;
    private readonly Collider[] idleOverlapColliders = new Collider[1];
    //private Vector3 chaseLastPos

    private void IdleStateFixedUpdate() {
        targetPlayerTank = null;
        var overlapColliderCount = Physics.OverlapSphereNonAlloc(transform.position,10f,idleOverlapColliders,playerOverlapSphereTargetMask);
        for(int i = 0;i < overlapColliderCount;i += 1) {
            if(!idleOverlapColliders[i].TryGetComponent(out targetPlayerTank)) continue;
            var raycastDirection = (targetPlayerTank.transform.position - transform.position).normalized;

            if(Physics.Raycast(transform.position,raycastDirection,out RaycastHit playerRaycastHit,10f,playerRaycastTargetMask) &&
               ReferenceEquals(playerRaycastHit.transform.gameObject,targetPlayerTank.gameObject)) {
                state = State.Attack;
            }
        }
    }

    private void ChaseStateFixedUpdate() {

    }

    private void AttackStateFixedUpdate() {
        float moveStep = Time.fixedDeltaTime * Time.timeScale;
        var raycastDirection = (targetPlayerTank.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(raycastDirection),moveStep * 5f);
        Shoot();

        /*if(Vector3.Distance(transform.position,targetPlayerTank.transform.position) > 20f) {
            targetPlayerTank = null;
            state = State.Idle;
            return;
        }

        var raycastDirection = (targetPlayerTank.transform.position - transform.position).normalized;
        if(Physics.Raycast(transform.position,raycastDirection,out RaycastHit playerRaycastHit,20f,playerRaycastTargetMask)) {
            if(!ReferenceEquals(playerRaycastHit.transform.gameObject,targetPlayerTank.gameObject)) {

            }
        }*/

        //ReferenceEquals(playerRaycastHit.transform.gameObject,targetPlayerTank.gameObject)
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        if(health <= 0) {
            Destroy(this);
            return;
        }

        if(state == State.Idle) {
            IdleStateFixedUpdate();
        }
        else if(state == State.Chase) {
            ChaseStateFixedUpdate();
        }
        else if(state == State.Attack) {
            AttackStateFixedUpdate();
        }

        /*threadAnimationSpeed = 0f;
        float moveStep = Time.fixedDeltaTime * Time.timeScale;
        if(targetPlayerTank) {
            bool doMove = true;
            var distance = Vector3.Distance(targetPlayerTank.transform.position,transform.position);
            moveDirection = (targetPlayerTank.currentGun.transform.position - currentGun.transform.position).normalized;

            if(Physics.Raycast(currentGun.transform.position,moveDirection,out playerRaycastHit,15f,playerRaycastTargetMask) &&
               ReferenceEquals(playerRaycastHit.transform.gameObject,targetPlayerTank.gameObject)) {
                currentGun.transform.rotation = Quaternion.LookRotation(moveDirection) * gunFixRotation;
                
                doMove = distance >= 5f;
            }
            if(doMove) {
                transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(moveDirection),moveStep * 5f);
                Rigidbody.MovePosition(transform.position + moveSpeed * moveStep * moveDirection);
                threadAnimationSpeed = moveSpeed * 1.15f;
            }
        }
        else {
            var overlapColliderCount = Physics.OverlapSphereNonAlloc(transform.position,10f,overlapColliders,playerOverlapSphereTargetMask);
            for(int i = 0;i < overlapColliderCount;i += 1) if(overlapColliders[i].TryGetComponent(out targetPlayerTank)) break;
        }

        rightTreadAnimation.SetFloat("MoveSpeed",threadAnimationSpeed);
        leftTreadAnimation.SetFloat("MoveSpeed",threadAnimationSpeed);*/
    }
}
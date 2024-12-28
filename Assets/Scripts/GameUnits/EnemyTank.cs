using UnityEngine;

public class EnemyTank : Tank {
    private readonly RaycastHit[] playerRaycastHits = new RaycastHit[1];

    protected override void FixedUpdate() {
        base.FixedUpdate();

        if(Vector3.Distance(PlayerTank.instance.currentGun.transform.position,currentGun.transform.position) <= 15f) {
            var vector = PlayerTank.instance.transform.position - transform.position;
            vector.y = 0f;
            currentGun.transform.rotation = Quaternion.Slerp(currentGun.transform.rotation,Quaternion.LookRotation(vector) * gunFixRotation,Time.fixedDeltaTime * 5f);
        }

        rightTreadAnimation.SetFloat("MoveSpeed",0f);
        leftTreadAnimation.SetFloat("MoveSpeed",0f);
    }
}
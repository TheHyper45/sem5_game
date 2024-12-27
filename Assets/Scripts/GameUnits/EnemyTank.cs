using UnityEngine;

public class EnemyTank : Tank {
    /*private DrivableGameUnit gameUnit;
    private NavMeshPath currentPath;
    private int currentPointOnPathIndex;
    private Vector3 prevTransformPosition;

    private void Awake() {
        gameUnit = GetComponent<DrivableGameUnit>();
        prevTransformPosition = transform.position;
    }

    private void Start() {
        currentPath = new();
        currentPointOnPathIndex = 0;
        NavMesh.CalculatePath(transform.position,DrivableGameUnit.player.transform.position,NavMesh.AllAreas,currentPath);
    }

    private void OnDrawGizmos() {
        if(!Application.IsPlaying(gameObject)) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLineStrip(currentPath.corners,false);
    }

    private void Update() {
        if(gameUnit.health <= 0) {
            Destroy(this);
            return;
        }
        if(!DrivableGameUnit.player) return;
        var diff = DrivableGameUnit.player.currentGun.transform.position - transform.position;
        gameUnit.RotateGunTowardsLocalUpdate(diff.x,diff.z,Time.deltaTime);
        gameUnit.Shoot();
    }*/
}
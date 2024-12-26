using UnityEngine;
//using UnityEngine.AI;

public class AIDrivableGameUnitController : MonoBehaviour {
    private DrivableGameUnit gameUnit;
    //private NavMeshAgent navMeshAgent;

    private void Awake() {
        gameUnit = GetComponent<DrivableGameUnit>();
        //navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        if(gameUnit.health <= 0) {
            //Destroy(navMeshAgent);
            Destroy(this);
            return;
        }
        var dist = Vector3.Distance(transform.position,DrivableGameUnit.player.transform.position);
        if(dist >= 5f && dist <= 20f) {
            //navMeshAgent.isStopped = false;
            //navMeshAgent.destination = DrivableGameUnit.player.transform.position;
        }
        else {
            //navMeshAgent.isStopped = true;
        }
    }
}
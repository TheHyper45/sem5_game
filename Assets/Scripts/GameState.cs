using System;
using UnityEngine;

public class GameState : MonoBehaviour {
    public bool GameHasEnded { get; private set; } = false;
    [NonSerialized,HideInInspector]
    public int playerCollectedMoney;

    public static GameState instance;

    private void Awake() {
        instance = this;
        playerCollectedMoney = 0;
    }

    private void OnDestroy() {
        instance = null;
    }

    private void FixedUpdate() {
        if(GameHasEnded) return;
        if(EnemySpawnBase.spawnBases.Count == 0) {
            Instantiate(ReferenceHub.instance.levelCompletedMenuPrefab,Vector3.zero,Quaternion.identity);
            GameHasEnded = true;
        }
        else if(!PlayerTank.instance || PlayerTank.instance.health <= 0) {
            Instantiate(ReferenceHub.instance.levelFailedMenuPrefab,Vector3.zero,Quaternion.identity);
            GameHasEnded = true;
        }
    }
}
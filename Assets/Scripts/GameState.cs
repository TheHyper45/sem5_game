using System;
using UnityEngine;

/*[Serializable]
public struct GameSaveDataJson {
    [Serializable]
    public enum LevelState {
        Locked,
        Unlocked,
        Completed
    }

    [Serializable]
    public struct LevelDataJson {
        public string name;
        public LevelState status;
    }

    public LevelDataJson[] levelDatas;
}*/

public class GameState : MonoBehaviour {
    public static GameState instance;

    public TankGun machineGunPrefab;
    public Bullet bulletPrefab;

    private void Awake() {
        if(instance) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
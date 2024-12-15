using System;
using UnityEngine;

public class GameState : MonoBehaviour {
    public static GameState instance;
    [HideInInspector,NonSerialized]
    public string currentSaveFilePath;

    private void Awake() {
        currentSaveFilePath = "";
        if(instance == null) {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else Destroy(gameObject);
    }
}

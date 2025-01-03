using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public struct GameSaveDataJson {
    [Serializable]
    public struct UpgradeDataJson {
        public string name;
        public int level;
    }
    [Serializable]
    public struct LevelDataJson {
        public string name;
        public bool completed;
        public int score;
    }
    public List<LevelDataJson> levelDatas;
    public List<UpgradeDataJson> upgradeDatas;
    public int money;
}

public class GameState : MonoBehaviour {
    public static GameState instance;

    public TankGun machineGunPrefab;
    public Bullet bulletPrefab;
    public MoneyPickup moneyPickupPrefab;

    private string saveFilePath = "";
    private GameSaveDataJson gameSaveDataJson;

    public GameSaveDataJson.LevelDataJson GetLevelData(string levelName) {
        var index = gameSaveDataJson.levelDatas.FindIndex((item) => item.name == levelName);
        if(index < 0) return new() { name = levelName,completed = false,score = 0};
        return gameSaveDataJson.levelDatas[index];
    }

    public void MarkLevelCompleted(string levelName,int moneyIncrement,int score = 1) {
        var index = gameSaveDataJson.levelDatas.FindIndex((item) => item.name == levelName);
        if(index < 0) {
            gameSaveDataJson.levelDatas.Add(new() { name = levelName,completed = true,score = score });
            gameSaveDataJson.money += moneyIncrement;
            SaveGameSaveData();
            return;
        }
        var data = gameSaveDataJson.levelDatas[index];
        data.completed = true;
        data.score = score;
        gameSaveDataJson.levelDatas[index] = data;
        gameSaveDataJson.money += moneyIncrement;
        SaveGameSaveData();
    }

    public int GetMoney() {
        return gameSaveDataJson.money;
    }

    public bool TakeMoney(int amount) {
        if(gameSaveDataJson.money < amount) return false;
        gameSaveDataJson.money -= amount;
        SaveGameSaveData();
        return true;
    }

    public bool IsGameSaveDataLoaded() {
        return !string.IsNullOrEmpty(saveFilePath);
    }

    public void ClearSaveData() {
        gameSaveDataJson = new();
        saveFilePath = "";
    }

    public GameSaveDataJson CreateSaveFile(string filePath) {
        GameSaveDataJson json = new() { levelDatas = new() };
        json.levelDatas.Add(new() { name = "Level1",completed = false,score = 0 });
        File.WriteAllText(filePath,JsonUtility.ToJson(json));
        return json;
    }

    public void LoadGameSaveData(string filePath) {
        try {
            gameSaveDataJson = JsonUtility.FromJson<GameSaveDataJson>(File.ReadAllText(filePath));
            saveFilePath = filePath;
        }
        catch(FileNotFoundException) {
            gameSaveDataJson = CreateSaveFile(filePath);
            saveFilePath = filePath;
        }
    }

    private void SaveGameSaveData() {
        File.WriteAllText(saveFilePath,JsonUtility.ToJson(gameSaveDataJson));
    }

    private void Awake() {
        if(instance) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
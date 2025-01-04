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

public static class SaveFile {
    private static string saveFilePath = "";
    private static GameSaveDataJson gameSaveDataJson = new();

    private static GameSaveDataJson CreateNewInternal(string filePath) {
        GameSaveDataJson json = new() { levelDatas = new() };
        json.levelDatas.Add(new() { name = "Level1",completed = false,score = 0 });
        File.WriteAllText(filePath,JsonUtility.ToJson(json));
        return json;
    }

    public static void CreateNew(string filePath) {
        CreateNewInternal(filePath);
    }

    public static void Load(string filePath) {
        try {
            gameSaveDataJson = JsonUtility.FromJson<GameSaveDataJson>(File.ReadAllText(filePath));
            saveFilePath = filePath;
        }
        catch(FileNotFoundException) {
            gameSaveDataJson = CreateNewInternal(filePath);
            saveFilePath = filePath;
        }
    }

    private static void Save() {
        File.WriteAllText(saveFilePath,JsonUtility.ToJson(gameSaveDataJson));
    }

    public static void Unload() {
        gameSaveDataJson = new();
        saveFilePath = "";
    }

    public static bool IsLoaded() {
        return !string.IsNullOrEmpty(saveFilePath);
    }

    public static int GetMoney() {
        return gameSaveDataJson.money;
    }

    public static void AddMoney(int amount) {
        gameSaveDataJson.money += amount;
        Save();
    }

    public static bool TakeMoney(int amount) {
        if(gameSaveDataJson.money >= amount) {
            gameSaveDataJson.money -= amount;
            Save();
            return true;
        }
        return false;
    }

    public static GameSaveDataJson.LevelDataJson GetLevelData(string levelName) {
        var index = gameSaveDataJson.levelDatas.FindIndex((item) => item.name == levelName);
        if(index < 0) return new() { name = levelName,completed = false,score = 0 };
        return gameSaveDataJson.levelDatas[index];
    }

    public static void MarkLevelCompleted(string levelName,int moneyIncrement,int score = 1) {
        var index = gameSaveDataJson.levelDatas.FindIndex((item) => item.name == levelName);
        if(index < 0) {
            gameSaveDataJson.levelDatas.Add(new() { name = levelName,completed = true,score = score });
            gameSaveDataJson.money += moneyIncrement;
            Save();
            return;
        }
        var data = gameSaveDataJson.levelDatas[index];
        data.completed = true;
        data.score = score;
        gameSaveDataJson.levelDatas[index] = data;
        gameSaveDataJson.money += moneyIncrement;
        Save();
    }
}
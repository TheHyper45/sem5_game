using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public struct GameSaveDataJson {
    [Serializable]
    public struct LevelDataJson {
        public string name;
        public bool completed;
        public int score;
    }
    [Serializable]
    public struct UpgradeDataJson {
        public string name;
        public int level;
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

    public static void Unload() {
        gameSaveDataJson = new();
        saveFilePath = "";
    }

    private static void Save() {
        File.WriteAllText(saveFilePath,JsonUtility.ToJson(gameSaveDataJson));
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

    public static int GetUpgradeLevel(string upgradeName) {
        var index = gameSaveDataJson.upgradeDatas.FindIndex((item) => item.name == upgradeName);
        if(index < 0) return 0;
        return Mathf.Clamp(gameSaveDataJson.upgradeDatas[index].level,0,ShopUpgrades.GetUpgradeDescription(upgradeName).levels.Length);
    }

    public static bool CanBuyUpgrade(string upgradeName) {
        var upgradeDescription = ShopUpgrades.GetUpgradeDescription(upgradeName);
        if(upgradeDescription == null) return false;
        var upgradeLevel = GetUpgradeLevel(upgradeName);
        if(upgradeLevel >= upgradeDescription.levels.Length) return false;
        if(gameSaveDataJson.money < upgradeDescription.levels[upgradeLevel].cost) return false;
        return true;
    }

    public static void BuyUpgrade(string upgradeName) {
        if(!CanBuyUpgrade(upgradeName)) return;
        var index = gameSaveDataJson.upgradeDatas.FindIndex((item) => item.name == upgradeName);
        if(index < 0) {
            GameSaveDataJson.UpgradeDataJson tmp2 = new() { name = upgradeName,level = 0 };
            var upgrade2 = ShopUpgrades.GetUpgradeDescription(upgradeName).levels[tmp2.level];
            tmp2.level += 1;
            gameSaveDataJson.upgradeDatas.Add(tmp2);
            gameSaveDataJson.money -= upgrade2.cost;
            Save();
            return;
        }
        var tmp = gameSaveDataJson.upgradeDatas[index];
        var upgrade = ShopUpgrades.GetUpgradeDescription(upgradeName).levels[tmp.level];
        tmp.level += 1;
        gameSaveDataJson.upgradeDatas[index] = tmp;
        gameSaveDataJson.money -= upgrade.cost;
        Save();
    }
}
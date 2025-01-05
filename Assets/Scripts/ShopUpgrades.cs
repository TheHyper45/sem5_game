public class ShopUpgradeDescription {
    public struct Level {
        public int cost;
        public object value;
    }
    public string label;
    public Level[] levels;
}

public struct ShopMachineGunUpgradeValue {
    public int damage;
    public float roundsPerSecond;
}

public static class ShopUpgrades {
    public static readonly ShopUpgradeDescription[] upgrades = new ShopUpgradeDescription[] {
        new() {
            label = "Health",
            levels = new ShopUpgradeDescription.Level[] {
                new(){cost = 100,value = 20},
                new(){cost = 150,value = 40},
                new(){cost = 300,value = 50},
                new(){cost = 600,value = 60},
                new(){cost = 1200,value = 70},
                new(){cost = 2400,value = 80},
            }
        },
        new() {
            label = "Speed",
            levels = new ShopUpgradeDescription.Level[] {
                new(){cost = 80,value = 0.5f},
                new(){cost = 160,value = 1.0f},
                new(){cost = 240,value = 1.25f},
                new(){cost = 500,value = 1.5f},
                new(){cost = 1000,value = 2.0f},
            }
        },
        new() {
            label = "Machine Gun",
            levels = new ShopUpgradeDescription.Level[] {
                new() {cost = 110,value = new ShopMachineGunUpgradeValue(){damage = 2,roundsPerSecond = 0.5f}},
                new() {cost = 220,value = new ShopMachineGunUpgradeValue(){damage = 3,roundsPerSecond = 1.0f}},
                new() {cost = 500,value = new ShopMachineGunUpgradeValue(){damage = 5,roundsPerSecond = 1.5f}},
                new() {cost = 1200,value = new ShopMachineGunUpgradeValue(){damage = 7,roundsPerSecond = 2.0f}},
                new() {cost = 2000,value = new ShopMachineGunUpgradeValue(){damage = 9,roundsPerSecond = 2.5f}}
            }
        },
        new() {
            label = "Shot Gun",
            levels = new ShopUpgradeDescription.Level[] {
                new() {cost = 120,value = new ShopMachineGunUpgradeValue(){damage = 0,roundsPerSecond = 0.0f}},
                new() {cost = 240,value = new ShopMachineGunUpgradeValue(){damage = 2,roundsPerSecond = 0.5f}},
                new() {cost = 500,value = new ShopMachineGunUpgradeValue(){damage = 3,roundsPerSecond = 1.0f}},
                new() {cost = 900,value = new ShopMachineGunUpgradeValue(){damage = 5,roundsPerSecond = 1.5f}},
                new() {cost = 1400,value = new ShopMachineGunUpgradeValue(){damage = 7,roundsPerSecond = 2.0f}},
                new() {cost = 2100,value = new ShopMachineGunUpgradeValue(){damage = 9,roundsPerSecond = 2.5f}}
            }
        }
    };
    public static ShopUpgradeDescription GetUpgradeDescription(string name) {
        foreach(var upgrade in upgrades) {
            if(upgrade.label == name) return upgrade;
        }
        return null;
    }
}
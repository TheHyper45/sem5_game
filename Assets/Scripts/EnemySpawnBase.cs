using System;
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawnBase : MonoBehaviour {
    [Serializable]
    private class SpawnPoint {
        public Transform point;
        [NonSerialized]
        public Tank spawnedTank;
    }

    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private Tank spawnTankPrefab;
    [SerializeField]
    private float tankSpawnTime;
    [SerializeField]
    private int maxSpawnTankCount;
    [SerializeField]
    private SpawnPoint[] spawnPoints;

    private int health;
    private float timer = 0f;
    private int spawnedTankCount = 0;
    public static readonly List<EnemySpawnBase> spawnBases = new();

    private void Awake() {
        spawnBases.Add(this);
    }

    private void OnDestroy() {
        spawnBases.Remove(this);
    }

    private void Start() {
        health = maxHealth;
        healthBar.SetValue((float)health / maxHealth);
    }

    private void SpawnTank() {
        if(spawnedTankCount >= maxSpawnTankCount && maxSpawnTankCount >= 0) return;
        for(int i = 0;i < spawnPoints.Length;i += 1) {
            var point = spawnPoints[i];
            if(point.spawnedTank) continue;
            point.spawnedTank = Instantiate(spawnTankPrefab,point.point.position,Quaternion.identity);
            spawnedTankCount += 1;
            return;
        }
    }

    private void FixedUpdate() {
        if(health <= 0) return;
        timer += Time.fixedDeltaTime * Time.timeScale;
        if(timer >= tankSpawnTime) {
            SpawnTank();
            timer -= tankSpawnTime;
        }
    }

    public void Hit(int damage) {
        if(health <= 0) return;
        health -= damage;
        healthBar.SetValue((float)health / maxHealth);
        if(health > 0) return;
        Destroy(gameObject);
    }
}
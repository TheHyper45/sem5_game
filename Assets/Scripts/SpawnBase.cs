using System;
using UnityEngine;

public class SpawnBase : MonoBehaviour {
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
    private SpawnPoint[] spawnPoints;

    private int health;
    private float timer = 0f;

    private void Start() {
        health = maxHealth;
        healthBar.SetValue((float)health / maxHealth);
    }

    private void SpawnTank() {
        for(int i = 0;i < spawnPoints.Length;i += 1) {
            var point = spawnPoints[i];
            if(point.spawnedTank) continue;
            point.spawnedTank = Instantiate(spawnTankPrefab,point.point.position,Quaternion.identity);
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
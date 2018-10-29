using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour {

    public int currentRound = 1;
    public int roundsBeforePauseNum = 5;
    public int maxEnemyMultiplier = 1;
    public int currentEnemyCount;
    public int maxEnemyCount;
    public int EnemiesSpawned;
    public int totalEnemyCount;

    public GameObject[] enemies;

    private Transform[] spawners;

    private void Start()
    {
        spawners = gameObject.GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        maxEnemyCount = currentRound * maxEnemyMultiplier;
        currentEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (currentEnemyCount < maxEnemyCount && EnemiesSpawned < totalEnemyCount)
        {
            SpawnEnemy();
        }

        if (EnemiesSpawned == totalEnemyCount && currentEnemyCount == 0)
        {
            currentRound++;
        }
    }

    private void SpawnEnemy()
    {
        int randEnemy = Random.Range(0, enemies.Length);
        int randSpawner = Random.Range(0, spawners.Length);
        Instantiate(enemies[randEnemy], spawners[randSpawner].position, spawners[randSpawner].rotation);
        EnemiesSpawned++;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawning : MonoBehaviour {

    public int currentWave = 0;
    public int roundsBeforePauseNum = 5;
    public int maxEnemyMultiplier = 1;
    public int currentEnemyCount;
    public int maxEnemyCount;
    public int EnemiesSpawned;
    public int totalEnemyCount;
    public int downTimeForNextWave;

    public GameObject[] enemies;

    public List<Transform> spawners;

    public Text WaveText;

    private bool isPaused = false;

    private void Start()
    {
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
        {
            if (child.tag == "Spawner")
            {
                spawners.Add(child);
            }
        }
        StartCoroutine(NewWave());
    }

    private void Update()
    {
        WaveText.text = "- Wave " + currentWave + " -";

        if (!isPaused)
        {
            currentEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (currentEnemyCount < maxEnemyCount && EnemiesSpawned < totalEnemyCount)
            {
                SpawnEnemy();
            }

            if (EnemiesSpawned == totalEnemyCount && currentEnemyCount == 0)
            {
                StartCoroutine(NewWave());
            }
        }
    }

    private void SpawnEnemy()
    {
        int randEnemy = Random.Range(0, enemies.Length);
        int randSpawner = Random.Range(1, spawners.Count);
        Instantiate(enemies[randEnemy], spawners[randSpawner].position, spawners[randSpawner].rotation);
        EnemiesSpawned++;
    }

    private IEnumerator NewWave()
    {
        currentWave++;
        maxEnemyCount = currentWave * maxEnemyMultiplier;
        totalEnemyCount = maxEnemyCount * 3;
        EnemiesSpawned = 0;

        yield return new WaitForSeconds(2);
        isPaused = false;
    }
}

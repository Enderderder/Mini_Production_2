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

    public GameObject[] enemies;

    public Transform[] spawners;

    public Text WaveText;

    private bool isPaused = false;

    private void Start()
    {
        spawners = gameObject.GetComponentsInChildren<Transform>();
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
        int randSpawner = Random.Range(1, spawners.Length);
        Instantiate(enemies[randEnemy], spawners[randSpawner].position, spawners[randSpawner].rotation);
        EnemiesSpawned++;
    }

    private IEnumerator NewWave()
    {
        isPaused = true;
        if (currentWave % 5 == 0 && currentWave != 0) // Every 5 waves...
        {
            /* Pop up the shop */
            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));
        }

        currentWave++;
        maxEnemyCount = currentWave * maxEnemyMultiplier;
        totalEnemyCount = maxEnemyCount * 3;
        EnemiesSpawned = 0;

        yield return new WaitForSeconds(2);
        isPaused = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawning : MonoBehaviour {

    public int currentWave = 0;
    public int maxEnemyMultiplier = 1;
    public int currentEnemyCount;
    public int maxEnemyCount;
    public int EnemiesSpawned;
    public int totalEnemyCount;
    public float downTimeForNextWave = 15;
    public GameObject wavedowntimeui;
    public Text cooldowntxt;

    public GameObject[] enemies;

    public List<Transform> spawners;

    public Text WaveText;

    private bool isPaused = false;
    private AudioSource audioSource;
    private bool newRound = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        wavedowntimeui.SetActive(false);
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
                if (!newRound)
                {
                    audioSource.Play();
                    newRound = true;
                }
                wavedowntimeui.SetActive(true);
                downTimeForNextWave -= 1 * Time.deltaTime;
                cooldowntxt.text = ((int)(Mathf.RoundToInt(downTimeForNextWave * 100f) / 100f)).ToString();
                if (downTimeForNextWave <= 0) {
                    StartCoroutine(NewWave());
                    downTimeForNextWave = 15;
                    wavedowntimeui.SetActive(false);
                }
            }
        }
    }

    private void SpawnEnemy()
    {
        int randEnemy = Random.Range(0, 10);
        int randSpawner = Random.Range(1, spawners.Count);
        if (currentWave >= 3)
        {
            if (randEnemy == 0)
            {
                Instantiate(enemies[1], spawners[randSpawner].position, spawners[randSpawner].rotation);
            }
            else
            {
                Instantiate(enemies[0], spawners[randSpawner].position, spawners[randSpawner].rotation);
            }
        }
        else
        {
            Instantiate(enemies[0], spawners[randSpawner].position, spawners[randSpawner].rotation);
        }
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

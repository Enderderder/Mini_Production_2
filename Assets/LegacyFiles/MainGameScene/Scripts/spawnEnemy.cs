using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnEnemy : MonoBehaviour
{
    public GameObject[] slimes;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    private bool gameOver;

    void Start()
    {
        gameOver = false;
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; ++i)
            {
                GameObject hazard = slimes[Random.Range(0, slimes.Length)];

                Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 5.0f);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            while (gameOver)
            {
                //Loop here forever
            }
        }
    }

    public void GameOver()
    {
        gameOver = true;
        Debug.Log("spawnEnemies got GameOver");
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet" || other.tag == "StaticBullet")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}

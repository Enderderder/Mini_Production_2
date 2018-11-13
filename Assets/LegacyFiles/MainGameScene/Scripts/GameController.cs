using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject[] portalandpotions;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public Text manaOverText;
    public Text gameOverText;
    public Text restartText;
    public Text scoreText;
    public Text quoteText;
    public Text manaText;

    private int score;
    //private bool gameOver;
    private bool restart;
    private float spawnX;
    private float spawnZ;
    private spawnEnemy spawnEnemies;

    void Start()
    {
        //gameOver = false;
        restart = false;
        gameOverText.text = "";
        restartText.text = "";
        manaOverText.text = "";
        quoteText.text = "";

        score = 0;
        DisplayScore();
        StartCoroutine(SpawnWaves());
        
        GameObject gameManager = GameObject.FindGameObjectWithTag("SlimeSpawner");
        spawnEnemies = gameManager.GetComponent<spawnEnemy>();

        if (null == spawnEnemies)
        {
            Debug.Log("spawnEnemies null GameController");
        }
    }

    private void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Scene1");
            }
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {

            for (int i = 0; i < hazardCount; ++i)
            {
                if (0 != portalandpotions.Length)
                {
                    GameObject hazard = portalandpotions[Random.Range(0, portalandpotions.Length)];

                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, Random.Range(-spawnValues.z, spawnValues.z));
                    Quaternion spawnRotation = Quaternion.identity;
                    Instantiate(hazard, spawnPosition, spawnRotation);
                }

                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
        }
    }

    public void GameOver()
    {
        manaOverText.text = "You lost all your Mana";
        gameOverText.text = "Game Over";
        //gameOver = true;
        quoteText.text = "Don't walk behind me; I may not lead. Don't walk in front of me; I may not follow. Just walk beside me and be my friend.'' - Albert Camus";

        restartText.text = "Press 'R' to restart";
        restart = true;

        if (null != spawnEnemies)
        {
            spawnEnemies.GameOver();
        }
        else
        {
            Debug.Log("GameController can't find Slime Spawner");
        }
    }

    public void DisplayMana(float amount)
    {
        manaText.text = "Mana: " + amount.ToString();
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        DisplayScore();
    }

    private void DisplayScore()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}

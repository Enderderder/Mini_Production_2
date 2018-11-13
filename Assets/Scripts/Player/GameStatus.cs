using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatus : MonoBehaviour {

    [Header("Stats")]
    public float respawnRate = 10;

    [Header("Gameobjects")]
    public Transform p1Spawn;
    public Transform p2Spawn;
    public Canvas gameOverCanvas;

    [Header("Prefabs")]
    public GameObject player1Prefab;
    public GameObject player2Prefab;

    private Player player1;
    private Player player2;
    private GameObject player1Object;
    private GameObject player2Object;
    private Obelisk obelisk;

    private void Start()
    {
        player1Object = GameObject.Find("Player1");
        player2Object = GameObject.Find("Player2");
        player1 = player1Object.GetComponent<Player>();
        player2 = player2Object.GetComponent<Player>();
        obelisk = GameObject.Find("Obelisk").GetComponent<Obelisk>();

        gameOverCanvas.enabled = false;
    }

    private void Update()
    {
        if (obelisk.currentHealth <= 0)
        {
            GameOver();
        }

        if (player1 != null)
        {
            if (player1.m_bIsDead)
            {
                StartCoroutine(RespawnPlayer(1));
            }
        }

        if (player2 != null)
        {
            if (player2.m_bIsDead)
            {
                StartCoroutine(RespawnPlayer(2));
            }
        }
    }

    IEnumerator RespawnPlayer(int playerNum)
    {
        if (playerNum == 1)
        {
            Destroy(player1Object);
            player1 = null;
            yield return new WaitForSeconds(respawnRate);
            player1Object = Instantiate(player1Prefab, p1Spawn.position, p1Spawn.rotation);
            player1 = player1Object.GetComponent<Player>();
        }
        else
        {
            Destroy(player2.gameObject);
            player2 = null;
            yield return new WaitForSeconds(respawnRate);
            player2Object = Instantiate(player2Prefab, p2Spawn.position, p2Spawn.rotation);
            player2 = player2Object.GetComponent<Player>();
        }
    }

    private void GameOver()
    {
        gameOverCanvas.enabled = true;
        Time.timeScale = 0;
    }

    public void Button_GameOver_ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}

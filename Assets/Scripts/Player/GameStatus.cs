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
    private AudioSource audioSource;
    private bool hasDied;

    private void Start()
    {
        player1Object = GameObject.Find("Player1");
        player2Object = GameObject.Find("Player2");
        player1 = player1Object.GetComponent<Player>();
        player2 = player2Object.GetComponent<Player>();
        obelisk = GameObject.Find("Obelisk").GetComponent<Obelisk>();
        audioSource = GetComponent<AudioSource>();

        gameOverCanvas.enabled = false;
    }

    private void Update()
    {
        if (obelisk.currentHealth <= 0 && !hasDied)
        {
            GameOver();
            hasDied = true;
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
            player1.enabled = false;
            player1Object.GetComponent<PlayerHealthBar>().enabled = false;
            foreach (SkinnedMeshRenderer mesh in player1Object.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                mesh.enabled = false;
            }
            yield return new WaitForSeconds(respawnRate);
            player1Object.transform.position = p1Spawn.position;
            player1.enabled = true;
            player1.ResetHealth();
            player1.m_bIsDead = false;
            foreach (SkinnedMeshRenderer mesh in player1Object.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                mesh.enabled = true;
            }
            player1Object.GetComponent<PlayerHealthBar>().enabled = true;
        }
        else
        {
            player2.enabled = false;
            player2Object.GetComponent<PlayerHealthBar>().enabled = false;
            foreach (SkinnedMeshRenderer mesh in player2Object.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                mesh.enabled = false;
            }
            yield return new WaitForSeconds(respawnRate);
            player2Object.transform.position = p2Spawn.position;
            player2.enabled = true;
            player2.ResetHealth();
            player2.m_bIsDead = false;
            foreach (SkinnedMeshRenderer mesh in player2Object.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                mesh.enabled = true;
            }
            player2Object.GetComponent<PlayerHealthBar>().enabled = true;
        }
    }

    private void GameOver()
    {
        audioSource.Play();

        gameOverCanvas.enabled = true;
        Time.timeScale = 0;
    }

    public void Button_GameOver_ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}

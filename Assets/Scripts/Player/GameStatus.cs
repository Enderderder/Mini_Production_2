using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStatus : MonoBehaviour {

    [Header("Stats")]
    public float respawnRate = 10;

    [Header("Gameobjects")]
    public Transform p1Spawn;
    public Transform p2Spawn;
    public Canvas gameOverCanvas;
    public Text cointxt;
    public Text turrettxt;

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
    public float respawnplayer1 = 5;
    public float respawnplayer2 = 5;
    public Transform[] playerobjs;
    public Transform[] player2objs;

    public int coin;
    public int turret;

    private void Start()
    {
        player1Object = GameObject.Find("Player1");
        player2Object = GameObject.Find("Player2");
        player1 = player1Object.GetComponent<Player>();
        player2 = player2Object.GetComponent<Player>();
        obelisk = GameObject.Find("Obelisk").GetComponent<Obelisk>();
        audioSource = GetComponent<AudioSource>();
        playerobjs = player1Object.GetComponentsInChildren<Transform>();
        player2objs = player2Object.GetComponentsInChildren<Transform>();

        gameOverCanvas.enabled = false;
    }

    private void Update()
    {
        if (obelisk.currentHealth > 0)
        {
            GameObject.FindGameObjectWithTag("Coin").GetComponent<Text>().text = coin.ToString();
            cointxt.text = coin.ToString();
            turrettxt.text = turret.ToString();
        }

        if (obelisk.currentHealth <= 0 && !hasDied)
        {
            GameOver();
            hasDied = true;
        }

        if (player1 != null)
        {
            if (player1.m_bIsDead)
            {
                DeSpawnPlayer(1);
                respawnplayer1 -= 1 * Time.deltaTime;
                if (respawnplayer1 <= 0)
                {
                RespawnPlayer(1);

                }
            }
        }

        if (player2 != null)
        {
            if (player2.m_bIsDead)
            {
                DeSpawnPlayer(2);
                respawnplayer2 -= 1 * Time.deltaTime;
                if (respawnplayer2 <= 0)
                {
                    RespawnPlayer(2);

                }
            }
        }
    }

    void DeSpawnPlayer(int playerNum)
    {
        if (playerNum == 1)
        {
            
            foreach (Transform objs in playerobjs)
            {
                if (objs.gameObject != player1Object) {
                    objs.gameObject.SetActive(false);
                }
            }

            player1Object.GetComponent<PlayerHealthBar>().SetBarActive(false);

        }
        else if (playerNum == 2)
        {
            foreach (Transform objs in player2objs)
            {
                if (objs.gameObject != player2Object)
                {
                    objs.gameObject.SetActive(false);
                }
            }

            player2Object.GetComponent<PlayerHealthBar>().SetBarActive(false);

        }
    }

    void RespawnPlayer(int playerNum)
    {
        if (playerNum == 1)
        {
            respawnplayer1 = 5;
            player1.ResetHealth();
            player1.m_bIsDead = false;
            player1Object.transform.position = p1Spawn.position;
            foreach (Transform objs in playerobjs)
            {
                    objs.gameObject.SetActive(true);
                
            }

            player1Object.GetComponent<PlayerHealthBar>().SetBarActive(true);

        }
        else if (playerNum == 2)
        {
            respawnplayer2 = 5;
            player2.ResetHealth();
            player2.m_bIsDead = false;
            player2Object.transform.position = p2Spawn.position;
            foreach (Transform objs in player2objs)
            {
                objs.gameObject.SetActive(true);

            }

            player2Object.GetComponent<PlayerHealthBar>().SetBarActive(true);
        }
    }


    //IEnumerator RespawnPlayer(int playerNum)
    //{
    //    if (playerNum == 1)
    //    {
    //        Transform[] playerobjs = player1Object.GetComponentsInChildren<Transform>();

//        player1Object.GetComponent<PlayerHealthBar>().SetBarActive(false);
//        foreach (SkinnedMeshRenderer mesh in player1Object.GetComponentsInChildren<SkinnedMeshRenderer>())
//        {
//            mesh.enabled = false;
//        }
//        yield return new WaitForSeconds(respawnRate);
//        if (player1Object.transform.position != p1Spawn.position)
//        {
//            player1Object.transform.position = p1Spawn.position;
//        }
//        foreach (Transform objs in playerobjs)
//        {
//            objs.gameObject.SetActive(true);
//        }

//        player1.enabled = true;
//        player1Object.SetActive(true);

//        player1.m_bIsDead = false;
//        player1.ResetHealth();
//        foreach (SkinnedMeshRenderer mesh in player1Object.GetComponentsInChildren<SkinnedMeshRenderer>())
//        {
//            mesh.enabled = true;
//        }
//        player1Object.GetComponent<PlayerHealthBar>().SetBarActive(true);
//    }
//    else
//    {
//        player2.enabled = false;
//        player2Object.GetComponent<PlayerHealthBar>().SetBarActive(false);
//        foreach (SkinnedMeshRenderer mesh in player2Object.GetComponentsInChildren<SkinnedMeshRenderer>())
//        {
//            mesh.enabled = false;
//        }
//        yield return new WaitForSeconds(respawnRate);
//        player2Object.transform.position = p2Spawn.position;
//        player2.enabled = true;
//        player2.ResetHealth();
//        player2.m_bIsDead = false;
//        foreach (SkinnedMeshRenderer mesh in player2Object.GetComponentsInChildren<SkinnedMeshRenderer>())
//        {
//            mesh.enabled = true;
//        }
//        player2Object.GetComponent<PlayerHealthBar>().SetBarActive(true);
//    }
//}

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

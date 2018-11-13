using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreIncreaseScript : MonoBehaviour
{
    public int scoreIncreaseAmount = 20;

    private GameController controllerScript;

    void Start()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        controllerScript = controller.GetComponent<GameController>();
        if (null == controllerScript)
        {
            Debug.Log("Can't find controller script - IncreaseScore");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (controllerScript != null)
            {
                controllerScript.IncreaseScore(scoreIncreaseAmount);
            }
            Destroy(gameObject);
        }
    }
}

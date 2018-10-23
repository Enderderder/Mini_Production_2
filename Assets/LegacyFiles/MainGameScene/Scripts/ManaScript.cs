using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaScript : MonoBehaviour
{
    public float manaIncreaseAmount = 5f;

    private Mana manaScript;

    void Start()
    {
        GameObject parentOfPlayers = GameObject.FindGameObjectWithTag("PlayerHolder");
        manaScript = parentOfPlayers.GetComponent<Mana>();
        if (null == manaScript)
        {
            Debug.Log("Can't find mana script - IncreaseMana");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (manaScript != null)
            {
                manaScript.GainMana(manaIncreaseAmount);
            }
            Destroy(gameObject);
        }
    }
}

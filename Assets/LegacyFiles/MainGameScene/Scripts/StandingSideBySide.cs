using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingSideBySide : MonoBehaviour
{
    public float ManaGainAmount = 5.0f;
    public float ManaLoseAmount = 0.1f;
    public int nextUpdate = 1;
    public float manaDecreaseSpeed = 1.0f;

    public ParticleSystem em;
    public ParticleSystem em2;
    private GameObject[] particleSystems;
    private Mana manaScript;
    //private bool isTogether = false;

	void Start()
	{
        GameObject parentOfPlayers = GameObject.FindGameObjectWithTag("PlayerHolder");
        manaScript = parentOfPlayers.GetComponent<Mana>();
        if (null == manaScript)
        {
            Debug.Log("Can't find mana script - Enemy");
        }

        InvokeRepeating("DecreaseMana", 5.0f, manaDecreaseSpeed * 2.0f);
    }

    void DecreaseMana()
    {
        manaScript.LoseMana(ManaLoseAmount);
    }

	void OnTriggerStay(Collider other)
	{
        //if(other.gameObject.name == "Player2")

		if (other.gameObject.name == "Player2" && (Time.time >= nextUpdate)) 
		{
            //One issue = BOTH players gain and lose mana when function is triggered
			if (manaScript != null) {
				manaScript.GainMana (ManaGainAmount);
				//Debug.Log ("Mana Increase" + manaScript.GetMana());
                //isTogether = true;
                em.Play();
                em2.Play();
            } else {
				Debug.Log ("Mana Null - SideBySide");
			}

			nextUpdate = Mathf.FloorToInt (Time.time) + 1;
		}

        if (other.gameObject.name == "Player1" || other.gameObject.name == "Player2")
        {
            //isTogether = true;
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StadingSideBySide : MonoBehaviour 
{
	public float ManaGainAmount = 1.0f;
	public Mana manaScript;
    private ParticleSystem em;
    private GameObject ps;

    private int nextUpdate = 1;

	void Start()
	{
        ps = GameObject.Find("ps");
        em = ps.GetComponent<ParticleSystem>();
        manaScript = GetComponentInParent<Mana> ();
		if (null == manaScript) {
			Debug.Log ("Can't find mana script - side by side");
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player" && (Time.time >= nextUpdate)) 
		{
			if (manaScript != null) {
				manaScript.GainMana (ManaGainAmount);
				Debug.Log ("ManaIncrease");
                em.Play();

            }
            else {
				Debug.Log ("Mana Null");
			}

			nextUpdate = Mathf.FloorToInt (Time.time) + 1;
		}

    }

    void OnTriggerExit()
    {
        //var emission = em.emission;
        //emission.enabled = false;
    }
}

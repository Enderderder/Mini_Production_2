using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageIconsScript : MonoBehaviour {

    public GameObject GainIcon;
    public GameObject AwayIcon;
    public GameObject HurtIcon;
    public GameObject AttackIcon;

    public Mana m_manaScript;
	

	void Update () {
        if(m_manaScript.GetManaState(ManaState.eGAINING))
        {
            GainIcon.SetActive(true);
            AwayIcon.SetActive(false);
        }
        else
        {
            GainIcon.SetActive(false);
            AwayIcon.SetActive(true);
        }

        if (m_manaScript.GetManaState(ManaState.eHURT))
        {
            HurtIcon.SetActive(true);
        }
        else
        {
            HurtIcon.SetActive(false);
        }

        if (m_manaScript.GetManaState(ManaState.eATTACK))
        {
            AttackIcon.SetActive(true);
        }
        else
        {
            AttackIcon.SetActive(false);
        }

    }
}

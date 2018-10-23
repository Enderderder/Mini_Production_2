using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ManaState
{
    eGAINING = 0,
    eHURT,
    eATTACK,
    eAWAY,
}

public class Mana : MonoBehaviour 
{
	public float m_StartingMana = 50f;
    private float m_MaxMana = 100f;
    public Slider m_Slider;
    public float m_ManaAttack = 2.0f;

	private float m_CurrentHealth;
	private bool m_Dead;
    private GameController gameController;
    private AudioSource audioSource;

    //Gaining hurt attack away
    private bool[] MStateArray = {false, false, false, true };

    public bool GetManaState(ManaState _state)
    {
        switch(_state)
        {
            case ManaState.eGAINING:
            {
                return MStateArray[0];
                break;
            }
            case ManaState.eHURT:
            {
                return MStateArray[1];
                break;
            }
            case ManaState.eATTACK:
            {
                return MStateArray[2];
                break;
            }
            case ManaState.eAWAY:
            {
                return MStateArray[3];
                break;
            }
            default:
                return false;
        }
    }

    private void OnEnable()
	{
		m_CurrentHealth = m_StartingMana;
		m_Dead = false;

        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        gameController = gameManager.GetComponent<GameController>();

        audioSource = GetComponent<AudioSource>();

        SetHealthUI ();
	}

	public void TakeDamage(float amount)
	{
        MStateArray[1] = true;
        if (m_CurrentHealth > 0.0f)
        {
            m_CurrentHealth -= amount;
            SetHealthUI();
        }
        else
        {
            m_CurrentHealth = 0.0f;
            if (!m_Dead)
            {
                OnDeath();
            }
        }
    }

	public void GainMana(float amount)
	{
        audioSource.Play();
        //Gaining hurt attack away
        MStateArray[0] = true;
        MStateArray[3] = false;

        if (m_CurrentHealth < m_MaxMana)
        {
            m_CurrentHealth += amount;
            SetHealthUI();
        }
        else
        {
            m_CurrentHealth = 100.0f;
        }
    }

    public void LoseMana(float amount)
    {
        //Gaining hurt attack away
        MStateArray[0] = false;
        MStateArray[3] = true;

        if (m_CurrentHealth > 0.0f)
        {
            m_CurrentHealth -= amount;
            SetHealthUI();
        }
        else
        {
            m_CurrentHealth = 0.0f;
            if (!m_Dead)
            {
                OnDeath();
            }
        }
    }

    public void UseManaAttack(float amount)
    {
        MStateArray[2] = true;

        if (m_CurrentHealth > 0.0f)
        {
            m_CurrentHealth -= amount;
            SetHealthUI();
        }
        else
        {
            MStateArray[2] = false;
            m_CurrentHealth = 0.0f;
            if (!m_Dead)
            {
                OnDeath();
            }
        }
    }

    public float GetMana()
    {
        return m_CurrentHealth;
    }

    public void SetIsNotHurt()
    {
        MStateArray[1] = false;
    }

    public void SetIsNotAttacking()
    {
        MStateArray[2] = false;
    }

    private void SetHealthUI()
	{
        gameController.DisplayMana(m_CurrentHealth);

        m_Slider.value = m_CurrentHealth;
        //Debug.Log("Mana: " + m_CurrentHealth);
	}

	private void OnDeath()
	{
		m_Dead = true;
        gameController.GameOver();
        Destroy(gameObject);

        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject go in enemy)
        {
            EnemyController enemyScript = go.GetComponent<EnemyController>();
            if (null != enemyScript)
            {
                enemyScript.PlayersDied();
            }
            else
            {
                Debug.Log("EnemyScriptNotFound");
            }
        }
    }
}

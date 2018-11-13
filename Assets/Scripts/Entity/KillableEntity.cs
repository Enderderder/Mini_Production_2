using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillableEntity : MonoBehaviour {

    public float currentHealth;
    public float maxHealth;

	void Start ()
    {
        currentHealth = maxHealth;
	}

    void Update ()
    {
		if (currentHealth <= 0)
        {
            Dead();
        }
	}

    public virtual void TakeDamage(float _fDamage)
    {
        currentHealth -= _fDamage;
    }

    public virtual void Dead()
    {

    }
}

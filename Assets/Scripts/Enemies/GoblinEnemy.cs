using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinEnemy : MonoBehaviour {

    [Header("Stats")]
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float attackDamageValue;

    //private GameObject Player;
    private GameObject obelisk;
    private Animator anim;

    private NavMeshAgent navAgent;

    private bool isAttacking = false;
    private Transform Target = null;

    void Start ()
    {
        currentHealth = maxHealth;

        navAgent = GetComponent<NavMeshAgent>();
        obelisk = GameObject.FindGameObjectWithTag("Obelisk");
        anim = GetComponentInChildren<Animator>();

        navAgent.destination = obelisk.transform.position;
	}

	void Update ()
    {
        // If no more health, then die
		if (currentHealth <= 0)
        {
            Death();
        }

        if (navAgent.isStopped)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Obelisk")
        {
            navAgent.isStopped = true;
            isAttacking = true;
            anim.SetTrigger("Attacked");
            Target = other.gameObject.transform;
        }
        else
        {
            navAgent.isStopped = false;
            navAgent.destination = obelisk.transform.position;
            Target = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Obelisk")
        {
            Target = null;
            navAgent.isStopped = false;
        }
    }

    public void TakeDamage(float _fDamage)
    {
        currentHealth -= _fDamage;
    }

    public void Attack()
    {
        if (Target != null)
        {
            if (Target.tag == "Obelisk")
            {
                Target.GetComponent<Obelisk>().TakeDamage(attackDamageValue);
            }
        }
    }

    public void DoneAttack()
    {
        anim.SetBool("Attack", false);
        isAttacking = false;
    }

    private void Death()
    {

    }
}

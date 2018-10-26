using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    [Header("Stats")]
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float attackDamageValue;
    [SerializeField] private float attackDelayValue;

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
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Obelisk")
        {
            navAgent.isStopped = true;

            isAttacking = true;

        }
        else
        {
            navAgent.isStopped = false;
        }

        if (isAttacking)
        {
            anim.SetTrigger("Attacked");
        }
    }

    public void TakeDamage(float _fDamage)
    {
        currentHealth -= _fDamage;
    }

    public void Attack(GameObject _other)
    {
        //yield return new WaitForSeconds(attackSpeed);
        if (_other.tag == "Obelisk")
        {
            _other.GetComponent<Obelisk>().TakeDamage(attackDamageValue);
        }
    }

    public void DoneAttack()
    {
        anim.SetBool("Attack", false);
        isAttacking = false;
    }

    //private IEnumerator Attack(GameObject _other)
    //{
    //    yield return new WaitForSeconds(0.3f);

    //    if (_other.tag == "Obelisk")
    //    {
    //        _other.GetComponent<Obelisk>().TakeDamage(attackDamageValue);
    //    }
    //}

    private void Death()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinEnemy : MonoBehaviour {

    [Header("Stats")]
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth = 20;
    [SerializeField] private float attackDamageValue;
    [SerializeField] private float lookRadius = 10f;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;

    //private GameObject Player;
    private GameObject obelisk;
    private Animator anim;

    private NavMeshAgent navAgent;

    private bool isAttacking = false;
    private Transform Target = null;

    private bool isChasing = false;
    private Transform player1;
    private Transform player2;

    private float obeliskRange;

    void Start ()
    {
        currentHealth = maxHealth;

        player1 = GameObject.Find("Player1").transform;
        player2 = GameObject.Find("Player2").transform;

        navAgent = GetComponent<NavMeshAgent>();
        obelisk = GameObject.FindGameObjectWithTag("Obelisk");
        anim = GetComponentInChildren<Animator>();

        navAgent.destination = obelisk.transform.position;

        obeliskRange = obelisk.GetComponent<Obelisk>().manaRegenRange;

        navAgent.speed = Random.Range(navAgent.speed / 1.5f, navAgent.speed * 1.5f);
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


        float player1Distance = Vector3.Distance(player1.position, transform.position);
        float player2Distance = Vector3.Distance(player2.position, transform.position);

        if (player1Distance <= lookRadius && player2Distance <= lookRadius)
        {
            if (player1Distance >= player2Distance && Vector3.Distance(player2.position, obelisk.transform.position) > obeliskRange)
            {
                navAgent.destination = player2.position;
            }
            else if (Vector3.Distance(player1.position, obelisk.transform.position) > obeliskRange)
            {
                navAgent.destination = player1.position;
            }
            else
            {
                navAgent.destination = obelisk.transform.position;
            }
        }
        else if (player1Distance <= lookRadius && Vector3.Distance(player1.position, obelisk.transform.position) > obeliskRange)
        {
            navAgent.destination = player1.position;
        }
        else if (player2Distance <= lookRadius && Vector3.Distance(player2.position, obelisk.transform.position) > obeliskRange)
        {
            navAgent.destination = player2.position;
        }
        else
        {
            navAgent.destination = obelisk.transform.position;
        }

        Debug.Log(obeliskRange);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Destroy(other.gameObject);
            TakeDamage(5);
        }
        else if (other.tag == "StaticBullet")
        {
            //Destroy(other.gameObject);
            TakeDamage(20);
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
        StartCoroutine(DamageEffect());
    }

    private IEnumerator DamageEffect()
    {
        meshRenderer.material.color = new Color(1, 0, 0);
        yield return new WaitForSeconds(0.1f);
        meshRenderer.material.color = new Color(1, 1, 1);
    }

    public void Attack()
    {
        if (Target != null)
        {
            if (Target.tag == "Obelisk")
            {
                Target.GetComponent<Obelisk>().TakeDamage(attackDamageValue);
            }
            else if (Target.tag == "Player")
            {
                Target.GetComponent<PlayerHeath>().TakeDamage(attackDamageValue);
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
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}

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
    private bool isInAtkRange = false;
    private GameObject Target = null;

    private bool isChasing = false;
    private GameObject player1;
    private GameObject player2;

    private float obeliskRange;

    Coroutine attackAction;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    void Start ()
    {
        currentHealth = maxHealth;

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        
        obelisk = GameObject.FindGameObjectWithTag("Obelisk");

        navAgent.destination = obelisk.transform.position;

        obeliskRange = obelisk.GetComponent<Obelisk>().manaRegenRange;

        // Set the initial target
        Target = obelisk;
        attackAction = StartCoroutine(AttackAction());

        navAgent.speed = Random.Range(navAgent.speed / 1.5f, navAgent.speed * 1.5f);
	}

	void Update ()
    {
        // If no more health, then die
		if (currentHealth <= 0)
        {
            Death();
        }

        //if (navAgent.isStopped)
        //{
        //    anim.SetBool("isWalking", false);
        //}
        //else
        //{
        //    anim.SetBool("isWalking", true);
        //}

        float player1Distance = Vector3.Distance(player1.transform.position, transform.position);
        float player2Distance = Vector3.Distance(player2.transform.position, transform.position);

        if (player1Distance <= lookRadius && player2Distance <= lookRadius)
        {
            if (player1Distance >= player2Distance && Vector3.Distance(player2.transform.position, obelisk.transform.position) > obeliskRange)
            {
                Target = player2;
                //navAgent.destination = player2.transform.position;
            }
            else if (Vector3.Distance(player1.transform.position, obelisk.transform.position) > obeliskRange)
            {
                Target = player1;
                //navAgent.destination = player1.transform.position;
            }
            else
            {
                Target = obelisk;
                //navAgent.destination = obelisk.transform.position;
            }
        }
        else if (player1Distance <= lookRadius && Vector3.Distance(player1.transform.position, obelisk.transform.position) > obeliskRange)
        {
            Target = player1;
            //navAgent.destination = player1.transform.position;
        }
        else if (player2Distance <= lookRadius && Vector3.Distance(player2.transform.position, obelisk.transform.position) > obeliskRange)
        {
            Target = player2;
            //navAgent.destination = player2.transform.position;
        }
        else
        {
            Target = obelisk;
            //navAgent.destination = obelisk.transform.position;
        }

        Debug.Log(obeliskRange);

        // Check if the agent has reach the destination
        if (!navAgent.pathPending)
        {
            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0.0f)
                {
                    anim.SetBool("isWalking", false);
                }
            }
        }
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
        //if (other.tag == "Player" || other.tag == "Obelisk")
        if (Target == other.gameObject)
        {
            //Target = other.gameObject;
            isAttacking = true;
            isInAtkRange = true;
        }
        else
        {
            //navAgent.isStopped = false;
            //navAgent.destination = obelisk.transform.position;
            //Target = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Target)
        {
            isInAtkRange = false;
        }


        if (other.tag == "Player" || other.tag == "Obelisk")
        {
            Target = null;
            //navAgent.isStopped = false;
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
        //anim.SetBool("isAttack", false);
        isAttacking = false;
    }

    private void Death()
    {
        StopCoroutine(attackAction);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }


    private IEnumerator AttackAction()
    {
        while (true)
        {
            anim.SetBool("isWalking", true);
            navAgent.SetDestination(Target.transform.position);

            if (isInAtkRange)
            {
                navAgent.SetDestination(this.transform.position);
                anim.SetTrigger("Attacked");
            }

            yield return null;
        }
    }
}

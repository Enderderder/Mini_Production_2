using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinEnemy : MonoBehaviour, KillableEntity
{

    [Header("Stats")]
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth = 40;
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
    private Player player1Script;
    private Player player2Script;
    private AudioSource audioSource;

    private float obeliskRange;

    Coroutine attackAction;

    private void Awake()
    {
        currentHealth = maxHealth;
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    void Start ()
    {
        currentHealth = maxHealth;


        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        player1Script = player1.GetComponent<Player>();
        player2Script = player2.GetComponent<Player>();
        audioSource = GetComponent<AudioSource>();
        
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
        

        if (currentHealth <= 0)
        {
            Dead();
            return;
        }

        float player1Distance = 10000;
        float player2Distance = 10000;
        float obeliskDistance = Vector3.Distance(obelisk.transform.position, transform.position);

        if (!player1Script.m_bIsDead)
        {
            player1Distance = Vector3.Distance(player1.transform.position, transform.position);
        }
        if (!player2Script.m_bIsDead)
        {
            player2Distance = Vector3.Distance(player2.transform.position, transform.position);
        }


        if (player1Distance < player2Distance && player1Distance < obeliskDistance)
        {
            Target = player1;
        }
        if (player2Distance < player1Distance && player2Distance < obeliskDistance)
        {
            Target = player2;
        }
        if (obeliskDistance <= player1Distance && obeliskDistance <= player2Distance)
        {
            Target = obelisk;
        }

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
            TakeDamage(20);
        }
        else if (other.tag == "StaticBullet")
        {
            TakeDamage(20);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Target == other.gameObject)
        {
            isAttacking = true;
            isInAtkRange = true;


        }
        else
        {
            Target = null;
            isInAtkRange = false;
            isAttacking = false;
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
            isInAtkRange = false;
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
        if (Target != null && isInAtkRange)
        {
            if (Target.tag == "Obelisk")
            {
                Target.GetComponent<Obelisk>().TakeDamage(attackDamageValue);
            }
            else if (Target.tag == "Player")
            {
                Target.GetComponent<Player>().TakeDamage(attackDamageValue);
            }
        }
    }

    public void DoneAttack()
    {
        isAttacking = false;
    }

    public void Dead()
    {
        StopCoroutine(attackAction);
        StartCoroutine(Death());
        Destroy(gameObject);
    }

    private IEnumerator Death()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
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

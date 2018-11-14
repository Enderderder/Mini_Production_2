using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigEnemy : MonoBehaviour, KillableEntity
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float attackDamageValue;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;

    private GameObject obelisk;
    private Animator anim;

    private NavMeshAgent navAgent;

    private bool isAttacking = false;
    private bool isInAttackRange = false;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        currentHealth = maxHealth;

        obelisk = GameObject.FindGameObjectWithTag("Obelisk");

        navAgent.destination = obelisk.transform.position;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Dead();
        }
        else
        {
            if (isInAttackRange)
            {
                anim.SetBool("isWalking", false);
                anim.SetBool("Attacking", true);
                navAgent.isStopped = true;
            }
            else
            {
                anim.SetBool("isWalking", true);
                anim.SetBool("Attacking", false);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!anim.GetBool("IsDead"))
        {
            if (other.tag == "Obelisk")
            {
                isInAttackRange = true;
            }

            if (other.tag == "Bullet")
            {
                Destroy(other.gameObject);
                TakeDamage(20);
            }
            else if (other.tag == "StaticBullet")
            {
                //Destroy(other.gameObject);
                TakeDamage(20);
            }
        }
    }

    public void TakeDamage(float _fDamage)
    {
        currentHealth -= _fDamage;
        StartCoroutine(DamageEffect());
    }

    private IEnumerator DamageEffect()
    {
        foreach (Material mat in meshRenderer.materials)
        {
            mat.color = new Color(1, 0, 0);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (Material mat in meshRenderer.materials)
        {
            mat.color = new Color(1, 1, 1);
        }
    }

    public void Attack()
    {
        obelisk.GetComponent<Obelisk>().TakeDamage(attackDamageValue);
    }

    public void DoneAttack()
    {
        isAttacking = false;
    }

    public void DestroyEntity()
    {
        Destroy(gameObject);
    }

    public void Dead()
    {
        navAgent.isStopped = true;
        anim.SetBool("IsDead", true);
    }
}

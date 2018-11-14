using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSpell : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float m_SpellDamage = 1.0f;
    [SerializeField] protected float m_SpellLifeTime = 3.0f;
    [SerializeField] protected float m_SpellSpeed;
    [SerializeField] protected float m_BigEnemyDmgMulplier = 1.25f;

    // Component
    private ParticleSystem[] m_particles;


    virtual protected void Start()
    {
        m_particles = GetComponentsInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        KillableEntity enemy = other.gameObject.GetComponent<KillableEntity>();
        if (enemy)
        {
            if (enemy.gameObject.tag == "BigEnemy")
            {
                enemy.TakeDamage(m_SpellDamage * m_BigEnemyDmgMulplier);
            }

            
        }

    }

    protected virtual void DamageEffectOnEnter()
    {

    }

    protected virtual void DamageEffectOnStay()
    {

    }

    protected virtual void DamageEffectOnExit()
    {

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpecialSpell : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float m_SpellDamage = 1.0f;
    [SerializeField] protected float m_SpellLifeTime = 3.0f;
    [SerializeField] protected float m_SpellSpeed = 1.0f;
    [SerializeField] protected float m_BigEnemyDmgMulplier = 1.25f;
    [SerializeField] protected float m_SpawnOffsetHeight = 0.2f;

    // Component
    protected ParticleSystem[] m_particles;
    protected Rigidbody m_rigidBody;

    virtual protected void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_particles = GetComponentsInChildren<ParticleSystem>();
        StartCoroutine(LifeTimeCountdown());
    }

    private void OnTriggerEnter(Collider other)
    {
        DamageEffectOnEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        DamageEffectOnExit(other);
    }

    private void OnTriggerStay(Collider other)
    {
        DamageEffectOnStay(other);
    }

    private IEnumerator LifeTimeCountdown()
    {
        yield return new WaitForSeconds(m_SpellLifeTime);

        StopAllCoroutines();
        Destroy(this.gameObject);
    }

    protected virtual void DamageEffectOnEnter(Collider other)
    {
    }

    protected virtual void DamageEffectOnStay(Collider other)
    {
    }

    protected virtual void DamageEffectOnExit(Collider other)
    {
    }


}

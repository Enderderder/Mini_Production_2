using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class RegularSpell : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float m_SpellDamage = 7.0f;
    [SerializeField] private float m_SpellSpeed = 7.0f;
    [SerializeField] private float m_SpellLifeTime = 3.0f;

    [SerializeField] private GameObject m_HitEffectPrefab;

    // References
    private ParticleSystem[] m_particles;
    private MeshRenderer m_meshRenderer;
    private Rigidbody m_rigidBody;

    // Flag
    private bool m_isDestroying = false;

    private void Awake()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_rigidBody = GetComponent<Rigidbody>();
        m_particles = GetComponentsInChildren<ParticleSystem>();
    }

    private void Start ()
    {
        // Startup all the particles
        foreach (var particle in m_particles)
        {
            var particleMainModule = particle.main;
            particleMainModule.loop = true;
            particle.Play();
        }

        // Speed up!
        m_rigidBody.velocity =
            this.transform.forward * m_SpellSpeed;

        // Start the life time countdown
        StartCoroutine(LifeTimeCountdown());
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the game object belongs to KillableEntity
        // Deal damage if it is
        KillableEntity killableEntity = other.gameObject.GetComponent<KillableEntity>();
        if (killableEntity)
        {
            killableEntity.TakeDamage(m_SpellDamage);
        }

        // Destroy the projectile on any contact
        StartCoroutine(DestroySpell());

        // Spawn a quick hit effect on to the thing
        Instantiate(m_HitEffectPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);

        //
        //if (other.tag == "Enemy")
        //{
        //    other.GetComponent<GoblinEnemy>().TakeDamage(m_SpellDamage);
        //}
        //else if (other.tag == "BigEnemy")
        //{
        //    other.GetComponent<BigEnemy>().TakeDamage(m_SpellDamage);
        //}
    }

    private IEnumerator LifeTimeCountdown()
    {
        yield return new WaitForSeconds(m_SpellLifeTime);

        StartCoroutine(DestroySpell());
    }

    private IEnumerator DestroySpell()
    {
        if (!m_isDestroying)
        {
            m_isDestroying = true;

            StartCoroutine(TrailShrink());
            // Stop and disable the ball
            m_rigidBody.velocity = Vector3.zero;
            m_meshRenderer.enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            yield return new WaitForSeconds(1.0f);

            StopAllCoroutines();
            Destroy(this.gameObject);
        }
    }

    private IEnumerator TrailShrink()
    {
        float shrinkMultiplier = 0.05f;

        while (true)
        {
            foreach (var particle in m_particles)
            {
                var particleMainModule = particle.main;

                if (particleMainModule.startLifetime.constant > 0.0f)
                {
                    float currentLifeTime = particleMainModule.startLifetime.constant;
                    float resultLifeTime = currentLifeTime - (1.0f - shrinkMultiplier);

                    particleMainModule.startLifetime = resultLifeTime;
                }
            }

            yield return null;
        }
    }
}

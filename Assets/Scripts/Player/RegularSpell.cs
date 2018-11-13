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
        // Destroy the projectile on any contact
        StartCoroutine(DestroySpell());
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

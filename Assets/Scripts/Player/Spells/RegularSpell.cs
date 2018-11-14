using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    [SerializeField] private ElementType m_element;
    // References
    private ParticleSystem[] m_particles;
    private MeshRenderer m_meshRenderer;
    private Rigidbody m_rigidBody;
    [SerializeField] private GameObject damagetxt;
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
            GameObject dmgobject = Instantiate(damagetxt, new Vector3(this.transform.position.x, this.transform.position.y + 2, this.transform.position.z), Quaternion.Euler(0, 45, 0));
            dmgobject.GetComponentInChildren<TextMeshPro>().text = "-" + m_SpellDamage;

            if (m_element == ElementType.Water) {
                dmgobject.GetComponentInChildren<TextMeshPro>().color = new Color(0.5F, 1.0F, 1.0F);
            }
            else if (m_element == ElementType.Fire)
            {
                dmgobject.GetComponentInChildren<TextMeshPro>().color = new Color(1.0F, 0.64F, 0.16F);
            }
            else if (m_element == ElementType.Earth)
            {
                dmgobject.GetComponentInChildren<TextMeshPro>().color = Color.gray;
            }
            else if (m_element == ElementType.Air)
            {
                dmgobject.GetComponentInChildren<TextMeshPro>().color = Color.white;
            }
            Destroy(dmgobject, 1);
        }

        // Destroy the projectile on any contact
        StartCoroutine(DestroySpell());
        Instantiate(m_HitEffectPrefab, this.gameObject.transform.position, Quaternion.identity);

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class bullet : MonoBehaviour {
    public Transform target;
    [SerializeField] private float m_SpellDamage = 7.0f;
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

    private void Start()
    {
        // Startup all the particles
        foreach (var particle in m_particles)
        {
            var particleMainModule = particle.main;
            particleMainModule.loop = true;
            particle.Play();
        }

    }

    private void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, target.position, 5 * Time.deltaTime);
        Vector3 direction = target.position - this.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        this.transform.rotation = rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the game object belongs to KillableEntity
        // Deal damage if it is
        KillableEntity killableEntity = other.gameObject.GetComponent<KillableEntity>();
        if (killableEntity != null)
        {
            killableEntity.TakeDamage(m_SpellDamage);
            GameObject dmgobject =
                Instantiate(damagetxt, new Vector3(this.transform.position.x, this.transform.position.y + 2, this.transform.position.z), Quaternion.Euler(0, 45, 0));
            dmgobject.GetComponentInChildren<TextMeshPro>().text = "-" + m_SpellDamage;

            dmgobject.GetComponentInChildren<TextMeshPro>().color = new Color(1.0F, 0.64F, 0.16F);


            Destroy(dmgobject, 1);
        }
        Destroy(this.gameObject, 1);
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

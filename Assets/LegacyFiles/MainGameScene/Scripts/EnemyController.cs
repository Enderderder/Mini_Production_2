using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public AudioClip moving;
    public AudioClip attacking;
    public AudioSource hurt;

    public float lookRadius = 50.0f;
    public float startingHealth = 20.0f;
    public float damage = 5.0f;
    public int enemyWorth = 10;

    private bool m_bHitPlayer = false;

    Transform target;

    NavMeshAgent agent;

    private Mana manaScript;
    private GameController scoreScript;
    private AudioSource audioSource;
    private float currentHealth;
    private float nextUpdate;

    private ParticleSystem em;
    private bool isDead;
    //private bool playerDead;


    // Use this for initialization
    void Start() {

        //playertoTarget = Random.Range(0, 1);
        var possibleTargets = GameObject.FindGameObjectsWithTag("Player");

        if (0 != possibleTargets.Length)
        {
            target = possibleTargets[Random.Range(0, possibleTargets.Length)].transform;
        }

        agent = GetComponent<NavMeshAgent>();
        
        GameObject parentOfPlayers = GameObject.FindGameObjectWithTag("PlayerHolder");
        if (null != parentOfPlayers)
        {
            manaScript = parentOfPlayers.GetComponent<Mana>();
        }
        if (null == manaScript)
        {
            Debug.Log("Can't find mana script - Enemy");
        }
        
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        scoreScript = gameManager.GetComponent <GameController>();

        currentHealth = startingHealth;

        audioSource = GetComponent<AudioSource>();

        nextUpdate = 0;
        isDead = false;
        //playerDead = false;

        em = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update() {

        if(!m_bHitPlayer)
        {
            if(manaScript != null)
                manaScript.SetIsNotHurt();
        }

        if (target != null)
        {
            float distance = Vector3.Distance(target.position, transform.position);

            if (distance <= lookRadius)
            {
                agent.SetDestination(target.position);

                if (distance <= agent.stoppingDistance)
                {
                    //attack target
                    //face target
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
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
        if (other.tag == "Player")
        {
            if (nextUpdate < Time.time)
            {
                //Debug.Log("Decrease Mana from Enemy");
                audioSource.clip = attacking;
                audioSource.Play();
                manaScript.TakeDamage(damage);
                m_bHitPlayer = true;
                nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            audioSource.clip = moving;
            audioSource.Play();
            m_bHitPlayer = false;
        }
    }

    private void TakeDamage(int amount)
    {
        currentHealth -= amount;
        hurt.Play();

        if (null != em)
        {
            em.Play();
        }
        else
        {
            Debug.Log("Can't find em EnemyController");
        }

        Debug.Log("Enemy health: " + currentHealth);
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            scoreScript.IncreaseScore(enemyWorth);
            Destroy(gameObject);
        }
    }

    public void PlayersDied()
    {
        //playerDead = true;
    }
}

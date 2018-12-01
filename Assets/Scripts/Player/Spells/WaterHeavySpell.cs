using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class WaterHeavySpell : SpecialSpell
{
    [SerializeField] private float m_DamageTick = 0.5f;
    [SerializeField] private float m_HealingPerTick = 2.0f;
    [SerializeField] private float m_SlowMultiplier = 0.8f;
    [SerializeField] private GameObject damagetxt;
    private List<KillableEntity> m_inRangeEntity;
    private List<Player> m_inRangePlayer;
    private Coroutine m_unleashDmgCoroutine;

    private void Awake()
    {
        m_inRangeEntity = new List<KillableEntity>();
        m_inRangePlayer = new List<Player>();
    }

    protected override void Start()
    {
        base.Start();

        this.gameObject.transform.position += new Vector3(0.0f, m_SpawnOffsetHeight, 0.0f);
        m_rigidBody.velocity = transform.forward * m_SpellSpeed;
        m_unleashDmgCoroutine = StartCoroutine(UnleashDamage());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        foreach (var enemy in m_inRangeEntity)
        {
            if (((MonoBehaviour)enemy))
            {
                ((MonoBehaviour)enemy).GetComponent<NavMeshAgent>().speed /= m_SlowMultiplier;
            }
        }
        StopAllCoroutines();
    }

    protected override void DamageEffectOnEnter(Collider other)
    {
        base.DamageEffectOnEnter(other);
        

        KillableEntity enemy = other.gameObject.GetComponent<KillableEntity>();
        if (enemy != null && !m_inRangeEntity.Contains(enemy))
        {
            m_inRangeEntity.Add(enemy);
            ((MonoBehaviour)enemy).GetComponent<NavMeshAgent>().speed *= m_SlowMultiplier;
        }
        else
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null && !m_inRangePlayer.Contains(player))
            {
                m_inRangePlayer.Add(player);
            }
        }
    }

    protected override void DamageEffectOnStay(Collider other)
    {
        base.DamageEffectOnStay(other);
    }

    protected override void DamageEffectOnExit(Collider other)
    {
        base.DamageEffectOnExit(other);

        KillableEntity enemy = other.gameObject.GetComponent<KillableEntity>();
        if (enemy != null && m_inRangeEntity.Contains(enemy))
        {
            m_inRangeEntity.Remove(enemy);
            ((MonoBehaviour)enemy).GetComponent<NavMeshAgent>().speed /= m_SlowMultiplier;
        }
        else
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null && m_inRangePlayer.Contains(player))
            {
                m_inRangePlayer.Remove(player);
            }
        }

    }

    private IEnumerator UnleashDamage()
    {
        while (true)
        {
            foreach (var enemy in m_inRangeEntity)
            {
                if (((MonoBehaviour)enemy))
                {
                    if (((MonoBehaviour)enemy).gameObject.tag == "BigEnemy")
                    {
                        enemy.TakeDamage(m_SpellDamage * m_BigEnemyDmgMulplier);
                        GameObject dmgobject =
                Instantiate(damagetxt, new Vector3(this.transform.position.x, this.transform.position.y + 2, this.transform.position.z), Quaternion.Euler(0, 45, 0));
                        dmgobject.GetComponentInChildren<TextMeshPro>().text = "-" + m_SpellDamage * m_BigEnemyDmgMulplier;
                        dmgobject.GetComponentInChildren<TextMeshPro>().color = new Color(0.5F, 1.0F, 1.0F);
                    }
                    else
                    {
                        enemy.TakeDamage(m_SpellDamage);
                        GameObject dmgobject =
                Instantiate(damagetxt, new Vector3(this.transform.position.x, this.transform.position.y + 2, this.transform.position.z), Quaternion.Euler(0, 45, 0));
                        dmgobject.GetComponentInChildren<TextMeshPro>().text = "-" + m_SpellDamage;
                        dmgobject.GetComponentInChildren<TextMeshPro>().color = new Color(0.5F, 1.0F, 1.0F);
                    }
                }
            }

            foreach (var player in m_inRangePlayer)
            {
                if (player)
                {
                    player.Heal(m_HealingPerTick);
                }
            }

            yield return new WaitForSeconds(m_DamageTick);
        }
    }
}

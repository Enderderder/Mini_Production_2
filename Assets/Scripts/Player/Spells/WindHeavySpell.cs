using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WindHeavySpell : SpecialSpell
{
    [SerializeField] private float m_DamageTick = 0.5f;
    [SerializeField] private GameObject damagetxt;
    private List<KillableEntity> m_inRangeEntity;
    private Coroutine m_unleashDmgCoroutine;

    protected override void Start()
    {
        base.Start();

        m_inRangeEntity = new List<KillableEntity>();
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
        StopAllCoroutines();
    }

    protected override void DamageEffectOnEnter(Collider other)
    {
        base.DamageEffectOnEnter(other);

        KillableEntity enemy = other.gameObject.GetComponent<KillableEntity>();
        if (enemy != null && !m_inRangeEntity.Contains(enemy))
        {
            m_inRangeEntity.Add(enemy);
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
                        GameObject dmgobject = Instantiate(damagetxt, new Vector3(this.transform.position.x, this.transform.position.y + 2, this.transform.position.z), Quaternion.Euler(0, 45, 0));
                        dmgobject.GetComponentInChildren<TextMeshPro>().text = "-" + m_SpellDamage * m_BigEnemyDmgMulplier;
                        dmgobject.GetComponentInChildren<TextMeshPro>().color = Color.white;

                        ((MonoBehaviour)enemy).GetComponent<Rigidbody>().velocity = Vector3.zero;
                        Vector3 pushDirection = transform.position - ((MonoBehaviour)enemy).transform.position;
                        pushDirection = pushDirection.normalized;
                        ((MonoBehaviour)enemy).transform.GetComponent<Rigidbody>().AddForce(pushDirection * 1 * 800);
                    }
                    else
                    {
                        enemy.TakeDamage(m_SpellDamage);
                        GameObject dmgobject = Instantiate(damagetxt, new Vector3(this.transform.position.x, this.transform.position.y + 2, this.transform.position.z), Quaternion.Euler(0, 45, 0));
                        dmgobject.GetComponentInChildren<TextMeshPro>().text = "-" + m_SpellDamage;
                        dmgobject.GetComponentInChildren<TextMeshPro>().color = Color.white;
                        ((MonoBehaviour)enemy).GetComponent<GoblinEnemy>().canwalk = false;
                        ((MonoBehaviour)enemy).GetComponent<Rigidbody>().velocity = Vector3.zero;
                        Vector3 pushDirection = transform.position - ((MonoBehaviour)enemy).transform.position;
                        pushDirection = pushDirection.normalized;
                        ((MonoBehaviour)enemy).transform.GetComponent<Rigidbody>().AddForce(pushDirection * 1 * 800);
                    }

                    
                }
            }

            yield return new WaitForSeconds(m_DamageTick);
        }
    }
}

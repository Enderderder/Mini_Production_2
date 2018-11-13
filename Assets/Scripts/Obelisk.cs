using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Obelisk : MonoBehaviour {

    public float currentHealth;
    [SerializeField] private float maxHealth = 1000;
    public float manaRegenRange;

    public MeshRenderer[] meshRenderers;

    private void Start()
    {
        // Set health to max
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // If no more health, then gameover
        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    public void TakeDamage(float _fDamage)
    {
        currentHealth -= _fDamage;
        StartCoroutine(DamageEffect());
    }

    private IEnumerator DamageEffect()
    {
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material.color = new Color(1, 0, 0);
        }

        yield return new WaitForSeconds(0.1f);

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material.color = new Color(1, 1, 1);
        }
    }

    private void GameOver()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, manaRegenRange / 6);
    }
}

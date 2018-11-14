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
        UpdateUI();
    }

    private void Update()
    {
        // If no more health, then gameover
        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    private void UpdateUI()
    {
        // Update the UI health bar with percentage
        // becauze UI image fill use 0 - 1 value
        this.GetComponent<ObeliskHealthBar>().ChangeHealth(currentHealth / maxHealth);
    }

    public void TakeDamage(float _fDamage)
    {
        currentHealth -= _fDamage;
        StartCoroutine(DamageEffect());
        UpdateUI();
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

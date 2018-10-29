using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHeath : MonoBehaviour {

    [Header("Stats")]
    public float currentHealth;
    public float maxHealth = 100f;
    public float currentMana;
    public float maxMana = 100f;

    [Header("Generate Mana")]
    public float manaRegenAmount = 10;
    public float manaRegenDelay = 1;

    [Header("UI Text")]
    public Text healthText; // TEMP until healthbars
    public Text manaText; // TEMP until manabars

    private Transform obelisk;
    private Obelisk obeliskScript;

    private bool canGenerateMana = true;

    private void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        obelisk = GameObject.FindGameObjectWithTag("Obelisk").transform;
        obeliskScript = obelisk.GetComponent<Obelisk>();
    }

    private void Update()
    {
        healthText.text = "Health: " + currentHealth;
        manaText.text = "Mana: " + currentMana;

        if (currentHealth <= 0)
        {
            Death();
        }

        if (Vector3.Distance(obelisk.position, transform.position) <= obeliskScript.manaRegenRange && canGenerateMana && currentMana < maxMana)
        {
            StartCoroutine(GenerateMana());
        }

        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
    }

    IEnumerator GenerateMana()
    {
        canGenerateMana = false;
        yield return new WaitForSeconds(manaRegenDelay);
        currentMana += manaRegenAmount;
        canGenerateMana = true;
    }

    public void TakeDamage(float damageVal)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageVal;
            StartCoroutine(DamageEffect());
        }
    }

    private IEnumerator DamageEffect()
    {
        MeshRenderer[] meshes = gameObject.transform.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material.color = new Color(1, 0, 0);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material.color = new Color(1, 1, 1);
        }
    }

    public void UseMana(float value)
    {
        if (currentMana > 0)
        {
            currentMana -= value;
        }
    }

    private void Death()
    {

    }
}

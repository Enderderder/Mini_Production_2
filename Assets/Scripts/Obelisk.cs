using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Obelisk : MonoBehaviour {

    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth = 1000;
    [SerializeField] private float manaRegenRange;

    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        // Set health to max
        currentHealth = maxHealth;

        DrawCircleRange(manaRegenRange);
    }

    private void Update()
    {
        // If no more health, then gameover
        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    private void DrawCircleRange(float _range)
    {
        LineRenderer rangeCircle = GetComponent<LineRenderer>();

        rangeCircle.positionCount = 50 + 1;
        rangeCircle.useWorldSpace = false;

        float x;
        float z;

        float angle = 20f;

        for (int i = 0; i < (50 + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * _range;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * _range;

            rangeCircle.SetPosition(i, new Vector3(x, 0, z));

            angle += (360f / 50);
        }
    }

    public void TakeDamage(float _fDamage)
    {
        currentHealth -= _fDamage;
        StartCoroutine(DamageEffect());
    }

    private IEnumerator DamageEffect()
    {
        meshRenderer.material.color = new Color(1, 0, 0);
        yield return new WaitForSeconds(0.1f);
        meshRenderer.material.color = new Color(1, 1, 1);
    }

    private void GameOver()
    {

    }
}

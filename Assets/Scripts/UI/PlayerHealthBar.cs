﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private GameObject HealthBarPrefab;
    [SerializeField] private Vector3 Offeset;

    private Image m_background;
    private Image m_healthFill;
    private Image m_manaFill;
    private RawImage m_elementIcon;
    private Image m_entershop;

    private void Awake()
    {
        // Instantiate the UI and make sure the reference
        m_background =
            Instantiate(HealthBarPrefab, GameObject.Find("/HealthUI_Global").transform).GetComponent<Image>();

        Transform healthBarTransform = m_background.transform;
        m_healthFill = healthBarTransform.Find("HealthFill").GetComponent<Image>();
        m_manaFill = healthBarTransform.Find("ManaFill").GetComponent<Image>();
        m_elementIcon = healthBarTransform.Find("ElementIcon").GetComponent<RawImage>();
        m_entershop = healthBarTransform.Find("EnterShop").GetComponent<Image>();
        m_entershop.enabled = false;
    }

    private void Update()
    {
        m_background.transform.position
            = Camera.main.WorldToScreenPoint(this.gameObject.transform.position + Offeset);
    }

    public void ChangeHealth(float _value)
    {
        if (m_healthFill)
        {
            m_healthFill.fillAmount = _value;
        }
    }

    public void SetBarActive(bool _bar)
    {
        m_background.gameObject.SetActive(_bar);
    }

    public void ChangeMana(float _value)
    {
        if (m_manaFill)
        {
            m_manaFill.fillAmount = _value;
        }
    }

    public void SwapElementIcon(Texture _icon)
    {
        if (m_elementIcon)
        {
            m_elementIcon.texture = _icon;
        }
    }

    public void EnableShop(bool _enable)
    {
        m_entershop.enabled = _enable;
    }

    public bool getenableShop()
    {
        return m_entershop.enabled;
    }
}

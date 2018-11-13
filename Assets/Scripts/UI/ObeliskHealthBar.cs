using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObeliskHealthBar : MonoBehaviour {

    [SerializeField] private GameObject HealthBarPrefab;
    [SerializeField] private Vector3 Offeset;
    [SerializeField] private Text damagetxt;
    private Image m_background;
    private Image m_healthFill;


    private void Start()
    {
        m_background =
            Instantiate(HealthBarPrefab, GameObject.Find("/HealthUI_Global").transform).GetComponent<Image>();

        Transform healthBarTransform = m_background.transform;
        m_healthFill = healthBarTransform.Find("HealthFill").GetComponent<Image>();
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
}

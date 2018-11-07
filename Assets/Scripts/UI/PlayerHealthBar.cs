using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private readonly GameObject HealthBarPrefab;
    [SerializeField] private Vector3 Offeset;

   private Image m_background;
    private Image m_healthFill;
    private Image m_manaFill;

    private void Start()
    {
        m_background =
            Instantiate(HealthBarPrefab, GameObject.Find("/HealthUI_Global").transform).GetComponent<Image>();

        Transform healthBarTransform = m_background.transform;
        m_healthFill = healthBarTransform.Find("HealthFill").GetComponent<Image>();
        m_manaFill = healthBarTransform.Find("ManaFill").GetComponent<Image>();
    }

    private void Update()
    {
        m_background.transform.position
            = Camera.current.WorldToScreenPoint(this.gameObject.transform.position + Offeset);
    }
}

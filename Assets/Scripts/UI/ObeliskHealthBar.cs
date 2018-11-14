using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObeliskHealthBar : MonoBehaviour {

    [SerializeField] private GameObject HealthBarPrefab;
    [SerializeField] private Vector3 Offeset;
    [SerializeField] private GameObject damagetxt;
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
            GameObject dmgobject = Instantiate(damagetxt, new Vector3 (this.transform.position.x, this.transform.position.y + 9, this.transform.position.z), Quaternion.Euler(0, 45, 0));
            dmgobject.GetComponentInChildren<TextMeshPro>().text = "-" + _value * 100;
            Destroy(dmgobject, 1);

        }

    }
}

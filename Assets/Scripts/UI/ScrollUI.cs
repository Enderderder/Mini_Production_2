using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollUI : MonoBehaviour {
    [SerializeField] private GameObject Textprefab;
    [SerializeField] private Vector3 Offeset;
    [SerializeField] private ElementType element;
    [SerializeField] private GameObject button;
    private Image buttonImage;
    private Text Info;
    // Use this for initialization
    void Start () {
        buttonImage = Instantiate(button, GameObject.Find("/HealthUI_Global").transform).GetComponent<Image>();
        buttonImage.enabled = false;
        Info = Instantiate(Textprefab, GameObject.Find("/HealthUI_Global").transform).GetComponent<Text>();
        if (element == ElementType.Earth)
        {
            Info.text = "Earth Element";
            Info.color = Color.green;
        }
        else if (element == ElementType.Air)
        {
            Info.text = "Air Element";
            Info.color = Color.white;
        }
        else if (element == ElementType.Fire)
        {
            Info.text = "Fire Element";
            Info.color = Color.red;
        }
        else if (element == ElementType.Water)
        {
            Info.text = "Water Element";
            Info.color = Color.blue;
        }
    }
	
	// Update is called once per frame
	void Update () {
        buttonImage.transform.position
            = Camera.main.WorldToScreenPoint(this.gameObject.transform.position + Offeset + new Vector3(0,3,0));
        Info.transform.position = Camera.main.WorldToScreenPoint(this.gameObject.transform.position + Offeset);
        
    }

    public void SetElement(ElementType ele)
    {
        element = ele;
        if (element == ElementType.Earth)
        {
            Info.text = "Earth Element";
            Info.color = Color.green;
        }
        else if (element == ElementType.Air)
        {
            Info.text = "Air Element";
            Info.color = Color.white;
        }
        else if (element == ElementType.Fire)
        {
            Info.text = "Fire Element";
            Info.color = Color.red;
        }
        else if (element == ElementType.Water)
        {
            Info.text = "Water Element";
            Info.color = Color.blue;
        }
    }

    public ElementType GetElement()
    {
        return element;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            buttonImage.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        buttonImage.enabled = false;
    }
}

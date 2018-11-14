using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollUI : MonoBehaviour {
    [SerializeField] private GameObject Textprefab;
    [SerializeField] private Vector3 Offeset;
    [SerializeField] private ElementType element;
    [SerializeField] private GameObject button;
    [SerializeField] private Animator anim;
    public GameObject scrollobject;
    public SkinnedMeshRenderer skinrender;
    public Material firescrollmaterial;
    public Material waterscrollmaterial;
    public Material earthscrollmaterial;
    public Material airscrollmaterial;
    private Image buttonImage;
    public Text Info;
    // Use this for initialization
    void Start () {
        anim = this.GetComponent<Animator>();
        buttonImage = Instantiate(button, GameObject.Find("/HealthUI_Global").transform).GetComponent<Image>();
        buttonImage.enabled = false;
        Info = Instantiate(Textprefab, GameObject.Find("/HealthUI_Global").transform).GetComponent<Text>();

        skinrender = scrollobject.GetComponent<SkinnedMeshRenderer>();

        if (element == ElementType.Earth)
        {
            Info.text = "Earth Element";
            Info.color = Color.green;
            skinrender.material = earthscrollmaterial;
        }
        else if (element == ElementType.Air)
        {
            Info.text = "Air Element";
            Info.color = Color.white;
            skinrender.material = airscrollmaterial;
        }
        else if (element == ElementType.Fire)
        {
            Info.text = "Fire Element";
            Info.color = Color.red;
            skinrender.material = firescrollmaterial;
        }
        else if (element == ElementType.Water)
        {
            Info.text = "Water Element";
            Info.color = Color.blue;
            skinrender.material = waterscrollmaterial;
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
            skinrender.material = earthscrollmaterial;
        }
        else if (element == ElementType.Air)
        {
            Info.text = "Air Element";
            Info.color = Color.white;
            skinrender.material = airscrollmaterial;
        }
        else if (element == ElementType.Fire)
        {
            Info.text = "Fire Element";
            Info.color = Color.red;
            skinrender.material = firescrollmaterial;
        }
        else if (element == ElementType.Water)
        {
            Info.text = "Water Element";
            Info.color = Color.blue;
            skinrender.material = waterscrollmaterial;
        }
    }

    public ElementType GetElement()
    {
        return element;
    }

    public void DestroySelf()
    {
        Destroy(Info);
        Destroy(buttonImage);
        Destroy(this.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player)
        {
            buttonImage.enabled = true;
            if (player.m_controlPickScroll.IsPressed)
            {
                anim.SetTrigger("Unfiold");
                player.ChangeElement(element);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        buttonImage.enabled = false;
    }
}

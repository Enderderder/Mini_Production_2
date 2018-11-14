using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour {

    private Text damage;
    public GameObject objectref;
	// Use this for initialization
	void Start () {
        damage = this.GetComponent<Text>();
        
    }

    public void setcurrentobj(GameObject _ref)
    {
        objectref = _ref;
    }
	// Update is called once per frame
	void Update () {
        
        this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + 1.0f, this.transform.position.z);
        
    }

}

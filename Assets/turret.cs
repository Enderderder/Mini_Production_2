using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret : MonoBehaviour {
    public GameObject bullet;
    private int count;
    public float firerate;
    private float m_firerate;
    public Transform firepoint;
	// Use this for initialization
	void Start () {
        count = 20;
        m_firerate = firerate;
	}
	
	// Update is called once per frame
	void Update () {
        firerate -= 1 * Time.deltaTime;

        if (count <= 0)
        {
            Destroy(this.gameObject);
        }
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy" && firerate <= 0 || other.tag == "BigEnemy" && firerate <= 0)
        {
            firerate = m_firerate;
           GameObject fire = Instantiate(bullet, firepoint.position, firepoint.rotation) as GameObject;
            fire.GetComponent<bullet>().target = other.transform;
            count--;
        }
    }
}

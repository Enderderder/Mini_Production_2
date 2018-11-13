using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.ParticleSystem;

public class ParticleActivate : MonoBehaviour
{
    public GameObject[] particles;


    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            foreach (GameObject particle in particles)
            {
                particle.GetComponent<ParticleSystem>().Play();
            }
        }
    }
}

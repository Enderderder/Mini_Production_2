using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnExplosion : MonoBehaviour
{
    public GameObject ParticleHolder;

    private ParticleSystem em;

    private void Start()
    {
        em = ParticleHolder.GetComponentInChildren<ParticleSystem>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            em.Play();
            Debug.Log("Hit");
        }
    }
}

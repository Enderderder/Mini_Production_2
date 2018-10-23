using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pinkMovement : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            anim.SetBool("isAttacking", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            anim.SetBool("isAttacking", false);
        }
    }
}

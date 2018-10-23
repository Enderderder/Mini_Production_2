using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pinkAttack : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        bool bIsAttacking = false;

        if(other.tag == "Player")
        {
            bIsAttacking = true;
        }

        anim.SetBool("isAttacking", bIsAttacking);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            bool bIsAttacking = false;
            anim.SetBool("isAttacking", bIsAttacking);
        }
    }
}

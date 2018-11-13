using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErinAnimationScript : MonoBehaviour {

    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            anim.CrossFade("ErinAttack2", 0.1f);
        }

        if (Input.GetKeyDown(KeyCode.W)||
            Input.GetKeyDown(KeyCode.A)||
            Input.GetKeyDown(KeyCode.S)||
            Input.GetKeyDown(KeyCode.D))
        {
            anim.CrossFade("ErinWalkingCycle", 0.1f);
        }

        if (Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D))
        {
            anim.SetBool("isWalking", true);
            anim.SetBool("isIdle", false);
        }
        else
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isIdle", true);
        }
    }
}

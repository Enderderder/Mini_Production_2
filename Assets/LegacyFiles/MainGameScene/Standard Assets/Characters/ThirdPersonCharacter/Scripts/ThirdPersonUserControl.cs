using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
        private List<int> controllerid;
        private float h = 0, v = 0;
        private KeyCode keyfire;
        

        public Mana manaScript;

        //Public variables
        public GameObject bulletPrefab;
        public Transform bulletSpawn;
        public int PlayerID = 0;

        private void Start()
        {
            
            controllerid = new List<int>();
            m_Character = GetComponent<ThirdPersonCharacter>();

            if (Input.GetJoystickNames().Length > 0) {
                for (int i = 0; i < Input.GetJoystickNames().Length; i++)
                {
                    if (Input.GetJoystickNames()[i] != "")
                    {
                        controllerid.Add(i + 1);
                    }
                }
            }

        }

        

        private void Update()
        {
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                    Debug.Log("KeyCode down: " + kcode);
            }

            if (PlayerID == 1) {

                if (controllerid.Count > 0) {
                    if (Input.GetKeyDown("joystick " + controllerid[0] + " button 0"))
                    {
                        Fire();
                        manaScript.UseManaAttack(2.0f);
                        m_Character.AnimAttack(true);
                    }
                    else if (Input.GetKeyUp("joystick " + controllerid[0] + " button 0"))
                    {
                        m_Character.StopAttack(true);
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.LeftControl))
                    {
                        Fire();
                        manaScript.UseManaAttack(2.0f);
                        m_Character.AnimAttack(true);
                    }
                    else if (Input.GetKeyUp(KeyCode.LeftControl))
                    {
                        m_Character.StopAttack(true);
                    }
                }

                
            }
            if (PlayerID == 2)
            {
                if (controllerid.Count > 1)
                {
                    if (Input.GetKeyDown("joystick " + controllerid[1] + " button 0"))
                    {
                        Fire();
                        manaScript.UseManaAttack(2.0f);
                        m_Character.AnimAttack(true);
                    }
                    else if (Input.GetKeyUp("joystick " + controllerid[1] + " button 0"))
                    {
                        m_Character.StopAttack(true);
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.RightControl))
                    {
                        Fire();
                        manaScript.UseManaAttack(2.0f);
                        m_Character.AnimAttack(true);
                    }
                    else if (Input.GetKeyUp(KeyCode.RightControl))
                    {
                        m_Character.StopAttack(true);
                    }
                }
            }

        }
        
        void Fire()
        {
            // Create the Bullet from the Bullet Prefab
            var bullet = (GameObject)Instantiate(
                bulletPrefab,
                bulletSpawn.position,
                bulletSpawn.rotation);

            //bullet.transform.position += m_Character.transform.position;

            // Add velocity to the bullet
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

            // Destroy the bullet after 2 seconds
            Destroy(bullet, 2.0f);
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            
            if (PlayerID == 1)
            {
                if (controllerid.Count > 0)
                {
                    h = Input.GetAxis("Horizontalp1" + controllerid[0]);
                    v = Input.GetAxis("Verticalp1" + controllerid[0]);
                }
                else
                {
                    h = Input.GetAxis("P1Horizontal");
                    v = Input.GetAxis("P1Vertical");
                }
                //Debug.Log(PlayerID + " h value: " + h + " cotroller id: Horizontalp" + PlayerID + controllerid[0]);
            }

            else if (PlayerID == 2)
            {
                if (controllerid.Count > 1)
                {
                    h = Input.GetAxis("Horizontalp2" + controllerid[1]);
                    v = Input.GetAxis("Verticalp2" + controllerid[1]);
                }
                else
                {
                    h = Input.GetAxis("P2Horizontal");
                    v = Input.GetAxis("P2Vertical");
                }
                //Debug.Log(PlayerID + " h value: " + h + " cotroller id: Horizontalp" + PlayerID + controllerid[1]);
            }
            // read inputs
            bool crouch = false;

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v*m_CamForward + h*m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v*Vector3.forward + h*Vector3.right;
            }
#if !MOBILE_INPUT
			// walk speed multiplier
	        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
        }
    }
}

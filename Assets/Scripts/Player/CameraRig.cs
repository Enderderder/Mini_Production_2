using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour 
{
	public float DampTime = 0.2f;
    public Vector3 CameraOffset;
    public float ZoomMax = 60.0f;
    public float ZoomMin = 10.0f;
    public float ZoomLimmit = 100.0f;

	public float m_ScreenEdgeBuffer = 4f;
	public float m_MinSize = 6.5f;

    /*[HideInInspector]*/
    //public Transform[] m_Targets;

    public Transform m_playerPos_1;
    public Transform m_playerPos_2;

    public Bounds m_cameraBound;

    private Camera m_camera;
	private float m_ZoomSpeed;
	private Vector3 m_MoveVelocity;
	private Vector3 m_DesiredPosition;

	private void Awake()
	{
		m_camera = GetComponent<Camera>();
	}

    private void Start()
    {
        // Force this camera to be the main camera
        this.gameObject.tag = "MainCamera";

        // Try to get the reference Location of both player
        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");
        if (player1 && player2) // If both player exists
        {
            m_playerPos_1 = player1.transform;
            m_playerPos_2 = player2.transform;
        }
        else
        {
            Debug.Log(gameObject.name + ": Cannot find the player");
        }

        m_cameraBound = GetEncapsulatingBounds();
        m_DesiredPosition = CalculateDesirePosition();
    }

    private void LateUpdate()
    {
        // Refresh the bound that include both player
        m_cameraBound = GetEncapsulatingBounds();

        // Refresh the desire location of the camera
        m_DesiredPosition = CalculateDesirePosition();
    }


    private void FixedUpdate()
	{
        if (m_playerPos_1 && m_playerPos_2)
        {
            MoveTowardsDesirePosition();
            //RotateTowardsDesirePosition();
            Zoom();
        }
	}

	private void MoveTowardsDesirePosition()
	{
		transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, DampTime);
	}

    private void RotateTowardsDesirePosition()
    {
        Vector3 centrePosition = Vector3.Lerp(m_playerPos_1.position, m_playerPos_2.position, 0.5f);

        Vector3 rotateDir = centrePosition - this.transform.position;

        transform.rotation = Quaternion.LookRotation(rotateDir);
    }

	private Vector3 CalculateDesirePosition()
	{
        Vector3 desirePosition = new Vector3();

        // Get the centre of the players
        Vector3 centrePosition = Vector3.Lerp(m_playerPos_1.position, m_playerPos_2.position, 0.5f);

        // Set the horizontal offset of the camera
        desirePosition = centrePosition + CameraOffset;

        return desirePosition;
	}

    private void Zoom()
    {
        float greatestDistance = Vector3.Distance(m_playerPos_1.position, m_playerPos_2.position);
        float newZoom = Mathf.Lerp(ZoomMin, ZoomMax, greatestDistance / ZoomLimmit);
        
        // Smooth zoom of the camera
        m_camera.fieldOfView = Mathf.Lerp(m_camera.fieldOfView, newZoom, Time.fixedDeltaTime);
    }

    private Bounds GetEncapsulatingBounds()
    {
        // Include the player 1 at the beginning
        Bounds resultBound = new Bounds(m_playerPos_1.position, Vector3.zero);

        // Include the player 2
        resultBound.Encapsulate(m_playerPos_1.position);
        resultBound.Encapsulate(m_playerPos_2.position);

        return resultBound;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour 
{
	public int m_PlayerNumber = 1;
	public float m_speed = 6f;
	public float m_TurnSpeed = 180f;

	Vector3 movement;
	Rigidbody playerRigidbody;
	private string m_MovementAxisName;
	private string m_TurnAxisName;
	private float m_MovementInputValue;
	private float m_TurnInputValue;


	void Awake()
	{
		playerRigidbody = GetComponent<Rigidbody> ();
	}

	void OnEnable()
	{
		playerRigidbody.isKinematic = false;
		m_MovementInputValue = 0f;
		m_TurnInputValue = 0f;
	}

	void Start()
	{
		m_MovementAxisName = "Vertical";// + m_PlayerNumber;
		m_TurnAxisName = "Horizontal";// + m_PlayerNumber;
	}

	void Update()
	{
		m_MovementInputValue = Input.GetAxis (m_MovementAxisName);
		m_TurnInputValue = Input.GetAxis (m_TurnAxisName);
	}

	void FixedUpdate()
	{
		//float h = Input.GetAxisRaw ("Horizontal");
		//float v = Input.GetAxisRaw ("Vertical");
		Move();
		Turn();
	}

	void Move()
	{
		movement.Set (m_TurnInputValue, 0.0f, m_MovementInputValue);
		movement = movement.normalized * m_speed * Time.deltaTime;
		playerRigidbody.MovePosition (transform.position + movement);
	}

	void Turn()
	{
		//float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
		//Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);
		//playerRigidbody.MoveRotation (playerRigidbody.rotation * turnRotation);
	}
}

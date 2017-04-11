using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float m_MovementSpeed = 1.0f;
	public float m_JumpPower = 1.0f;
	public float m_TurnDamping = 0.01f;
	public Animator m_Animator;
	public LayerMask m_EnemyMask;

	private CharacterController m_CharacterController;

	private float m_HorizontalValue;
	private float m_VerticalValue;
	private float m_FireValue;
	private float m_JumpValue;
	private Vector3 m_MoveVelocity = new Vector3 ();

	private float vSpeed = 0f;

	// Use this for initialization
	void Start () {
		m_CharacterController = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		m_HorizontalValue = Input.GetAxis ("Horizontal");
		m_VerticalValue = Input.GetAxis ("Vertical");
		m_FireValue = Input.GetAxis ("Fire3");
		m_JumpValue = Input.GetAxis ("Jump");
	}

	void FixedUpdate() {
		Vector3 look = new Vector3 (m_HorizontalValue, 0.0f, m_VerticalValue);

		m_Animator.SetFloat ("Walking", look.magnitude);

		if (m_FireValue > 0.0f) {
			m_Animator.SetTrigger ("hit");

			Collider[] hits = Physics.OverlapSphere (transform.position, 2.0f, m_EnemyMask);

			for (int i = 0; i < hits.Length; i++) {
				GameObject obj = hits [i].gameObject;

				if (obj != this.gameObject)
					obj.SetActive (false);
			}
		}


		/*m_Rigidbody.MoveRotation (Quaternion.LookRotation ((Vector3.SmoothDamp (this.transform.forward, look, ref m_MoveVelocity, m_TurnDamping))));

		m_Rigidbody.MovePosition (m_Rigidbody.position + (Vector3.Normalize(look) * m_MovementSpeed * Time.deltaTime));*/

		transform.LookAt ((transform.position + Vector3.SmoothDamp (this.transform.forward, look, ref m_MoveVelocity, m_TurnDamping)));

		//m_CharacterController.SimpleMove ((Vector3.Normalize (look) * m_MovementSpeed * Time.deltaTime));



		Vector3 direction = Vector3.Normalize (look) * m_MovementSpeed;

		if (m_CharacterController.isGrounded) {
			vSpeed = 0f;

			if (m_JumpValue > 0.0f) {
				vSpeed = m_JumpPower;
				print ("jumping");
			}
		}

		vSpeed -= 9.8f * Time.deltaTime;

		direction.y = vSpeed;

		m_CharacterController.Move (direction * Time.deltaTime);
	}
}

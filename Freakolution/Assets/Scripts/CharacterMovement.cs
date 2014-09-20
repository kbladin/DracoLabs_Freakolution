using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

	private float speed = 20f;

	public Animator[] animators;

	void Start()
	{
		animators = gameObject.GetComponentsInChildren<Animator>();
	}
	
	void FixedUpdate()
	{
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		
		Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);
		
		rigidbody.velocity = moveDirection * speed;
		//rigidbody.AddForce(moveDirection*speed);

		// Animation data
		//Mathf.Abs (rigidbody.velocity);
		animators [0].SetFloat ("speed", rigidbody.velocity.magnitude);
	}
}

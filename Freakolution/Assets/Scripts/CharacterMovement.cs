using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

	private float speed = 20f;
	
	void Start()
	{
	}
	
	void FixedUpdate()
	{
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		
		Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);
		float horizontalAngle = Mathf.Atan2(verticalInput, horizontalInput);
		
		rigidbody.velocity = moveDirection * speed;
		//rigidbody.AddForce(moveDirection*speed);
	}
}

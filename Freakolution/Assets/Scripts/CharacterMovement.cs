using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

	private float speed = 20f;
	public Vector3 moveDirection;
	
	void Start()
	{
		moveDirection = Vector3.forward;
	}
	
	void FixedUpdate()
	{
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		
		if(horizontalInput != 0 && verticalInput != 0)
		{

		}
		moveDirection = new Vector3(horizontalInput, 0, verticalInput);
		rigidbody.velocity = moveDirection * speed;
		//rigidbody.AddForce(moveDirection*speed);
	}
}

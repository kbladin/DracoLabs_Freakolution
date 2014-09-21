using UnityEngine;
using System.Collections;

public class CharacterSprite : MonoBehaviour {
	
	private Rigidbody parent_rigid_body;
	private Animator anim;
	
	void Start()
	{
		anim = GetComponent<Animator> ();
		parent_rigid_body = GetComponentInParent<Rigidbody>();
	}
	
	void FixedUpdate()
	{
		float horizontalAngle = Mathf.Atan2(parent_rigid_body.velocity.z, parent_rigid_body.velocity.x);

		anim.SetFloat ("speed", parent_rigid_body.velocity.magnitude);
		anim.speed = parent_rigid_body.velocity.magnitude / 15;
		anim.SetFloat ("horizontalAngle", horizontalAngle);
	}
}

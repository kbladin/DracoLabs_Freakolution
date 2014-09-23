using UnityEngine;
using System.Collections;

public class CharacterSprite : MonoBehaviour {
	
	private CharacterController parent_character_controller;
	private Animator anim;
	
	void Start()
	{
		anim = GetComponent<Animator> ();
		parent_character_controller = GetComponentInParent<CharacterController>();
	}
	
	void FixedUpdate()
	{
		float horizontalAngle = Mathf.Atan2(parent_character_controller.velocity.z, parent_character_controller.velocity.x);

		anim.SetFloat ("speed", parent_character_controller.velocity.magnitude);
		float characterSpeed = parent_character_controller.velocity.magnitude;

		anim.speed = characterSpeed > 0.1f ? characterSpeed / 5 : 1.0f;
		anim.SetFloat ("horizontalAngle", horizontalAngle);
	}
}

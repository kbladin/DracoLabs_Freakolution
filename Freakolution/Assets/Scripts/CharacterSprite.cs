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
		float horizontalAngle = Mathf.Atan2(
			gameObject.GetComponentInParent<Player>().controller.GetDirection().z,
			gameObject.GetComponentInParent<Player>().controller.GetDirection().x);

		anim.SetFloat ("speed", parent_character_controller.velocity.magnitude);
		float characterSpeed = parent_character_controller.velocity.magnitude;

		anim.speed = characterSpeed > 0.1f ? characterSpeed / 3 : 1.0f;
		if (gameObject.GetComponentInParent<Player> ().attacking) {
			anim.speed = 1;
		}
		anim.SetFloat ("horizontalAngle", horizontalAngle);

		if (gameObject.GetComponentInParent<Player>().attacking) {
			anim.SetBool ("attacking", true);
			//gameObject.GetComponentInParent<Player> ().attacking = false;
		}

		if ((anim.GetCurrentAnimatorStateInfo (0).IsName ("Character_attack_back") ||
		     anim.GetCurrentAnimatorStateInfo (0).IsName ("Character_attack_front") ||
		     anim.GetCurrentAnimatorStateInfo (0).IsName ("Character_attack_right") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Character_attack_left")) &&
		    anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.8) { // 0.8 due to numerical problems
			anim.SetBool ("attacking", false);
			gameObject.GetComponentInParent<Player> ().attacking = false;
		}

		/*
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Character_attack_left")) {
			anim.SetBool ("attacking", false);
			gameObject.GetComponentInParent<Player> ().attacking = false;

		}*/

	}
}

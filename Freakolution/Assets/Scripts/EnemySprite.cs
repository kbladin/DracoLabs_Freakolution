using UnityEngine;
using System.Collections;

public class EnemySprite : MonoBehaviour {
	private Animator anim;
	
	void Start()
	{
		anim = GetComponent<Animator> ();
	}
	
	void FixedUpdate()
	{
		Enemy enemyComponent = gameObject.GetComponentInParent<Enemy> ();
		float horizontalAngle = Mathf.Atan2(
			enemyComponent.GetDirection().z,
			enemyComponent.GetDirection().x);
	
		anim.SetFloat ("horizontalAngle", horizontalAngle);

		float enemySpeed = enemyComponent.GetVelocity().magnitude;
		anim.SetFloat ("speed", enemySpeed);
		anim.speed = enemySpeed > 0.1f ? enemySpeed / 3 : 1.0f;

		if (enemyComponent.attacking) {
			anim.speed = 1;
			anim.SetBool ("attacking", true);
		}

		if ((anim.GetCurrentAnimatorStateInfo (0).IsName ("Enemy_attack_back") ||
		     anim.GetCurrentAnimatorStateInfo (0).IsName ("Enemy_attack_front") ||
		     anim.GetCurrentAnimatorStateInfo (0).IsName ("Enemy_attack_right") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("Enemy_attack_left")) &&
		    anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.8) { // 0.8 due to numerical problems
			anim.SetBool ("attacking", false);
			enemyComponent.attacking = false;
		}

		/*
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Character_attack_left")) {
			anim.SetBool ("attacking", false);
			gameObject.GetComponentInParent<Player> ().attacking = false;

		}*/

	}
}

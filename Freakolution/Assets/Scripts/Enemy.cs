using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public bool attacking;

	private float health;
	private float attackCooldownTime;
	private Vector3 moveDirection;
	private float attackRange;
	public float damage;
	// Use this for initialization
	void Start () 
	{
		// needs to get the enemies movement direction
		moveDirection = Vector3.forward;
		health = 100f;
		attackCooldownTime = 3f;
		attackRange = 2f;
		damage = 10f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(health < 0)
		{
			Destroy(gameObject);
		}
		
		if(attackCooldownTime>=3)
		{
			Attack();
		}
		
		if(attackCooldownTime<3)
		{
			attackCooldownTime += Time.deltaTime;
		}
		
	}
	
	public void LoseHealth(float damage)
	{
		//implement damage formula
		health -= damage;
	}

	public Vector3 GetDirection() {
		// Not implemented.
		// return new Vector3(0,0,1);
		return (GetVelocity ().magnitude > 0.1 ?
		        GetComponent<AI> ().GetMoveDirection () :
		        GetComponent<AI> ().GetDirectionToTarget ());
	}

	public Vector3 GetVelocity() {
		// Function not implemented.
		return GetComponent<AI>().GetVelocity();
	}
	
	private void Attack()
	{
		RaycastHit hit;
		Vector3 rayDirection = GetComponent<AI>().GetDirectionToTarget();
		Ray rayCast = new Ray(transform.position, rayDirection);

		if(Physics.Raycast(rayCast, out hit))
		{
			float distance = hit.distance;
			
			if(distance < attackRange && hit.transform.tag=="Player"){
				attacking = true;

				Player playerComponent = hit.transform.GetComponent<Player>();
				playerComponent.LoseHealth(damage);
				attackCooldownTime = 0;
			}
		}
	}
}

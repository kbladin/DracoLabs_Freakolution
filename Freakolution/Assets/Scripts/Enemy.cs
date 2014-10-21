﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public bool attacking;

	private float health;
	private float attackCooldownTime;
	private Vector3 moveDirection;
	private float attackRange;
	private float damage;
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
		// Does not work since the transform is affected by the camera. Need a separate direction vector.
		return new Vector3(0,0,-1);
	}

	public Vector3 GetVelocity() {
		// Function not implemented.
		return new Vector3(1,0,0);
	}
	
	private void Attack()
	{
		attacking = true;

		RaycastHit hit;
		Vector3 rayDirection = transform.forward;
		Ray rayCast = new Ray(transform.position, rayDirection);

		if(Physics.Raycast(rayCast, out hit))
		{
			float distance = hit.distance;
			
			if(distance < attackRange && hit.transform.tag=="Player"){
				
				Player playerComponent = hit.transform.GetComponent<Player>();
				playerComponent.LoseHealth(damage);
				attackCooldownTime = 0;
			}
		}
	}
}

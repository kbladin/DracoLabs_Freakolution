﻿using UnityEngine;
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
		attackRange = 1f;
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
		
		Vector3 attackDirection = GetComponent<AI>().GetDirectionToTarget();
		//sphere.position=transform.position + attackDirection * attackRange;
		Collider[] targets = Physics.OverlapSphere(transform.position + attackDirection * attackRange, 0.5f);
		Transform nearest = null;
		float closestDistance = attackRange+0.5f;
		foreach (Collider hit in targets){
			if(hit && hit.tag == "Player"){
				float dist = Vector3.Distance(transform.position, hit.transform.position);
				if(dist < closestDistance){
					attacking = true;
					closestDistance = dist;
					nearest = hit.transform;
				}
			}
		}
		
		if(nearest){
			Player enemyComponent = nearest.transform.GetComponent<Player>();
			enemyComponent.LoseHealth(damage);
			attackCooldownTime = 0;
		}
	}
}

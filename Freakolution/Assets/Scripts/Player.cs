﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float health;
	public bool attacking;
	private float attackRange;
	private float damage;
	private bool alive;
	//property
	public bool Alive {get{return alive;} set{alive = value;}}
	//needs this to get movement direction
	private string fireInputName;
	public ThirdPersonController controller;
	
	// Use this for initialization
	void Start () 
	{
		alive = true;
		attackRange = 1f;

		attacking = false;

		damage = 30f;
		health = 200f;
		controller = GetComponent<ThirdPersonController>();
		fireInputName = controller.GetFireInputName();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetButtonDown(fireInputName))
		{
			Attack();
		}
	}

	public void LoseHealth(float damage) 
	{
		//here needs to be damage formula
		this.health -= damage;
		if(health <= 0)
		{
			alive = false;
			gameObject.SetActive(false);
			//GetComponentInChildren<Renderer>().enabled = false;
		}
	}
	
	private void Attack() 
	{
		attacking = true;

		Vector3 attackDirection = controller.GetDirection();
		//sphere.position=transform.position + attackDirection * attackRange;
		Collider[] targets = Physics.OverlapSphere(transform.position + attackDirection * attackRange, 0.5f);
		Transform nearest = null;
		float closestDistance = attackRange+0.5f;
		foreach (Collider hit in targets){
			if(hit && hit.tag == "Enemy"){
				float dist = Vector3.Distance(transform.position, hit.transform.position);
				if(dist < closestDistance){
					//Debug.Log(dist);
					closestDistance = dist;
					nearest = hit.transform;
				}
			}
		}
		
		if(nearest){
			Enemy enemyComponent = nearest.transform.GetComponent<Enemy>();
			enemyComponent.LoseHealth(damage);
		}
		
	}
	
	
}

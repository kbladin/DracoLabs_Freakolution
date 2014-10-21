using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float health;
	private float attackRange;
	private float damage;
	//Can be removed before merging with master/developS
	public Transform sphere;
	//needs this to get movement direction
	ThirdPersonController controller;
	
	// Use this for initialization
	void Start () 
	{
		attackRange = 1f;
		damage = 30f;
		health = 200f;
		controller = GetComponent<ThirdPersonController>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetButtonDown("Fire1"))
		{
			Attack();
		}
		if(health <= 0)
		{	
			//pause time.
			Time.timeScale = 0.0f;
			//destroying gameObject will cause error since AI script is trying to reach it.
			//Destroy(gameObject);
		}
	}
	
	public void LoseHealth(float damage) 
	{
		//here needs to be damage formula
		this.health -= damage;
	}
	
	private void Attack() 
	{
		// gets the players normalized moveDirection
		Vector3 attackDirection = controller.GetDirection();
		//only for testing
		sphere.position=transform.position + attackDirection * attackRange;
		//gets all the colliders that is overlapping the attackSphere
		Collider[] targets = Physics.OverlapSphere(sphere.position, 0.5f);
		//we want to find the nearest one.
		Transform nearest = null;
		
		float closestDistance = attackRange+0.5f; //0.5 = radius of sphere
		
		foreach (Collider hit in targets){
			if(hit && hit.tag == "Enemy"){
				// get the distance to the enemy.
				float dist = Vector3.Distance(transform.position, hit.transform.position);
				if(dist < closestDistance){
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

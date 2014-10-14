using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float health;
	private float attackRange;
	private float damage;
	//needs this to get movement direction
	ThirdPersonController controller;
	
	// Use this for initialization
	void Start () 
	{
		attackRange = 3f;
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
			Destroy(gameObject);
		}
	}
	
	public void LoseHealth(float damage) 
	{
		//here needs to be damage formula
		this.health -= damage;
	}
	
	private void Attack() 
	{
		RaycastHit hit;
		Vector3 rayDirection = controller.GetDirection();
		Ray rayCast = new Ray(transform.position, rayDirection);
		
		if(Physics.Raycast(rayCast, out hit))
		{	
			Debug.Log(hit.distance);
			if(hit.distance < attackRange && hit.transform.tag=="Enemy"){
				
				Enemy enemyComponent = hit.transform.GetComponent<Enemy>();
				enemyComponent.LoseHealth(damage);
			}
		}
	}
	
	
}

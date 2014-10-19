using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float health;
	private float attackRange;
	private float damage;
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
		Vector3 attackDirection = controller.GetDirection();
		sphere.position=transform.position + attackDirection * attackRange;
		Collider[] targets = Physics.OverlapSphere(sphere.position, 0.5f);
		Transform nearest = null;
		float closestDistance = attackRange+2;
		foreach (Collider hit in targets){
			if(hit && hit.tag == "Enemy"){
				float dist = Vector3.Distance(transform.position, hit.transform.position);
				if(dist < closestDistance){
					Debug.Log(dist);
					closestDistance = dist;
					nearest = hit.transform;
				}
			}
		}
		
		if(nearest){
			Enemy enemyComponent = nearest.transform.GetComponent<Enemy>();
			enemyComponent.LoseHealth(damage);
		}
		
		
		/*if(Physics.SphereCast(transform.position, 1, rayDirection, out hit))
		{	
			cube.position=hit.point;
			cube.rotation = Quaternion.LookRotation(hit.normal);
			Debug.Log(hit.distance);
			Debug.Log (hit.collider.gameObject.name);
			if(hit.distance < attackRange && hit.transform.tag=="Enemy"){
				
				Enemy enemyComponent = hit.transform.GetComponent<Enemy>();
				enemyComponent.LoseHealth(damage);
			}
		}*/
	}
	
	
}

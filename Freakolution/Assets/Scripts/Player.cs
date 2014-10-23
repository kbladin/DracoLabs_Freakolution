using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float health;
	private float attackRange;
	private float damage;
	private bool alive;
	//property
	public bool Alive {get{return alive;} set{alive = value;}}
	//needs this to get movement direction
	ThirdPersonController controller;
	private string fireInputName;
	
	// Use this for initialization
	void Start () 
	{
		alive = true;
		attackRange = 3f;
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

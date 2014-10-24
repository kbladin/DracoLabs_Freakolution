using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float health;
	public bool attacking;
	private float attackRange;
	private float playerDamage;
	private bool alive;
	private Chemicals playerChemicals;
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
		ParticleSystem[] ps = GetComponentsInChildren<ParticleSystem>();
		ps[1].startColor = playerChemicals.getChemicals();

		playerDamage = 30f;
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

	public void LoseHealth(float damage, Chemicals enemyChemicals) 
	{
		//here needs to be damage formula
		this.health -= damage * (1 + enemyChemicals.getReaction(enemyChemicals));
		if(health <= 0)
		{
			alive = false;
			gameObject.SetActive(false);
		}
	}
	
	private void Attack() 
	{
		attacking = true;

		Vector3 attackDirection = controller.GetDirection();

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
			enemyComponent.LoseHealth(playerDamage, playerChemicals);
		}
		
	}
	
	public void SetChemicals(Chemicals theChemicals)
	{
		playerChemicals = theChemicals;
	}
	
}

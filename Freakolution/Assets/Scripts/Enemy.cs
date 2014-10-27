using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public bool attacking;

	private float health;
	private float attackCooldownTime;
	private Vector3 moveDirection;
	private float attackRange;
	public float enemyDamage;
	private Chemicals enemyChemicals;
	// Use this for initialization
	void Start () 
	{
		// needs to get the enemies movement direction
		moveDirection = Vector3.forward;
		health = 100f;
		attackCooldownTime = 3f;
		attackRange = 1f;
		enemyDamage = 10f;
		//GetComponentInChildren<Light>().color = new Color(enemyChemicals.Redion, enemyChemicals.Greenium, enemyChemicals.Blurine);
		GetComponentInChildren<Light>().color = enemyChemicals.getChemicals();
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
	
	public void LoseHealth(float damage, Chemicals chemicals)
	{
		//implement damage formula
		this.health -= damage*(1-chemicals.getReaction(enemyChemicals));
	}

	public Vector3 GetDirection() {
		// Not implemented.
//		 return new Vector3(1,0,0);
		return (GetVelocity ().magnitude > 0.1 ?
		        GetComponent<AI> ().GetMoveDirection () :
		        GetComponent<AI> ().GetDirectionToTarget ());

//		return GetComponent<AI> ().GetMoveDirection ();
	}

	public Vector3 GetVelocity() {
		// Function not implemented.
		return GetComponent<AI>().GetVelocity();
	}
	
	private void Attack()
	{
		
		Vector3 attackDirection = GetComponent<AI>().GetDirectionToTarget();
		//sphere.position=transform.position + attackDirection * attackRange;
		Collider[] targets = Physics.OverlapSphere(transform.position + attackDirection * attackRange, 1.2f);
		Transform nearest = null;
		float closestDistance = attackRange+1.2f;
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
			enemyComponent.LoseHealth(enemyDamage, enemyChemicals);
			attackCooldownTime = 0;
		}
	}
	
	public void SetChemicals(Chemicals theChemicals)
	{
		enemyChemicals = theChemicals;
	}
}

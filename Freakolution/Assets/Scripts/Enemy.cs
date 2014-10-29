using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public bool attacking;
	public float maxHealth;
	private float health;
	private float attackCooldownTime;
	private Vector3 moveDirection;
	private float attackRange;
	public float enemyDamage;
	private Chemicals enemyChemicals;
	// Use this for initialization
	public GameObject drawSpherePrefab;
	//private GameObject drawSphere;
	public GameObject sparkPrefab;
	public GameObject bloodPrefab;
	private GameObject[] playerList;
	private float[] damageTaken;
	/*public GameObject drawCapsulePrefab;
	private GameObject drawCapsule;
*/

	//Sound effects
	public AudioClip zapAudio;
	public AudioClip[] hurtAudio;
	
	void Start () 
	{
		// needs to get the enemies movement direction
		moveDirection = Vector3.forward;
		health = maxHealth;
		attackCooldownTime = 3f;
		attackRange = 1f;
		enemyDamage = 10f;
		//GetComponentInChildren<Light>().color = new Color(enemyChemicals.Redion, enemyChemicals.Greenium, enemyChemicals.Blurine);
		GetComponentInChildren<ParticleSystem>().startColor = enemyChemicals.getChemicalsWithAlpha(0.15f);
		SetPlayerList ();
		findNewTarget();
	}

	void SetPlayerList(){
		playerList = GameObject.FindGameObjectsWithTag("Player");
		damageTaken = new float[playerList.Length];
		
		//need to remove dead player
	}

	void findNewTarget() {
		// Should actually find the one which is closest.
		float minimumDistance = 10000000000;
		float maxDamageTaken = 0;
		int maxDmgIndex = 0;
		int minDistIndex = 0;
		for (int i=0; i<playerList.Length; ++i) {
			//			Player p = playerList[i].GetComponent<Player>();
			if( !playerList[i].GetComponent<Player>().Alive)
				continue;
			
			if (damageTaken[i] > maxDamageTaken) {
				maxDamageTaken = damageTaken[i];
				maxDmgIndex = i;
			}
			
			if ((playerList [i].transform.position - transform.position).magnitude < minimumDistance) {
				minimumDistance = (playerList [i].transform.position - transform.position).magnitude;
				minDistIndex = i;
			}
		}
		if (maxDamageTaken > 0)
			GetComponentInParent<AI> ().setPlayer (playerList[maxDmgIndex].transform); 
		else
			GetComponentInParent<AI> ().setPlayer (playerList[minDistIndex].transform); 
	}

	// Update is called once per frame
	void Update () 
	{
		findNewTarget();
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

		/*
		if (!drawCapsule) {
			drawCapsule =
				Instantiate (
					drawCapsulePrefab, 
					transform.position,
					transform.rotation) as GameObject;
		} else {
			drawCapsule.transform.position = transform.position;
			drawCapsule.GetComponent<Transform>().localScale =
				new Vector3(
					1f,
					0.71f,
					1f);
		}*/
		
	}
	
	public void LoseHealth(float damage, Chemicals chemicals, Player playerAttacked)
	{
		//play sound effect
		int randClip = Random.Range(0, hurtAudio.Length) ;
		audio.PlayOneShot(hurtAudio[randClip]);;
		//implement damage formula
		this.health -= damage * (1-enemyChemicals.getReaction(chemicals));
		for (int i=0; i<playerList.Length; ++i) {
			if (playerList[i].GetComponent<Player>() == playerAttacked)
				damageTaken[i] += damage;
		}
		Destroy(Instantiate (bloodPrefab, transform.position, transform.rotation) as GameObject, 15f);
	}
	public float GetHealth()
	{
		return this.health;
	}

	public Vector3 GetDirection() {
		// Not implemented.
//		 return new Vector3(1,0,0);
		return (GetVelocity ().magnitude > 0.1 ?
		        GetComponent<AI> ().GetVelocity ().normalized :
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
		Vector3 attackPosition = transform.position + attackDirection * attackRange;
		float attackRadius = 0.7f;
		Collider[] targets = Physics.OverlapSphere(attackPosition, attackRadius);
		Transform nearest = null;
		float closestDistance = attackRange+attackRadius;
		/*
		if (!drawSphere) {
						drawSphere =
				Instantiate (
				drawSpherePrefab, 
				attackPosition,
				transform.rotation) as GameObject;
				} else {
			drawSphere.transform.position = transform.position + attackDirection * attackRange;
			drawSphere.GetComponent<Transform>().localScale = new Vector3(attackRadius,attackRadius,attackRadius);
		}
		*/
		foreach (Collider hit in targets){
			if(hit && hit.tag == "Player"){
				float dist = Vector3.Distance(transform.position, hit.transform.position);
				if(dist < closestDistance){
					attacking = true;
					audio.PlayOneShot(zapAudio);
					closestDistance = dist;
					nearest = hit.transform;
				}
			}
		}
		
		if(nearest){
			Player enemyComponent = nearest.transform.GetComponent<Player>();
			enemyComponent.LoseHealth(enemyDamage, enemyChemicals);
			attackCooldownTime = 0;
			Destroy(Instantiate(sparkPrefab, attackPosition, transform.rotation) as GameObject, 1f);
		}
	}
	
	public void SetChemicals(Chemicals theChemicals)
	{
		enemyChemicals = theChemicals;
	}
}

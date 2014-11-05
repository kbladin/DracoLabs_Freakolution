using UnityEngine;
using System.Collections;

public struct PlayerStats {
	public int playerNumber;
	public int killCount;
	public float damageDone;
	public float damageTaken;
	public int barrelsBuilt;
	public int barrelsMoved;
	public Chemicals chemicals;
}

public class Player : MonoBehaviour {

	//Fields
	private GameObject gameOverDisplay;
	private float maxHealth;
	private float health;
	//public PlayerStats stats;
	public bool attacking;
	private float attackCooldownTime;
	private float pukeCooldownTime;
	private float buildCooldownTime;
	private float pukeCooldown = 0;
	private float attackCooldown = 0;
	private float buildCooldown = 0;
	private float attackRange;
	private float playerDamage;
	private float playerPukeDamage;
	private bool alive;
	private Chemicals playerChemicals;
	private bool blockWait = false;
	private GameObject carriedBlock;
	//needs this to get movement direction
	public ThirdPersonController controller;
	public GameObject blockPrefab;
	public GameObject renderBlockPrefab;
	private GameObject renderBlock;
	public GameObject pukePrefab;
	public GameObject puke;
	Node previousNode = null;
	Node currentNode = null;
	bool previousObst = false;
	
	//Maybe the healer should be its own inherited class of player
	// but for now its just a boolean
	private bool isHealer = false;
	
	//Properties
	public bool Alive {get{return alive;} set{alive = value;}}
	public bool IsHealer {get{return isHealer;} set{isHealer=value;}}
	public float MaxHealth {get{return maxHealth;} set{maxHealth=value;}}
	public float AttackCooldownTime {get{return attackCooldownTime;} set{attackCooldownTime = value;}}
	public float PukeCooldownTime {get{return pukeCooldownTime;} set{pukeCooldownTime = value;}}
	public float BuildCooldownTime {get{return buildCooldownTime;} set{buildCooldownTime = value;}}
	public float PlayerDamage {get{return playerDamage;} set{playerDamage = value;}}
	public float PlayerPukeDamage {get{return playerPukeDamage;} set{playerPukeDamage = value;}}
	
	public Chemicals PlayerChemicals {get{return playerChemicals;} set{playerChemicals = value;}}
	
	//Sound effects
	public AudioClip[] attackAudio;
	public AudioClip[] hurtAudio;
	public AudioClip barrelDrop;
	public AudioClip missSwoosh;
	public AudioClip hitEnemy;
	public AudioClip runningAudio;
	public AudioClip barrelPickup;
	public AudioClip pukeAudio;

	public PlayerStats stats;

	// Use this for initialization
	void Start ()
	{
		alive = true;
		attackRange = 1f;

		attacking = false;
		ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
		ps.startColor = playerChemicals.getChemicals();
		GetComponentInChildren<SpriteRenderer> ().color =
			new Color(playerChemicals.getChemicals ().r + 0.6f,
			          playerChemicals.getChemicals ().g + 0.6f,
			          playerChemicals.getChemicals ().b + 0.6f);

		health = maxHealth;
		controller = GetComponent<ThirdPersonController>();
		gameOverDisplay = GameObject.Find ("GameOverDisplay");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown(controller.GetJumpInputName()) && (pukeCooldown > pukeCooldownTime))
		{
			Puke();
		}
		if(Input.GetButtonDown(controller.GetFireInputName()) && (attackCooldown > attackCooldownTime))
		{
			Attack();
		}
		if(Input.GetButtonDown(controller.GetFire2InputName()))
		{
			blockWait = true;
		}
		if(Input.GetButtonUp(controller.GetFire2InputName()))
		{
			if ((buildCooldown > buildCooldownTime)) {
				TryBuildBlock ();
			}

			blockWait = false;
			Destroy(renderBlock);
		}
		if(blockWait){
			RenderBlockWait();
		}

		if (Input.GetButtonDown (controller.GetFire3InputName ()) && carriedBlock) {
			TryPlaceBlock();
		} else if (Input.GetButtonDown (controller.GetFire3InputName ())) {
			TryPickBlock();
		}
		
		CharacterController characterController = GetComponent<CharacterController>();
		
		if(characterController.velocity.magnitude >= 0.2f && characterController.isGrounded && !audio.isPlaying)
		{
			audio.clip = runningAudio;
			audio.Play();
		}
		else if(characterController.velocity.magnitude < 0.2f && audio.isPlaying)
		{
			audio.clip = runningAudio;
			audio.Stop();
		}

		if (carriedBlock) {
			Vector3 location = transform.position + controller.GetDirection() * 1.3f;
			Node n = Pathfinder.Instance.FindRealClosestNode(location);
			Vector3 realLoc = new Vector3(n.xCoord, n.yCoord + 0.5f, n.zCoord);
			carriedBlock.transform.position = realLoc;
			carriedBlock.GetComponent<Rigidbody>().isKinematic = true;
			carriedBlock.GetComponent<BoxCollider>().enabled = false;
		}

		if (puke) {
			puke.transform.position = transform.position + new Vector3(0,0.5f,0);
		}

		previousNode = currentNode;

		if(previousNode != null ){
			if(!previousObst)
				previousNode.walkable = true;
			previousNode.currentObject = null;
		}

		currentNode = Pathfinder.Instance.FindRealClosestNode(transform.position);
		if(currentNode.walkable == false) {
			previousObst = true;
		} else {
			previousObst = false;
		}

		//currentNode.walkable = false;
		currentNode.currentObject = gameObject;
		
		attackCooldown += Time.deltaTime;
		buildCooldown += Time.deltaTime;
		pukeCooldown += Time.deltaTime;
	}


	public void LoseHealth(float damage, Chemicals enemyChemicals) 
	{
		//play sound effect
		int randClip = Random.Range(0, hurtAudio.Length) ;
		AudioSource.PlayClipAtPoint(hurtAudio[randClip], transform.position);
		
		//Damagae formula
		float dmgToLoose = damage * playerChemicals.getReaction (enemyChemicals);
		this.health -= dmgToLoose;
		stats.damageTaken += dmgToLoose;

		if(health <= 0)
		{
			alive = false;
			stats.chemicals = playerChemicals;
			gameOverDisplay.GetComponent<GameOverDisplay>().AddPlayerStats(stats);
			currentNode.walkable = true;
			currentNode.currentObject = null;
			gameObject.SetActive(false);
		}
	}
	
	public void GainHealth(float healPower, Chemicals pukeChemicals)
	{
		this.health += healPower/2 + healPower * playerChemicals.getReaction(pukeChemicals);
		
		if(this.health > this.maxHealth)
			this.health = this.maxHealth;
	}

	public float GetHealth() {
		return this.health;
	}

	private void Attack() 
	{
		attacking = true;
		
		//play sound effect
		int randClip = Random.Range(0, attackAudio.Length) ;
		AudioSource.PlayClipAtPoint(attackAudio[randClip], transform.position);
		
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
			enemyComponent.LoseHealth(playerDamage, playerChemicals, this);
			AudioSource.PlayClipAtPoint(hitEnemy,transform.position);
		}
		else
		{
			AudioSource.PlayClipAtPoint(missSwoosh,transform.position);
		}
		attackCooldown = 0;
	}

	private void Puke() {
		AudioSource.PlayClipAtPoint(pukeAudio, transform.position);

		puke = Instantiate (pukePrefab,
		                               transform.position + new Vector3(0,0.5f,0),
		                               Quaternion.LookRotation(controller.GetDirection())) as GameObject;
		puke.GetComponent<Puke> ().SetPlayer(this);
		puke.GetComponent<Puke> ().SetPukeChemicals(playerChemicals);
		puke.GetComponent<Puke> ().SetDamage(playerPukeDamage);
		puke.GetComponent<ParticleSystem> ().startColor = playerChemicals.getChemicals();
		puke.GetComponentsInChildren<ParticleSystem> ()[1].startColor = playerChemicals.getChemicals();
		Destroy(puke, 3f);
		pukeCooldown = 0;
	}
	/*
	void OnParticleCollision(GameObject other) {
		Rigidbody body = other.rigidbody;
		if (body) {
			Vector3 direction = other.transform.position - transform.position;
			direction = direction.normalized;
			Destroy(other);// body.AddForce(direction * 5);
		}
	}
*/
	private bool CanBuildHere(Vector3 v){
		Vector3 n = Pathfinder.Instance.FindRealClosestNode(transform.position).GetVector();
		float dist = (v - n).magnitude;
		return (!Physics.CheckSphere (v, /*1 / Mathf.Sqrt(2))*/ 0.45f) && dist < 2.3);
	}

	private void TryBuildBlock() {
		Vector3 location = transform.position + controller.GetDirection() * 1.3f;
		Node n = Pathfinder.Instance.FindRealClosestNode(location);
		Vector3 realLoc = new Vector3(n.xCoord, n.yCoord + 0.5f, n.zCoord);
		if (CanBuildHere(realLoc)) {
			//Play sound effect
			AudioSource.PlayClipAtPoint(barrelDrop, transform.position);

			GameObject block = Instantiate(blockPrefab, realLoc, new Quaternion(0f,0f,0f, 0f)) as GameObject;
			n.walkable = false;
			n.currentObject = block;
			
			block.GetComponent<Rigidbody> ().isKinematic = true;
			Destroy(carriedBlock);
			buildCooldown = 0;
			stats.barrelsBuilt++;
		}
	}

	private void RenderBlockWait() {
		Vector3 location = transform.position + controller.GetDirection() * 1.3f;
		Node n = Pathfinder.Instance.FindRealClosestNode(location);
		Vector3 realLoc = new Vector3(n.xCoord, n.yCoord + 0.5f, n.zCoord);
		if (!renderBlock) {
			renderBlock = Instantiate (renderBlockPrefab, realLoc, transform.rotation) as GameObject;
				} else {
			renderBlock.transform.position = realLoc;	
		}
		if (!CanBuildHere(realLoc)) {
						renderBlock.GetComponent<MeshRenderer> ().material.color = new Color (1f, 0, 0, 0.4f);
		} else if (buildCooldown < buildCooldownTime) {
						renderBlock.GetComponent<MeshRenderer> ().material.color = new Color (1f, 1f, 0, 0.4f);
				} else {
						renderBlock.GetComponent<MeshRenderer> ().material.color = new Color (0, 1f, 0, 0.4f);
		}
	}

	private void TryPickBlock () {
		Vector3 location = transform.position + controller.GetDirection() * 1.3f;
		Collider[] targets = Physics.OverlapSphere(location , 0.45f);
		float closestDistance = 1.3f+0.35f;
		Collider nearest = null;
		foreach (Collider hit in targets) {
			if (hit.name == "Block" || hit.name == "Block(Clone)") {
				float dist = Vector3.Distance(location, hit.transform.position);
				if(dist < closestDistance){
					closestDistance = dist;
					nearest = hit;
				}
			}
		}
		if (nearest) {
			//Play sound effect
			AudioSource.PlayClipAtPoint(barrelDrop, transform.position);

			Node n = Pathfinder.Instance.FindRealClosestNode(nearest.gameObject.transform.position);
			carriedBlock = nearest.gameObject;
			n.walkable = true;
			//Play sound effect
			AudioSource.PlayClipAtPoint(barrelPickup, transform.position);
		}
	}
	public int getNumberOfKills ()
	{
		return stats.killCount;
	}
	
	public void SetChemicals(Chemicals theChemicals)
	{
		playerChemicals = theChemicals;
	}
	private void TryPlaceBlock () {
		Vector3 location = transform.position + controller.GetDirection() * 1.3f;
		Node n = Pathfinder.Instance.FindRealClosestNode(location);
		Vector3 realLoc = new Vector3(n.xCoord, n.yCoord + 0.5f, n.zCoord);
		if (carriedBlock && CanBuildHere(realLoc)) {
			//Play sound effect
			AudioSource.PlayClipAtPoint(barrelDrop, transform.position);
			carriedBlock.transform.position = realLoc;
			n.walkable = false;
			n.currentObject = carriedBlock;

			carriedBlock.GetComponent<Rigidbody> ().isKinematic = true;
			carriedBlock.GetComponent<BoxCollider>().enabled = true;
			carriedBlock = null;
			stats.barrelsMoved++;
				}
	}
	
	// should maybe be done with seperate classes
	public void SetPlayerClass(string playerClass)
	{
		if (playerClass == "Healer") {
						IsHealer = true;
						MaxHealth = 200f;
						AttackCooldownTime = 0.4f;
						PukeCooldownTime = 3.0f;
						BuildCooldownTime = 20.0f;
						PlayerDamage = 30f;
						PlayerPukeDamage = 4f;
				} else if (playerClass == "Tank") {
						IsHealer = false;
						MaxHealth = 300;
						AttackCooldownTime = 0.4f;
						PukeCooldownTime = 4.0f;
						BuildCooldownTime = 20.0f;
						PlayerDamage = 60f;
						PlayerPukeDamage = 2f;
				} else if (playerClass == "Marksman") {
						IsHealer = false;
						MaxHealth = 100;
						AttackCooldownTime = 0.5f;
						PukeCooldownTime = 2.0f;
						BuildCooldownTime = 20.0f;
						PlayerDamage = 15f;
						PlayerPukeDamage = 15f;
				} else if (playerClass == "Engineer") {
						IsHealer = false;
						MaxHealth = 200f;
						AttackCooldownTime = 0.4f;
						PukeCooldownTime = 3.0f;
						BuildCooldownTime = 3.0f;
						PlayerDamage = 30f;
						PlayerPukeDamage = 4f;
				} else {
						IsHealer = false;
						MaxHealth = 300;
						AttackCooldownTime = 0.4f;
						PukeCooldownTime = 4.0f;
						BuildCooldownTime = 20.0f;
						PlayerDamage = 60f;
						PlayerPukeDamage = 2f;
		}
	}
}



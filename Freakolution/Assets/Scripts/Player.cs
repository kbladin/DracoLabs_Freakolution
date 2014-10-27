using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float maxHealth;
	private float health;
	public bool attacking;
	public float attackCooldownTime;
	public float buildCooldownTime;
	private float attackCooldown = 0;
	private float buildCooldown = 0;
	private float attackRange;
	private float playerDamage;
	private bool alive;
	private Chemicals playerChemicals;
	private bool blockWait = false;
	private GameObject carriedBlock;
	//property
	public bool Alive {get{return alive;} set{alive = value;}}
	//needs this to get movement direction
	private string fireInputName;
	public ThirdPersonController controller;
	public GameObject blockPrefab;
	public GameObject renderBlockPrefab;
	private GameObject renderBlock;
	Node previousNode = null;
	Node currentNode = null;
	bool previousObst = false;
	// Use this for initialization
	void Start ()
	{
		alive = true;
		attackRange = 1f;

		attacking = false;
		ParticleSystem[] ps = GetComponentsInChildren<ParticleSystem>();
		ps[1].startColor = playerChemicals.getChemicals();

		playerDamage = 30f;
		health = maxHealth;
		controller = GetComponent<ThirdPersonController>();
		fireInputName = controller.GetFireInputName();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown(fireInputName) && (attackCooldown > attackCooldownTime))
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
				BuildBlock ();
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

		if (carriedBlock) {
			Vector3 location = transform.position + controller.GetDirection() * 1.3f;
			carriedBlock.transform.position = location;
			carriedBlock.GetComponent<Rigidbody>().isKinematic = true;
			carriedBlock.GetComponent<BoxCollider>().enabled = false;
		}

		attackCooldown += Time.deltaTime;
		buildCooldown += Time.deltaTime;


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

		currentNode.walkable = false;
		currentNode.currentObject = gameObject;
		


	}

	public void LoseHealth(float damage, Chemicals enemyChemicals) 
	{
		//here needs to be damage formula
		this.health -= damage * (1 + enemyChemicals.getReaction(enemyChemicals));
		if(health <= 0)
		{
			alive = false;
			gameObject.SetActive(false);

			currentNode.walkable = true;
			currentNode.currentObject = null;
		}
	}

	public float GetHealth() {
		return health;
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
		attackCooldown = 0;
	}

	private void BuildBlock() {
		Vector3 location = transform.position + controller.GetDirection() * 1.3f;
		if (!Physics.CheckSphere (location, /*1 / Mathf.Sqrt(2))*/ 0.5f)) {
			Node n = Pathfinder.Instance.FindRealClosestNode(location);

			Vector3 realLoc = new Vector3(n.xCoord, location.y, n.zCoord);

			GameObject block = Instantiate(blockPrefab, realLoc, transform.rotation) as GameObject;
			n.walkable = false;
			n.currentObject = block;
			
			block.GetComponent<Rigidbody> ().isKinematic = true;
			Destroy(carriedBlock);
			buildCooldown = 0;
		}
	}

	private void RenderBlockWait() {
		Vector3 location = transform.position + controller.GetDirection() * 1.3f;
		if (!renderBlock) {
						renderBlock = Instantiate (renderBlockPrefab, location, transform.rotation) as GameObject;
				} else {
			renderBlock.transform.position = location;	
		}
		if (Physics.CheckSphere (location, /*1 / Mathf.Sqrt (2))*/0.5f)) {
						renderBlock.GetComponent<MeshRenderer> ().material.color = new Color (1f, 0, 0, 0.4f);
		} else if (buildCooldown < buildCooldownTime) {
						renderBlock.GetComponent<MeshRenderer> ().material.color = new Color (1f, 1f, 0, 0.4f);
				} else {
						renderBlock.GetComponent<MeshRenderer> ().material.color = new Color (0, 1f, 0, 0.4f);
		}
	}

	private void TryPickBlock () {
		Vector3 location = transform.position + controller.GetDirection() * 1.3f;
		Collider[] targets = Physics.OverlapSphere(location , 0.5f);
		float closestDistance = 1.3f+0.5f;
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
			Node n = Pathfinder.Instance.FindRealClosestNode(nearest.gameObject.transform.position);
			carriedBlock = nearest.gameObject;
			n.walkable = true;

		}
	}
	
	
	public void SetChemicals(Chemicals theChemicals)
	{
		playerChemicals = theChemicals;
	}
	private void TryPlaceBlock () {
		Vector3 location = transform.position + controller.GetDirection() * 1.3f;
		if (!Physics.CheckSphere (location, 0.5f) && carriedBlock) {
						Node n = Pathfinder.Instance.FindRealClosestNode(location);
						Vector3 realLoc = new Vector3(n.xCoord, location.y, n.zCoord);
						carriedBlock.transform.position = realLoc;
						n.walkable = false;

						carriedBlock.GetComponent<Rigidbody> ().isKinematic = true;
						carriedBlock.GetComponent<BoxCollider>().enabled = true;
						carriedBlock = null;
				}
	}
}

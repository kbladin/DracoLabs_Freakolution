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
	private float damage;
	private bool alive;
	private bool blockWait = false;
	//property
	public bool Alive {get{return alive;} set{alive = value;}}
	//needs this to get movement direction
	private string fireInputName;
	public ThirdPersonController controller;
	public GameObject blockPrefab;
	public GameObject renderBlockPrefab;
	private GameObject renderBlock;

	// Use this for initialization
	void Start ()
	{
		alive = true;
		attackRange = 1f;

		attacking = false;

		damage = 30f;
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
			if ((buildCooldown > buildCooldownTime))
				BuildBlock ();

			blockWait = false;
			Destroy(renderBlock);
		}
		if(blockWait){
			RenderBlockWait();
		}

		attackCooldown += Time.deltaTime;
		buildCooldown += Time.deltaTime;
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

	public float GetHealth() {
		return health;
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
		attackCooldown = 0;
	}

	private void BuildBlock() {
		Vector3 location = transform.position + controller.GetDirection() * 1.3f;
		if (!Physics.CheckSphere (location, /*1 / Mathf.Sqrt(2))*/ 0.5f)) {
			GameObject block = Instantiate(blockPrefab, location, transform.rotation) as GameObject;
			block.GetComponent<Rigidbody> ().isKinematic = false;
		}
		buildCooldown = 0;
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
	
}

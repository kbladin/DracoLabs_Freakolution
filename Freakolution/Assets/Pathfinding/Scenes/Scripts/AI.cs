using UnityEngine;
using System.Collections;

public class AI : Pathfinding {
	
	public bool isRange = false;
	public float rangeDistance = 0.5f;
	public float speed = 12f;
	public float detectionRange = 50f;

	private GameObject playerObject;
	private Transform target;
	private CharacterController controller;
    private bool newPath = true;
    private bool moving = false;
	private bool resetPath = false;
    private GameObject[] AIList;
	private GameObject[] playerList;

	private Node currentNode;
	private Node previousNode;



	public void setPlayer(GameObject p) {
		playerObject = p;
		target = playerObject.transform;
	}
	

	void Start () 
    {
//		this.renderer.material.color = Color.red;
		SetPlayerList();
	}

//	bool isTargetOptimal(){
//		return true;
//	}

	void SetPlayerList(){
		playerList = GameObject.FindGameObjectsWithTag("Player");

		//need to remove dead player
	}

	bool isPositionOptimal() {
		Node nPlayer = Pathfinder.Instance.FindRealClosestNode(target.position);
		Node nTransform = Pathfinder.Instance.FindRealClosestNode(transform.position);
//		print(nTransform.currentObject.GetInstanceID());

//		if(nTransform.currentObject == null){
//			return false;
//		}


		if(nTransform.currentObject != null && nTransform.currentObject.GetInstanceID() != gameObject.GetInstanceID()){
//				this.renderer.material.color = Color.green;
				return false;
		} else {
			Node nBestWalkable = Pathfinder.Instance.FindClosestEmptyNode(target.position);
			if (Vector3.Distance(nPlayer.GetVector(), nTransform.GetVector()) <= Vector3.Distance(nPlayer.GetVector(), nBestWalkable.GetVector())+rangeDistance) {
//				this.renderer.material.color = Color.blue;
//				transform.LookAt(target.position);
				return true;
			} else {
//				this.renderer.material.color = Color.blue;
				return false;
			}

		}
	}

	bool isPathOptimal() {
		if (Path == null || Path.Count == 0 ){ //|| !newPath
			return false;
		}
		Node nPlayer = Pathfinder.Instance.FindRealClosestNode(target.position);
		Node nTransform = Pathfinder.Instance.FindRealClosestNode(transform.position);
		Node nPathTarget = Pathfinder.Instance.FindRealClosestNode(Path[Path.Count - 1]);
		Node nBestWalkable = Pathfinder.Instance.FindClosestEmptyNode(target.position);

		Node n = Pathfinder.Instance.FindRealClosestNode(Path[0]);
		if(n.currentObject != null){
				return false;
		}

		if (Vector3.Distance(nPlayer.GetVector(), nPathTarget.GetVector()) <= Vector3.Distance(nPlayer.GetVector(), nBestWalkable.GetVector())) {
			if(nTransform.currentObject != null && nTransform.currentObject.GetInstanceID() != gameObject.GetInstanceID()){
				return false;
			}
//			this.renderer.material.color = Color.yellow;
			return true;
		} else {
			return false;
		}
	}

	bool isTargetWithinRange() {
		if(Vector3.Distance(target.position, transform.position) < rangeDistance){
			return true;
		} else {
			return false;
		}
	}

	void findNewTarget() {
		// Should actually find the one which is closest.
		float minimumDistance = 10000000000;
		int minIndex = 0;
		for (int i=0; i<playerList.Length; ++i) {
//			Player p = playerList[i].GetComponent<Player>();
			if( !playerList[i].GetComponent<Player>().Alive)
				continue;

			if ((playerList [i].transform.position - transform.position).magnitude < minimumDistance) {
				minimumDistance = (playerList [i].transform.position - transform.position).magnitude;
				minIndex = i;
			}
		}
		target = playerList[minIndex].transform;
	}

	void Update ()
    {
		findNewTarget();

		if (!isPositionOptimal()){
			//move
			if(!isPathOptimal()){
				//newPath
//				this.renderer.material.color = Color.red;
				if(newPath)
					StartCoroutine(NewPathToFreeSpot());
				else 
//				if(!resetPath)
					StartCoroutine(NewPathBestEffort());

//				if(Path.Count == 0 || resetPath){
//					Node n = Pathfinder.Instance.FindClosestWalkableNode(transform.position);
//					this.renderer.material.color = Color.black;
//					Path.Add(new Vector3(n.xCoord, transform.position.y, n.zCoord));
//					resetPath = true;
//					transform.collider.enabled = false;
//					MoveMethod();
//				}
			}

			//only move if next point on path is free
			if (newPath && false){
				MoveMethod();
			}
			else {
			if(Path.Count >1){
				Node n = Pathfinder.Instance.FindRealClosestNode(Path[0]);
				if(n.currentObject == null || n.currentObject.GetInstanceID() == gameObject.GetInstanceID()){
//						n.currentObject = gameObject;
					MoveMethod();
				} 
			}
			}

		} else {
			//stop
			if(isTargetWithinRange()){
				//attack
			}
		}
	}

    IEnumerator NewPath()
    {
//        newPath = false;
//        FindPath(transform.position, player.position);
        yield return new WaitForSeconds(1F);
        newPath = true;
    }

	IEnumerator NewPathToFreeSpot()
	{
		newPath = false;
		Node n = Pathfinder.Instance.FindClosestEmptyNode(target.position);

		FindPath(transform.position, new Vector3(n.xCoord,0 , n.zCoord), false);
		yield return new WaitForSeconds(0.3F);
		newPath = true;
	}

	IEnumerator NewPathBestEffort()
	{
		Node n = Pathfinder.Instance.FindClosestWalkableNode(target.position);
		
		FindPath(transform.position, new Vector3(n.xCoord,0 , n.zCoord), true);
		yield return new WaitForSeconds(1F);
	}
	
	public Vector3 GetDirectionToTarget() {
		if (target){
			return (target.position - transform.position).normalized;
		}	else {
//			print("no target!");
			return new Vector3 (0,0,-1);
		}
	}

	public Vector3 GetVelocity() {
			return new Vector3 (0, 0, 0);
	}
    private void MoveMethod()
    {
        if (Path.Count > 0)
        {
			Vector3 tarPos = Path[0]; 
//			tarPos.y = transform.lossyScale.y;
			tarPos.y = tarPos.y + 0.75f; // Height of the collider / 2.

			//Vector3 realPos =  new Vector3(transform.position.x +0.2f,0.0f, transform.position.z +0.2f);

			Vector3 direction = (tarPos - transform.position).normalized;
//			Vector3 direction = (Path[0] - transform.position).normalized;





            direction.Normalize();

            
			transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, Time.deltaTime * speed);
            
			if (transform.position.x < Path[0].x + 0.4F && transform.position.x > Path[0].x - 0.4F && transform.position.z > Path[0].z - 0.4F && transform.position.z < Path[0].z + 0.4F)
            {

				resetPath = false;
//				transform.collider.enabled = true;

				previousNode = currentNode;

				if(previousNode != null && previousNode.currentObject != null && previousNode.currentObject.GetInstanceID() == gameObject.GetInstanceID()){
//					previousNode.walkable =  true;
					previousNode.currentObject = null;
				}

				Node n = Pathfinder.Instance.FindRealClosestNode(Path[0]);
				currentNode = n;//Pathfinder.Instance.Map[n.x, n.y];
				if( currentNode.currentObject == null ){//&& Path.Count == 1 currentNode.walkable == true &&
//					currentNode.walkable = false;
					currentNode.currentObject = gameObject;
				} 
                Path.RemoveAt(0);
            }

//            RaycastHit[] hit = Physics.RaycastAll(transform.position + (Vector3.up * 20F), Vector3.down, 100);
//            float maxY = -Mathf.Infinity;
//            foreach (RaycastHit h in hit)
//            {
//                if (h.transform.tag == "Untagged")
//                {
//                    if (maxY < h.point.y)
//                    {
//                        maxY = h.point.y;
//                    }
//                }
//            }
//            if (maxY > -100)
//            {
//                transform.position = new Vector3(transform.position.x, maxY + 1F, transform.position.z);
//            }
        }
    }
}

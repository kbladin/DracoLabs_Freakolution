using UnityEngine;
using System.Collections;

public class AI : Pathfinding {


	public bool isRange = false;
	public float rangeDistance = 1.0f;
	public float speed = 12f;
	public float detectionRange = 50f;

	private GameObject playerObject;
    public Transform player;
    private CharacterController controller;
    private bool newPath = true;
    private bool moving = false;
    private GameObject[] AIList;

	private Node currentNode;
	private Node previousNode;

	public void setPlayer(GameObject p) {
		playerObject = p;
		player = playerObject.transform;
	}
	

	void Start () 
    {
//        AIList = GameObject.FindGameObjectsWithTag("Enemy");
	}

	bool isTargetOptimal(){
		return true;
	}

	bool isPositionOptimal() {
		Node nPlayer = Pathfinder.Instance.FindRealClosestNode(player.position);
		Node nTransform = Pathfinder.Instance.FindRealClosestNode(transform.position);


		if(Path.Count == 0) {
			if(nTransform.currentObject != null && nTransform.currentObject != gameObject){
				//				Debug.Log("pas bon " + n.currentObject.ToString() + "contre " + gameObject.ToString());
				//				StartCoroutine(NewPathToFreeSpot());
				return false;
			} else {
				Node nBestWalkable = Pathfinder.Instance.FindClosestWalkableNode(player.position);
				if (Vector3.Distance(nPlayer.GetVector(), nTransform.GetVector()) <= Vector3.Distance(nPlayer.GetVector(), nBestWalkable.GetVector())+0.4f) {
					return true;
				} else {
					return false;
				}

			}
		} else {
			return false;
		}



//		Vector3.Distance(nPlayer.GetVector(), nTransform.GetVector()) < Vector3.Distance(nPlayer.GetVector(), nBestWalkable.GetVector())


	}

	bool isPathOptimal() {
		if (Path == null || Path.Count == 0){
			return false;
		}

		//			if(Path.Count > 1){

		Node nPlayer = Pathfinder.Instance.FindRealClosestNode(player.position);
		Node nTransform = Pathfinder.Instance.FindRealClosestNode(transform.position);
		Node nPathTarget = Pathfinder.Instance.FindRealClosestNode(Path[Path.Count - 1]);
		Node nBestWalkable = Pathfinder.Instance.FindClosestWalkableNode(player.position);

//		Node n = Pathfinder.Instance.FindRealClosestNode(Path[0]);
//		if(n.currentObject != null){
//			return false;
//		}


		if (Vector3.Distance(nPlayer.GetVector(), nPathTarget.GetVector()) <= Vector3.Distance(nPlayer.GetVector(), nBestWalkable.GetVector())+0.4f) {
//			if(nTransform.currentObject != null && nTransform.currentObject != gameObject){
//				return false;
//			}
			return true;
		} else {
			return false;
		}
//		Node nBestWalkable = Pathfinder.Instance.FindClosestWalkableNode(player.position);
	}

	bool isTargetWithinRange() {
		if(Vector3.Distance(player.position, transform.position) < rangeDistance){
			return true;
		} else {
			return false;
		}
	}

	void findNewTarget() {

	}

	void Update () 
    {

//		this.rigidbody.WakeUp();
		if (!isTargetOptimal()) {
			findNewTarget();
		}

		if (!isPositionOptimal()){
			//move
			if(!isPathOptimal()){
				//newPath
				StartCoroutine(NewPathToFreeSpot());
			}

			if(Path.Count >1){
			Node n = Pathfinder.Instance.FindRealClosestNode(Path[0]);
			
			if(n.currentObject == null){
				MoveMethod();
//				return false;
			}
			}
//			MoveMethod();

		} else {
			//stop
			if(isTargetWithinRange()){
				//attack
			}
		}



//		if(Vector3.Distance(player.position, transform.position) < 55F && !moving){
//			Node n = Pathfinder.Instance.FindRealClosestNode(transform.position);
//			if(n.currentObject != null && n.currentObject != gameObject){
//				//				Debug.Log("pas bon " + n.currentObject.ToString() + "contre " + gameObject.ToString());
//				StartCoroutine(NewPathToFreeSpot());
//			}
//			if (newPath )
//			{
//				StartCoroutine(NewPathToFreeSpot());
//				moving = true;
//			}
//		} 
//        else if (Vector3.Distance(player.position, transform.position) < 5F)
//        {
//            //Stop!
//			Node n = Pathfinder.Instance.FindRealClosestNode(this.transform.position);
//			if(n.currentObject != null && n.currentObject != gameObject){
//				//				Debug.Log("pas bon " + n.currentObject.ToString() + "contre " + gameObject.ToString());
//				StartCoroutine(NewPathToFreeSpot());
//			}
//
//			MoveMethod();
//
//        }
//
//		else if (Vector3.Distance(player.position, transform.position) < 65F && moving)
//		{
//			Node n = Pathfinder.Instance.FindClosestWalkableNode(player.position);
//			Node nReal = Pathfinder.Instance.FindRealClosestNode(transform.position);
//
//			if (Path.Count > 0)
//			{
//				if (Vector3.Distance(player.position, Path[Path.Count - 1]) > 5F)
//				{
//					if(nReal.currentObject != null && nReal.currentObject != gameObject){
//						//				Debug.Log("pas bon " + n.currentObject.ToString() + "contre " + gameObject.ToString());
//						StartCoroutine(NewPathToFreeSpot());
//					}
//					else 
//						if(Vector3.Distance(player.position, new Vector3(n.xCoord, 0 , n.zCoord)) < 4F)
//					{
//						StartCoroutine(NewPathToFreeSpot());
//
//					}
//
//				}
//			}
//			else
//			{
//
//
//				if(nReal.currentObject != null && nReal.currentObject != gameObject){
//					//				Debug.Log("pas bon " + n.currentObject.ToString() + "contre " + gameObject.ToString());
//					StartCoroutine(NewPathToFreeSpot());
//				}
//				else if (newPath && Vector3.Distance(player.position, new Vector3(n.xCoord, 0 , n.zCoord)) < 4F)
//				{
//					StartCoroutine(NewPathToFreeSpot());
//				}
//			}
//			//Move the ai towards the player
//			MoveMethod();
//		} else {
////			if(Path.Count == 0)
////				moving = false;
//
//		}

	
	}

    IEnumerator NewPath()
    {
//        newPath = false;
        FindPath(transform.position, player.position);
        yield return new WaitForSeconds(1F);
        newPath = true;
    }

	IEnumerator NewPathToFreeSpot()
	{
//		newPath = false;
		Node n = Pathfinder.Instance.FindClosestWalkableNode(player.position);

		FindPath(transform.position, new Vector3(n.xCoord,0 , n.zCoord));
		yield return new WaitForSeconds(1F);
		newPath = true;
	}


    private void MoveMethod()
    {
        if (Path.Count > 0)
        {
            Vector3 direction = (Path[0] - transform.position).normalized;

//			int i = 0;
//            foreach (GameObject g in AIList)
//            {
//				Debug.Log("Obj " + g.tag + " : " + i + " :: " + g.transform.position.ToString());
//				i++;
//                if(Vector3.Distance(g.transform.position, transform.position) < 1F)
//                {
//                    Vector3 dir = (transform.position - g.transform.position).normalized;
//                    dir.Set(dir.x, 0, dir.z);
//                    direction += 0.2F * dir;
//                }
//            }



            direction.Normalize();

            
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, Time.deltaTime * speed);
            if (transform.position.x < Path[0].x + 0.4F && transform.position.x > Path[0].x - 0.4F && transform.position.z > Path[0].z - 0.4F && transform.position.z < Path[0].z + 0.4F)
            {
				previousNode = currentNode;
				if(previousNode != null && previousNode.currentObject != null && previousNode.currentObject == gameObject){
//					previousNode.walkable = true;
					previousNode.currentObject = null;
				}
				Node n = Pathfinder.Instance.FindRealClosestNode(Path[0]);
				currentNode = n;//Pathfinder.Instance.Map[n.x, n.y];
				if( currentNode.currentObject == null ){//&& Path.Count == 1 currentNode.walkable == true &&
//					currentNode.walkable = false;
					currentNode.currentObject = gameObject;
				} 
//				else {
//					StartCoroutine(NewPath());
//				}

//				if(Path.Count > 1) {
//					Node n1 = Pathfinder.Instance.FindRealClosestNode(Path[1]);
//					if(n1.walkable == false){
//						StartCoroutine(NewPath());
//					}
//				}
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
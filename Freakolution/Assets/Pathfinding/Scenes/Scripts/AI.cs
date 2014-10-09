using UnityEngine;
using System.Collections;

public class AI : Pathfinding {

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


	void OnCollisionEnter(Collision other){
//		Debug.Log(other.gameObject.tag);

//		if(other.gameObject.tag == "Enemy"){
////			SendMessageUpwards("CollisionWithOtherWall", SendMessageOptions.DontRequireReceiver);
////			Debug.Log("Collision With Other Enemy");
//
//			Node n = Pathfinder.Instance.FindRealClosestNode(this.transform.position);
//			if(n.currentObject == null){
//				n.currentObject = gameObject;
//				n.walkable = false;
//				
//			}
//			if(n.currentObject != gameObject){
//				//				Debug.Log("pas bon " + n.currentObject.ToString() + "contre " + gameObject.ToString());
//				StartCoroutine(NewPathToFreeSpot());
//			}
//		}
	}

	void OnCollisionStay(Collision other){
//				Debug.Log(other.gameObject.tag);
		
//		if(other.gameObject.tag == "Enemy"){
//			//			SendMessageUpwards("CollisionWithOtherWall", SendMessageOptions.DontRequireReceiver);
//			Debug.Log("Collision Stay With Other Enemy");
//			
//			Node n = Pathfinder.Instance.FindRealClosestNode(this.transform.position);
//			if(n.currentObject == null){
//				n.currentObject = gameObject;
//				n.walkable = false;
//				
//			}
//			if(n.currentObject != gameObject){
//				//				Debug.Log("pas bon " + n.currentObject.ToString() + "contre " + gameObject.ToString());
//				StartCoroutine(NewPathToFreeSpot());
//			}
//		}
	}


	void Start () 
    {
        AIList = GameObject.FindGameObjectsWithTag("Enemy");
	}
	
	void Update () 
    {

//		this.rigidbody.WakeUp();


		if(Vector3.Distance(player.position, transform.position) < 55F && !moving){
			Node n = Pathfinder.Instance.FindRealClosestNode(transform.position);
			if(n.currentObject != null && n.currentObject != gameObject){
				//				Debug.Log("pas bon " + n.currentObject.ToString() + "contre " + gameObject.ToString());
				StartCoroutine(NewPathToFreeSpot());
			}
			if (newPath )
			{
				StartCoroutine(NewPathToFreeSpot());
				moving = true;
			}
		} 
        else if (Vector3.Distance(player.position, transform.position) < 5F)
        {
            //Stop!
			Node n = Pathfinder.Instance.FindRealClosestNode(this.transform.position);
//			if(n.currentObject == null){
//				n.currentObject = gameObject;
//			}
			if(n.currentObject != null && n.currentObject != gameObject){
				//				Debug.Log("pas bon " + n.currentObject.ToString() + "contre " + gameObject.ToString());
				StartCoroutine(NewPathToFreeSpot());
			}
			MoveMethod();
        }

		else if (Vector3.Distance(player.position, transform.position) < 65F && moving)
		{
			Node n = Pathfinder.Instance.FindClosestWalkableNode(player.position);
			Node nReal = Pathfinder.Instance.FindRealClosestNode(transform.position);

			if (Path.Count > 0)
			{
				if (Vector3.Distance(player.position, Path[Path.Count - 1]) > 5F)
				{
					if(nReal.currentObject != null && nReal.currentObject != gameObject){
						//				Debug.Log("pas bon " + n.currentObject.ToString() + "contre " + gameObject.ToString());
						StartCoroutine(NewPathToFreeSpot());
					}
					else 
						if(Vector3.Distance(player.position, new Vector3(n.xCoord, 0 , n.zCoord)) < 4F)
					{
						StartCoroutine(NewPathToFreeSpot());

					}

				}
			}
			else
			{


				if(nReal.currentObject != null && nReal.currentObject != gameObject){
					//				Debug.Log("pas bon " + n.currentObject.ToString() + "contre " + gameObject.ToString());
					StartCoroutine(NewPathToFreeSpot());
				}
				else if (newPath && Vector3.Distance(player.position, new Vector3(n.xCoord, 0 , n.zCoord)) < 4F)
				{
					StartCoroutine(NewPathToFreeSpot());
				}
			}
			//Move the ai towards the player
			MoveMethod();
		} else {
			if(Path.Count == 0)
				moving = false;
		}



//        if (Vector3.Distance(player.position, transform.position) < 25F && !moving)
//        {
//            if (newPath)
//            {
//                StartCoroutine(NewPath());
//            }
//            moving = true;
//        }
//        else if (Vector3.Distance(player.position, transform.position) < 4F)
//        {
//            //Stop!
//        }
//        else if (Vector3.Distance(player.position, transform.position) < 35F && moving)
//        {
//            if (Path.Count > 0)
//            {
//                if (Vector3.Distance(player.position, Path[Path.Count - 1]) > 5F)
//                {
//                    StartCoroutine(NewPath());
//                }
//            }
//            else
//            {
//                if (newPath)
//                {
//                    StartCoroutine(NewPath());
//                }
//            }
//            //Move the ai towards the player
//            MoveMethod();
//        }
//        else
//        {
//            moving = false;
//			Node n = Pathfinder.Instance.FindRealClosestNode(this.transform.position);
//			if(n.currentObject == null){
//				n.currentObject = gameObject;
////				n.walkable = false;
//
//			}
//			if(n.currentObject != gameObject){
////				Debug.Log("pas bon " + n.currentObject.ToString() + "contre " + gameObject.ToString());
//				StartCoroutine(NewPathToFreeSpot());
//			}
//        }
	}

    IEnumerator NewPath()
    {
        newPath = false;
        FindPath(transform.position, player.position);
        yield return new WaitForSeconds(1F);
        newPath = true;
    }

	IEnumerator NewPathToFreeSpot()
	{
		newPath = false;
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

            
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, Time.deltaTime * 12F);
            if (transform.position.x < Path[0].x + 0.4F && transform.position.x > Path[0].x - 0.4F && transform.position.z > Path[0].z - 0.4F && transform.position.z < Path[0].z + 0.4F)
            {
				previousNode = currentNode;
				if(previousNode != null && previousNode.currentObject == gameObject){
					previousNode.walkable = true;
					previousNode.currentObject = null;
				}
				Node n = Pathfinder.Instance.FindRealClosestNode(Path[0]);
				currentNode = n;//Pathfinder.Instance.Map[n.x, n.y];
//				if(currentNode.walkable == true && currentNode.currentObject == null){
					currentNode.walkable = false;
					currentNode.currentObject = gameObject;
//				} else {
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

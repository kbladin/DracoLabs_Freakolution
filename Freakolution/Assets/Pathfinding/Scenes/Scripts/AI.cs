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


	void Start () 
    {
        AIList = GameObject.FindGameObjectsWithTag("Enemy");
	}
	
	void Update () 
    {
        if (Vector3.Distance(player.position, transform.position) < 25F && !moving)
        {
            if (newPath)
            {
                StartCoroutine(NewPath());
            }
            moving = true;
        }
        else if (Vector3.Distance(player.position, transform.position) < 4F)
        {
            //Stop!
        }
        else if (Vector3.Distance(player.position, transform.position) < 35F && moving)
        {
            if (Path.Count > 0)
            {
                if (Vector3.Distance(player.position, Path[Path.Count - 1]) > 5F)
                {
                    StartCoroutine(NewPath());
                }
            }
            else
            {
                if (newPath)
                {
                    StartCoroutine(NewPath());
                }
            }
            //Move the ai towards the player
            MoveMethod();
        }
        else
        {
            moving = false;
        }
	}

    IEnumerator NewPath()
    {
        newPath = false;
        FindPath(transform.position, player.position);
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
				if(previousNode != null)
					previousNode.walkable = true;
				Node n = Pathfinder.Instance.FindRealClosestNode(Path[0]);
				currentNode = n;//Pathfinder.Instance.Map[n.x, n.y];
				currentNode.walkable = false;

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

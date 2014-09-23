using UnityEngine;
using System.Collections;

public class DistanceDetection : MonoBehaviour {

	public float distance;
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetButtonDown("Fire1"))
		{
			RaycastHit hit;
			Ray rayCast = new Ray(transform.position, Vector3.forward);
			
			
			if(Physics.Raycast(rayCast, out hit))
			{
				Debug.Log(hit.distance);
				
				//hit.collider.GetComponent<PlayerScripts
			}
		}
		
		
		
	}
}

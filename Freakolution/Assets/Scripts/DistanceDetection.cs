using UnityEngine;
using System.Collections;

public class DistanceDetection : MonoBehaviour {

	public float distance;
	ThirdPersonController controller;
	// Update is called once per frame
	void Awake(){
		controller = GetComponent<ThirdPersonController>();
	}
	
	void Update () {
	
		if(Input.GetButtonDown("Fire1"))
		{
			RaycastHit hit;
			Vector3 rayDirection = controller.GetDirection();
			Ray rayCast = new Ray(transform.position, rayDirection);
			
			
			if(Physics.Raycast(rayCast, out hit))
			{
				distance = hit.distance;
				Debug.Log(hit.distance);
				
				//hit.collider.GetComponent<PlayerScripts
			}
		}
		
		
		
	}
}

using UnityEngine;
using System.Collections;

public class Barrel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.RotateAround (transform.position, new Vector3(0,1,0), Random.Range(0f,360f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

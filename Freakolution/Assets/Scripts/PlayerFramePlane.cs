using UnityEngine;
using System.Collections;

public class PlayerFramePlane : MonoBehaviour {

	public GameObject referenceChemicalName;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (referenceChemicalName.GetComponent<TextMesh> ().text == "Redium") {
			GetComponent<MeshRenderer>().material.color = new Color(1f,0,0) * 0.5f;
		}
		else if (referenceChemicalName.GetComponent<TextMesh> ().text == "Greenogen") {
			GetComponent<MeshRenderer>().material.color = new Color(0,1f,0) * 0.5f;
		}
		else if (referenceChemicalName.GetComponent<TextMesh> ().text == "Bluorine") {
			GetComponent<MeshRenderer>().material.color = new Color(0,0,1f) * 0.5f;
		}
		else {
			GetComponent<MeshRenderer>().material.color = new Color(0.4f,0.4f,0.4f);
		}
	}
}

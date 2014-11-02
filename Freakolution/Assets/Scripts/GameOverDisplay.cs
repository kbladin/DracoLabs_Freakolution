using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameOverDisplay : MonoBehaviour {

	public GameObject scoreBoardElementPrefab;
	public GameObject PlanePrefab;
	private GameObject [] listObjects;

	//private ArrayList stats = new ArrayList();
	private List<PlayerStats> stats = new List<PlayerStats>();
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void AddPlayerStats(PlayerStats s) {
		stats.Add (s);
	}

	public void Display() {
		GameObject title = Instantiate(scoreBoardElementPrefab, transform.position, transform.rotation) as GameObject;
		title.transform.parent = transform;
		title.GetComponent<TextMesh> ().text = "Game Over";
		title.transform.localPosition += new Vector3 (0,4.5f,1f);;

		// Sort the bastard
		for (int i=0; i<stats.Count; ++i) {
			for (int j=i; j<stats.Count; ++j) {
				if(stats[j].playerNumber < stats[i].playerNumber) {
					PlayerStats tmp = stats[i];
					stats[i] = stats[j];
					stats[j] = tmp;
				}
			}
		}

		listObjects = new GameObject[stats.Count];
		for (int i=0; i<stats.Count; ++i) {
			listObjects[i] = Instantiate(scoreBoardElementPrefab, transform.position, transform.rotation) as GameObject;
			listObjects[i].transform.parent = transform;
			float xMultiplier = (i%2) - 0.5f;
			float yMultiplier = (i/2) - 0.5f;
			listObjects[i].transform.localPosition += new Vector3(
				8 * xMultiplier, -(3 * yMultiplier), 1f);
			listObjects[i].GetComponent<TextMesh>().text =
				"Player " + (stats[i].playerNumber + 1) + "\n" +
				"Kill count: "  + stats[i].killCount + "\n" + 
				"Damage done: " + stats[i].damageDone.ToString("0.0") + "\n" +
				"Damage taken: " + stats[i].damageTaken.ToString("0.0") + "\n" +
				"Barrels built: " + stats[i].barrelsBuilt + "\n" +
				"Barrels moved: " + stats[i].barrelsMoved + "\n";
			listObjects[i].GetComponent<TextMesh>().characterSize = 0.03f;

			GameObject plane = Instantiate(PlanePrefab, transform.position, transform.rotation) as GameObject;

			plane.transform.parent = transform;
			plane.transform.localRotation = Quaternion.Euler(new Vector3(-90f,0,0));
			plane.transform.localPosition = listObjects[i].transform.localPosition + new Vector3(0,-1.2f,1f);
			plane.transform.localScale = new Vector3(0.45f, 1f, 0.28f);

			Color planeColor = stats[i].chemicals.getChemicals() * 0.7f + (new Color(0.3f,0.3f,0.3f));
			planeColor.a = 0.7f;
			plane.GetComponent<MeshRenderer>().material.color = planeColor;
		}
	}
}

using UnityEngine;
using System.Collections;

public class EnemyCounter : MonoBehaviour {
	
	private int numOfEnemies;
	// Use this for initialization
	void Start () {
		numOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
		GetComponent<GUIText>().text = numOfEnemies.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		int newNumOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
		if(newNumOfEnemies != numOfEnemies)
		{
			numOfEnemies = newNumOfEnemies;
			GetComponent<GUIText>().text = numOfEnemies.ToString();
		}
			
	}
}

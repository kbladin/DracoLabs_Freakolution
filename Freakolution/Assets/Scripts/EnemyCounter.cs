using UnityEngine;
using System.Collections;

public class EnemyCounter : MonoBehaviour {
	
	private int numOfEnemies;

	void Start () {
		numOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
		GetComponent<GUIText>().text ="Scientists: " + numOfEnemies.ToString();
	}
	
	void Update () {
		int newNumOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
		if(newNumOfEnemies != numOfEnemies)
		{
			numOfEnemies = newNumOfEnemies;
			GetComponent<GUIText>().text ="Scientists: " + numOfEnemies.ToString();
		}
			
	}
}

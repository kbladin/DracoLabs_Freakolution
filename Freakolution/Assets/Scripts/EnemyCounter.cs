using UnityEngine;
using System.Collections;

public class EnemyCounter : MonoBehaviour {
	
	private int numOfEnemies;
	private int numOfEnemiesStart;
	private RandomSpawn spawnScript;
	
	private GUIText enemyCounterComponent;
	
	public int NumOfEnemies {get {return numOfEnemies;} set{numOfEnemies = value;}}

	void Start () {
		spawnScript = GetComponentInParent<RandomSpawn>();
		numOfEnemiesStart = spawnScript.SpawnsPerWave ;
		numOfEnemies = numOfEnemiesStart;
		
		enemyCounterComponent = GetComponent<GUIText>();
		
		enemyCounterComponent.text ="Scientists: " + numOfEnemies.ToString() + "/" + numOfEnemiesStart.ToString();
	}
	
	void Update () {
		
		if(numOfEnemiesStart != spawnScript.SpawnsPerWave)
		{
			numOfEnemiesStart = spawnScript.SpawnsPerWave;
			numOfEnemies = numOfEnemiesStart;
			enemyCounterComponent.text ="Scientists: " + numOfEnemies.ToString() + "/" + numOfEnemiesStart.ToString();
		}	
	}
	
	public void decreaseEnemyCount()
	{
		numOfEnemies = numOfEnemies -1;
		enemyCounterComponent.text ="Scientists: " + numOfEnemies.ToString() + "/" + numOfEnemiesStart.ToString();
	}
}

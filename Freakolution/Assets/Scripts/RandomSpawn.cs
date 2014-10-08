using UnityEngine;
using System.Collections;

public class RandomSpawn : MonoBehaviour {
		// Variables
		private float timer = 0.0f;
		private float timeLimit = 2.0f;
		private float spawnCount = 0.0f;
		private float maxSpawn = 10.0f;
		private float waveCounter = 1.0f;
		private float waveInterval = 10.0f;
				
		// Empty game objects with the spawn point locations
		public Transform[] spawns;
		
		// Location of the spawned enemy
		public Transform location;
		
		public GameObject enemyPrefab;
		
		// Is executed every frame
		void Update () {
			
			timer += Time.deltaTime;
			//Debug.Log(timer);
			if(timer >=timeLimit)
				Spawn();
			
		}


		
		void Spawn()
		{
			timer = 0;
			spawnCount++;
			
			//Will randomly pick a number between 0 and the size of spawns array
			//The length will be determined by the number of spawn elements in Unity
			int randomPick = Mathf.Abs(Random.Range(0,spawns.Length));
			location = spawns[randomPick];
			
			GameObject enemy = Instantiate(enemyPrefab, location.position, location.rotation) as GameObject;
			
			enemy.rigidbody.AddForce(location.forward * 100f);
			
			//Check if the correct amount of enemies for the wave has spawned already, then new wave.
			if (spawnCount == maxSpawn) {
				spawnCount = 0;
				waveCounter++;
				//Sets a new lower timeLimit with the increasing wave number so that enemies spawn faster with each wave
				timeLimit -= 0.2f;
				if(timeLimit < 0.01f)
					timeLimit = 0.01f;
				//Sets the time you got between the waves
				timer = -waveInterval;
			}

		}
	
}

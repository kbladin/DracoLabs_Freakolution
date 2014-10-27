using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomSpawn : MonoBehaviour {

		// Variables
		
		// A timer that will reset at the end of spawnInterval
		private float spawnIntervalTimer = 0.0f;
		// A timer that will reset every spawn cooldown
		private float spawnCooldownTimer = 0.0f;
		// A timer that will reset every waveInterval
		private float waveIntervalTimer = 0.0f;
		// THe number of spawned enemies each wave
		public int spawnCount = 10;
		// The number of waves that have passed (started)
		public int waveCounter = 0;
		// THe between waves
		public float waveInterval = 2.0f;
		// the time that enemies are spawned in a wave
		public float spawnInterval = 10.0f;
		// The time it takes between spawning enemies in a wave
		public float spawnCooldown = 0.5f;
				
		// Empty game objects with the spawn point locations
		public Transform[] spawns;
		
		public List<GameObject> enemies;
		
		// Location of the latest spawned enemy
		public Transform location;
		
		//The gameObject that will be spawned
		public GameObject enemyPrefab;
				
		public GameObject player;
		void Start () {
//			spawnIntervalTimer = 0.0f;
//			spawnCooldownTimer = 0.0f;
//			waveIntervalTimer = 0.0f;
//			spawnCooldown = 1.0f;
//			spawnCount = 10;
//			waveCounter = 1;
//			waveInterval = 10.0f;
//			spawnInterval = 20.0f;
		}
		
		// Is executed every frame
		void Update () {
			
			
			if(spawnIntervalTimer < spawnInterval)
			{
				spawnIntervalTimer += Time.deltaTime;
				spawnCooldownTimer += Time.deltaTime;
				
				if(spawnCooldownTimer >=spawnCooldown)
				{	
					Spawn();
					spawnCooldownTimer = 0.0f;
					spawnCount++;
				}
						
			}
			else
			{
				//Debug.Log(enemies.Count);
				GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
				if(enemy.Length == 0){
					//wait for next wave
					waveIntervalTimer += Time.deltaTime;
					if(waveIntervalTimer > waveInterval)
					{
						waveCounter++;
						spawnIntervalTimer=0.0f;
						spawnCount = 0;
						spawnCooldown *= 0.8f; 
						waveIntervalTimer = 0.0f;
				}
				}
				
			}
			
		}


		
		void Spawn()
		{		
			//Will randomly pick a number between 0 and the size of spawns array
			//The length will be determined by the number of spawn elements in Unity
			int randomPick = Mathf.Abs(Random.Range(0,spawns.Length));
			location = spawns[randomPick];
			enemyPrefab.GetComponent<AI>().setPlayer( player);
			GameObject enemy = Instantiate(enemyPrefab, location.position, location.rotation) as GameObject;
			
//			enemy.rigidbody.AddForce(location.forward * 100f);
		}
	
}

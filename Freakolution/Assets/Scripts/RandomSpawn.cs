using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomSpawn : MonoBehaviour {

		// Variables
		
		// A timer that will reset at the end of spawnInterval
		public float spawnIntervalTimer;
		// A timer that will reset every spawn cooldown
		public float spawnCooldownTimer;
		// A timer that will reset every waveInterval
		public float waveIntervalTimer;
		// THe number of spawned enemies each wave
		private int spawnCount;
		// The number of waves that have passed (started)
		public int waveCounter;
		// THe between waves
		private float waveInterval;
		// the time that enemies are spawned in a wave
		private float spawnInterval;
		// The time it takes between spawning enemies in a wave
		private float spawnCooldown;
				
		// Empty game objects with the spawn point locations
		public Transform[] spawns;
		
		public List<GameObject> enemies;
		
		// Location of the latest spawned enemy
		public Transform location;
		
		//The gameObject that will be spawned
		public GameObject enemyPrefab;
				
		public GameObject player;
		void Start () {
			spawnIntervalTimer = 0.0f;
			spawnCooldownTimer = 0.0f;
			waveIntervalTimer = 0.0f;
			spawnCooldown = 5.0f;
			spawnCount = 0;
			waveCounter = 1;
			waveInterval = 10.0f;
			spawnInterval = 20.0f;
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
			//enemyPrefab.GetComponent<AI>().setPlayer( player);
			GameObject enemy = Instantiate(enemyPrefab, location.position, location.rotation) as GameObject;
			
			//enemy.rigidbody.AddForce(location.forward * 100f);
		}
	
}

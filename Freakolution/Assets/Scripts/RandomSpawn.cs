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
		// A counter for how many enemies should spawn
		private int numberOfSpawns = 0;
		// THe number of spawned enemies each wave
		public int spawnsPerWave = 5; //Initiate to 5 enemies for wave 1, should be changed to depend on the nr of players
		// THe time between waves
		public float waveInterval = 10.0f;
		// the time that enemies are spawned in a wave
		public float spawnInterval = 20.0f;
		// The time it takes between spawning enemies in a wave
		public float spawnCooldown;
		// The number of waves that have passed (started)
		private int waveCounter = 1;				
		// Empty game objects with the spawn point locations
		public Transform[] spawns;
		//The factor for how the number of enemies increase each wave
		private float waveFactor = 0.2f;
		
		public List<GameObject> enemies;
		
		// Location of the latest spawned enemy
		public Transform location;
		
		//The gameObject that will be spawned
		public GameObject enemyPrefab;

		public GameObject blockPrefab;
		public int numberOfStartingBlocks;
		
		//properties
		public int WaveCounter {get{return waveCounter;} set{waveCounter = value;}}
		public int NumberOfSpawns {get{return numberOfSpawns;} set{numberOfSpawns=value;}}
		public int SpawnsPerWave {get{return spawnsPerWave;} set{spawnsPerWave=value;}}
				
		public GameObject player;
		void Start () {

		SpawnBlocks ();
		spawnCooldown = spawnInterval / spawnsPerWave;
		}
		
		// Is executed every frame
		void Update () {
			
			//If the time since start of the wave is smaller than the total time for
			if(spawnIntervalTimer < spawnInterval)
			{
				spawnIntervalTimer += Time.deltaTime;
				spawnCooldownTimer += Time.deltaTime;
				//If the time between spawns > spawncooldown, spawn..
				if(spawnCooldownTimer >=spawnCooldown)
				{	
					Spawn();
					spawnCooldownTimer = 0.0f;
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
						//Number of spawns increase for each wave by a factor
						spawnsPerWave += (int)(waveFactor*spawnsPerWave);
						waveCounter++;
						spawnIntervalTimer=0.0f;
						numberOfSpawns = 0;
						//The cooldown between each spawn depends on number of spawns and total spawn time
						spawnCooldown = spawnInterval / spawnsPerWave;
						waveIntervalTimer = 0.0f;
				}
				}
				
			}
			
		}


		
		void Spawn()
		{		
			//int numOfPlayers = GameObject.FindGameObjectsWithTag("Player").Length;
			//int randomChemicalPick = Mathf.Abs(Random.Range(0,numOfPlayers-1));
			//Will randomly pick a number between 0 and the size of spawns array
			//The length will be determined by the number of spawn elements in Unity
			int randomSpawnPick = Mathf.Abs(Random.Range(0,spawns.Length));
			location = spawns[randomSpawnPick];
			//enemyPrefab.GetComponent<AI>().setPlayer( player);
			GameObject enemy = Instantiate(enemyPrefab, location.position, location.rotation) as GameObject;
			//Increment number of spawns
			numberOfSpawns++;
			//enemy.GetComponent<Enemy>().SetChemicals(new Chemicals());
			//enemy.rigidbody.AddForce(location.forward * 100f);
		}

	void SpawnBlocks() {
		for (int i=0; i<numberOfStartingBlocks; ++i) {
			Vector3 randPos = new Vector3(Random.Range(-17.5f, 17.5f), 0.5f,Random.Range(-19f, -10.5f));
			Node n = Pathfinder.Instance.FindRealClosestNode(randPos);

			if(n.walkable) {
				Vector3 realLoc = new Vector3(n.xCoord, n.yCoord + 0.5f,n.zCoord);
				Instantiate(blockPrefab, realLoc, new Quaternion(0,0,0,0));
				n.walkable = false;
			}
			else {--i;}

		}
	}
	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomSpawn : MonoBehaviour {

		// Variables
		
		// A timer that will reset every spawn cooldown
		private float spawnCooldownTimer = 0f;
		// A timer that will reset every waveInterval
		private float waveIntervalTimer = 0.0f;
		// A counter for how many enemies should spawn
		private int numberOfSpawns = 0;
		// THe number of spawned enemies each wave
		private int spawnsPerWave = 0;// Should be set to 0 in the beginning
		
	private int spawnsOnFirstWave = 5;
		// THe time between waves
		private float waveInterval = 15.0f;
		// the time that enemies are spawned in a wave
		public float spawnInterval;// = 20.0f;
		// The time it takes between spawning enemies in a wave
		private float spawnCooldown;
		// The number of waves that have passed (started)
		private int waveCounter = 0;				
		// Empty game objects with the spawn point locations
		public Transform[] spawns;
		//The factor for how the number of enemies increase each wave
		private float waveFactor = 0.2f;
		
		private bool waveStarted=false;
		
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
				
		// Audio
		public AudioClip alarmAudio;

		public GameObject player;
		void Start () {

		SpawnBlocks ();
		}
		
		// Is executed every frame
		void Update () {
		
			
			if(waveIntervalTimer < waveInterval)
			{
				waveIntervalTimer += Time.deltaTime;
			}
			//Check if waveinterval timer is below waveinterval
			else if(!waveStarted)
			{
				waveStarted = true;
				waveCounter++;
				InitiateWave(waveCounter);
				//Sound horn
				AudioSource.PlayClipAtPoint(alarmAudio, transform.position);
		}
		
			else if(numberOfSpawns < spawnsPerWave)
			{
				//spawn enemies
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
				GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
				if(enemy.Length == 0)
				{
					numberOfSpawns = 0;
					//The cooldown between each spawn depends on number of spawns and total spawn time
					spawnCooldown = spawnInterval / spawnsPerWave;
					waveIntervalTimer = 0.0f;
					waveStarted = false;
				}
			}
		}


		
		void Spawn()
		{		
			//Will randomly pick a number between 0 and the size of spawns array
			//The length will be determined by the number of spawn elements in Unity
			int randomSpawnPick = Mathf.Abs(Random.Range(0,spawns.Length));
			location = spawns[randomSpawnPick];
			GameObject enemy = Instantiate(enemyPrefab, location.position, location.rotation) as GameObject;
			//Increment number of spawns
			numberOfSpawns++;
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
	
	void InitiateWave(int waveCounter)
	{
		if(waveCounter == 1)
		{
			spawnsPerWave = spawnsOnFirstWave;
		}
		else
		{
			spawnsPerWave += (int)(waveFactor*spawnsPerWave);
		}
		spawnCooldown = spawnInterval / spawnsPerWave;
		
	}
	
}

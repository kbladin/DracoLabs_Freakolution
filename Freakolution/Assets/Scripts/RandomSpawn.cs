using UnityEngine;
using System.Collections;

public class RandomSpawn : MonoBehaviour {
		
		public float timer = 0.0f;
		
		// Empty game objects with the spawn point locations
		public Transform[] spawns;
		
		// Location of the spawned enemy
		public Transform location;
		
		public GameObject enemyPrefab;
		public GameObject player;

		void Update () {
			
			timer += Time.deltaTime;
			//Debug.Log(timer);
			if(timer >=2)
				
				Spawn();	
		}
		
		void Spawn()
		{
			timer = 0;
			
			//Will randomly pick a number between 0 and the size of spawns array 
			int randomPick = Mathf.Abs(Random.Range(0,spawns.Length));
			location = spawns[randomPick];
			enemyPrefab.GetComponent<AI>().setPlayer( player);
			GameObject enemy = Instantiate(enemyPrefab, location.position, location.rotation) as GameObject;
			
			enemy.rigidbody.AddForce(location.forward * 100f);
//		AI aiScript  = 
//			aiScript;
		}
	
}

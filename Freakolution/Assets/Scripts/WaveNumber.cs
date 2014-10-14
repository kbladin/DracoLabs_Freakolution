using UnityEngine;
using System.Collections;

public class WaveNumber : MonoBehaviour {
	GameObject ground;
	RandomSpawn randomSpawn;
	// Use this for initialization
	void Start () {
		ground = GameObject.Find("Ground");
		randomSpawn = ground.GetComponent<RandomSpawn>();
	}
	
	// Update is called once per frame
	void Update () {
		
		GetComponent<TextMesh>().text = randomSpawn.waveCounter.ToString();;
		
	}
}

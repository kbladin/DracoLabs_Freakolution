using UnityEngine;
using System.Collections;

public class WaveNumber : MonoBehaviour {

	private RandomSpawn randomSpawn;
	private int waveNumber;
	// Use this for initialization
	void Start () {
		
		randomSpawn = GetComponentInParent<RandomSpawn>();
		waveNumber = randomSpawn.WaveCounter;
		GetComponent<GUIText>().text = waveNumber.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(waveNumber != randomSpawn.WaveCounter)
		{
			waveNumber = randomSpawn.WaveCounter;
			GetComponent<GUIText>().text = waveNumber.ToString();
		}
		
		
	}
}

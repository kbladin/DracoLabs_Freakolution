using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
* This class will handle things like playing music, Pause function,
* detecting when game is over... and such
*/
public class GameManager : MonoBehaviour {
	
	private bool pause = false;
	public GUITexture pauseGUI;
	public GUITexture gameOverGUI;
	
	public Transform[] playerSpawns;
	public GameObject playerPrefab;
	private List<GameObject> players;
	
	private int numOfPlayers = 2;
	
	void Start () {
		players = new List<GameObject>();
		for(int i = 0; i<numOfPlayers;i++)
		{
			GameObject player = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation) as GameObject;
			player.GetComponent<ThirdPersonController>().SetPlayerNumber(i);
			players.Add (player);
		}
		
		pauseGUI.enabled = false;
		gameOverGUI.enabled = false;
		//players = GameObject.FindGameObjectsWithTag("Player");
	
	}
	
	void Update () {
	

		if(!gameOverGUI.enabled && Input.GetKeyUp(KeyCode.Escape)) 
		{
			pause = !pause;
		}
		
		if(pause) {
			Time.timeScale = 0.0f;
			pauseGUI.enabled = true;
		}
		else {
			Time.timeScale = 1.0f;
			pauseGUI.enabled = false;
		}
		
		if(!isPlayersAlive())
		{
			//Game over... bitch
			// load main menu.
			gameOver();
		}
		
		if(gameOverGUI.enabled && Input.GetKeyUp(KeyCode.Escape))
		{
			Application.LoadLevel(0);
		}
	
	}
	
	private bool isPlayersAlive(){
	
		bool someoneIsAlive = false;
		Player playerScriptComponent;
		for(int i=0; i<players.Count; i++)
		{
			playerScriptComponent = players[i].GetComponent<Player>();
			if(playerScriptComponent.Alive)
			{
				someoneIsAlive = true;
				break;
			}
		}
		return someoneIsAlive;
	}
	
	
	private void gameOver(){
		
		Time.timeScale=0.0f;
		gameOverGUI.enabled = true;
	}
	
}	
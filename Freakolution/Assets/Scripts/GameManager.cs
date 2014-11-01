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
	public Texture waveHUDTex;
	
	private int numOfPlayers = 2;
	
	void Awake () {
	
		players = new List<GameObject>();
		numOfPlayers = GameVariables.nPlayers;
//		print("n players = " + );
		for(int i=0;i<numOfPlayers;i++){
//			print("player "+i+" playing = " + GameVariables.playersPlaying[i]);
//			print("player "+i+" class = " + GameVariables.playerClasses[i]);
//			print("player "+i+" chemical = " + GameVariables.playerChemicals[i]);
			int chemical = 0;
			if(GameVariables.playerChemicals != null)
				chemical = GameVariables.playerChemicals[i];


			GameObject player = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation) as GameObject;
			player.GetComponent<ThirdPersonController>().SetPlayerNumber(i);
			player.GetComponent<Player>().SetChemicals(new Chemicals(chemical));
//			player.transform.Find("CharacterSprite").transform.Find("PlayerNumberBack").Find("PlayerNumberText").GetComponent<TextMesh>().text = (i+1).ToString();
			player.transform.Find("CharacterSprite").transform.Find("PlayerNumberText").GetComponent<TextMesh>().text = (i+1).ToString();
			//			player.transform.Find("PlayerNumberText").GetComponent<TextMesh>().text = (i+1).ToString();

			players.Add (player);



		}



//		for(int i = 0; i<numOfPlayers;i++)
//		{	
//			
//			GameObject player = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation) as GameObject;
//			player.GetComponent<ThirdPersonController>().SetPlayerNumber(i);
//			player.GetComponent<Player>().SetChemicals(new Chemicals(i));
//			players.Add (player);
//		}
	
	}
	
	void Start ()
	{
		pauseGUI.enabled = false;
		gameOverGUI.enabled = false;	
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
		
		if(!isPlayersAlive() && !gameOverGUI.enabled)
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
	
//	void OnGUI()
//	{
//		GUI.Box (new Rect (Screen.width/2 - 50,0,100,50), new GUIContent(waveHUDTex));
//	}
	
}	
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
* This class will handle things like playing music, Pause function,
* detecting when game is over... and such
*/
public class GameManager : MonoBehaviour {
	
	//private bool pause = false;
	private bool gameOverBool = false;
	public GameObject pauseMenu;
	//public GUITexture gameOverGUI;
	public GameObject gameOverDisplay;
	
	public Transform[] playerSpawns;
	public GameObject playerPrefab;
	private List<GameObject> players;
	public Texture waveHUDTex;
	
	public Texture2D healerSelector;
	public Texture2D tankSelector;
	public Texture2D engineerSelector;
	public Texture2D marksmanSelector;
	
	private int numOfPlayers = 4;
	
	void Awake () {
		players = new List<GameObject>();
		numOfPlayers = GameVariables.nPlayers;
//		print("n players = " + );
		for(int i=0;i<4;i++){
//			print("player "+i+" playing = " + GameVariables.playersPlaying[i]);
//			print("player "+i+" class = " + GameVariables.playerClasses[i]);
//			print("player "+i+" chemical = " + GameVariables.playerChemicals[i]);
			int chemical = 0;
			if(GameVariables.playerChemicals != null)
				chemical = GameVariables.playerChemicals[i];


			if(GameVariables.playersPlaying[i]){

				GameObject player = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation) as GameObject;
				player.GetComponent<Player>().SetChemicals(new Chemicals(chemical));
				player.GetComponent<Player>().stats.playerNumber = i;
				player.transform.Find("CharacterSprite").transform.Find("PlayerNumberText").GetComponent<TextMesh>().text = (i+1).ToString();
				player.GetComponent<Player>().SetPlayerClass(GameVariables.playerClasses[i]);
				player.GetComponent<HealthGlobe>().SetPlayerNumber(i);

				
				if(GameVariables.playerClasses[i] == "Healer")
					player.transform.Find("CharacterSprite").transform.Find("PlayerNumberBack").renderer.material.mainTexture = healerSelector;
				else if(GameVariables.playerClasses[i] == "Tank")
					player.transform.Find("CharacterSprite").transform.Find("PlayerNumberBack").renderer.material.mainTexture = tankSelector;
				else if(GameVariables.playerClasses[i] == "Engineer")
					player.transform.Find("CharacterSprite").transform.Find("PlayerNumberBack").renderer.material.mainTexture = engineerSelector;
				else if(GameVariables.playerClasses[i] == "Marksman")
					player.transform.Find("CharacterSprite").transform.Find("PlayerNumberBack").renderer.material.mainTexture = marksmanSelector;
				
				if(GameVariables.keyboardPlayer == i){
					player.GetComponent<ThirdPersonController>().SetPlayerNumber(4);
				} else {
					player.GetComponent<ThirdPersonController>().SetPlayerNumber(i);
				}
				
				players.Add (player);
			}	
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
		pauseMenu.GetComponent<PauseMenu>().GamePaused=false;
		gameOverBool = false;

	}
	
	void Update () {
	
		if(!gameOverBool && (Input.GetKeyUp(KeyCode.Escape) || Input.GetButtonUp("P_Button7"))) 
		{
			if(pauseMenu.GetComponent<PauseMenu>().GamePaused)
			{
				pauseMenu.GetComponent<PauseMenu>().UnPauseGame();
			}
			else
			{
				pauseMenu.GetComponent<PauseMenu>().PauseGame();
			}
		}
		
		if(!isPlayersAlive() && !gameOverBool)
		{
			gameOver();
		}
		
		if(gameOverBool && (Input.GetKeyUp(KeyCode.Escape) || Input.GetAxis("P_Button7")>0))
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
		gameOverDisplay.GetComponent<GameOverDisplay> ().Display ();
		Time.timeScale=0.0f;
		gameOverBool = true;
	}
	
//	void OnGUI()
//	{
//		GUI.Box (new Rect (Screen.width/2 - 50,0,100,50), new GUIContent(waveHUDTex));
//	}
	
}	

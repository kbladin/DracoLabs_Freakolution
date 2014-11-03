using UnityEngine;
using System.Collections;

public class HealthGlobe : MonoBehaviour {

	private float globeHeight;
	public Texture globePic;
	public Texture globeFrame;
	public Texture killCounter;
	//The variables for setting the position of GUI HUD for this player
	private float leftPosition;
	private float topPosition;
	public int globeSize;
	public int counterSize;
	private int charNumber;
	private float counterPositionX;
	private float counterPositionY;
	private string enemiesKilled;
	public GUIStyle theStyle;
	public GameObject killFrame;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnGUI(){

		globeSize = (Screen.height) / 8;
		counterSize = globeSize / 2;
		theStyle.fontSize = counterSize/3;
		//Get percentage of health, [0,1]
		float scaleHealth = GetComponentInParent<Player> ().GetHealth () / GetComponentInParent<Player> ().MaxHealth;
		globeHeight = scaleHealth * globeSize;
		SetPosition (charNumber);
		//Rect(left: float, top: float, width: float, height: float)

		enemiesKilled = (GetComponentInParent<Player> ().getNumberOfKills ()).ToString ();
		//killCounter.renderer.material.SetColor("_Color",Color(0,0,0,0.5)) ;
		float r = Mathf.Min(2 * (1 - scaleHealth), 1f);
		float g = Mathf.Min(2 * scaleHealth, 1f);


		/*Original
		GUI.BeginGroup (new Rect (leftPosition, topPosition, (globeSize+20), (globeSize+20)));
			//Creating a group for the health globe
			GUI.BeginGroup (new Rect(0, -globeHeight+globeSize, globeSize,globeSize));
				GUI.color = new Color (r, g, 0);
				GUI.DrawTexture (new Rect(0,-globeSize+globeHeight, globeSize, globeSize),globePic);
				GUI.color = new Color (1, 1, 1);
			GUI.EndGroup ();
		Rect textureRec = new Rect(counterPosition,counterSize-10, counterSize, counterSize);
		GUI.color = GetComponentInParent<Player> ().getChemicals ();
		GUI.DrawTexture(textureRec ,killCounter);
		GUI.color = new Color (0, 0, 0, 1);
		//killFrame.transform.position = new Vector2 (counterPosition, counterSize - 10);
		GUI.Label (new Rect (counterPosition+36-(enemiesKilled.Length*2), counterSize+16, 30, 30), enemiesKilled, theStyle);

		GUI.EndGroup ();*/


		GUI.BeginGroup (new Rect (leftPosition, topPosition, (globeSize+20), (globeSize+20)));
		//Creating a group for the health globe
		GUI.BeginGroup (new Rect(0, -globeHeight+globeSize, globeSize,globeSize));
		GUI.color = new Color (r, g, 0);
		GUI.DrawTexture (new Rect(0,-globeSize+globeHeight, globeSize, globeSize),globePic);
		GUI.color = new Color (1, 1, 1);
		GUI.EndGroup ();
		GUI.DrawTexture (new Rect(0,0, globeSize, globeSize),globeFrame);
		Rect textureRec = new Rect(counterPositionX,counterPositionY, counterSize, counterSize);
		GUI.color = GetComponentInParent<Player> ().PlayerChemicals.getChemicals();
		GUI.DrawTexture(textureRec ,killCounter);
		GUI.color = new Color (0, 0, 0, 1);
		//killFrame.transform.position = new Vector2 (counterPosition, counterSize - 10);
		GUI.Label (new Rect (counterPositionX+(theStyle.fontSize*1.5f)-(theStyle.fontSize*enemiesKilled.Length/3.7f), counterPositionY+(theStyle.fontSize/1.2f), counterSize, counterSize), enemiesKilled, theStyle);
		
		GUI.EndGroup ();
		
		
	}
	public void SetPlayerNumber(int i){
		charNumber = i;
	}

	void SetPosition(int i){
		//If top left of the screen
		if (i == 0) {
			leftPosition = 20;
			//topPosition = -globeHeight+(globeSize);
			topPosition = 0;
			counterPositionX = counterSize;
			counterPositionY = counterSize;
		}
		//If top right of the screen
		else if(i == 1){
			leftPosition = Screen.width-globeSize-20;
			//topPosition = -globeHeight+(globeSize);
			topPosition = 0;
			counterPositionX = 0;
			counterPositionY = counterSize;
		}
		//Bottom left of the screen
		else if(i == 2){
			leftPosition = 20;
			topPosition = Screen.height-globeSize-20;
			counterPositionX = counterSize;
			counterPositionY = counterSize;
		}
		//If bottom right of the screen
		else if(i == 3){
			leftPosition = Screen.width-globeSize-20;
			topPosition = Screen.height-globeSize-20;
			counterPositionX = 0;
			counterPositionY = counterSize;
		}


	}
}

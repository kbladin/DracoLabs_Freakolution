using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public GameObject[] menuButtons = null;
	
	private bool gamePaused = false;
	public bool GamePaused {get{return gamePaused;} set{gamePaused = value;}}
	
	private int nControllers = 0;
	// Use this for initialization
	void Start () {
	
		string[] joysticks = Input.GetJoystickNames();
		nControllers = joysticks.Length;

	}
	
	// Update is called once per frame
	void Update () {
	
		if(gamePaused)
		{
			int input = getJoystickInput();	
			
		}
		
	}
	
	private int getJoystickInput()
	{
		for (int i = 1; i <= nControllers; i++) {
			if (Mathf.Abs(Input.GetAxis("P"+i+"_Vertical")) > 0.2)
				Debug.Log (Input.GetJoystickNames()[i]+" is moved");
		}
	return 1;
	}
	
	public void PauseGame()
	{
		Time.timeScale = 0.0f;
		
		//enable background
		GetComponent<GUITexture>().enabled = true;
		
		//enable menu text
		GUIText[] menuTexts = GetComponentsInChildren<GUIText>();
		for(int i = 0; i < menuTexts.Length; i++)
		{
			menuTexts[i].enabled = true;
		}
		
		GamePaused = true;
	}
	
	public void UnPauseGame()
	{
		Time.timeScale = 1.0f;
		//enable background
		GetComponent<GUITexture>().enabled = false;
		
		//enable menu text
		GUIText[] menuTexts = GetComponentsInChildren<GUIText>();
		for(int i = 0; i < menuTexts.Length; i++)
		{
			menuTexts[i].enabled = false;
		}
		
		GamePaused = false;
	}
}

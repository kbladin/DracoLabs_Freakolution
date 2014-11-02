using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public GameObject[] menuButtons = null;
	private int selectedMenuButton;
	
	private bool gamePaused = false;
	public bool GamePaused {get{return gamePaused;} set{gamePaused = value;}}

	int[] previousJoystickInputs;
	int previousKeyboardInput;
	
	private int nControllers = 0;
	// Use this for initialization
	void Start () {
	
		selectedMenuButton=0;
		string[] joysticks = Input.GetJoystickNames();
		nControllers = joysticks.Length;
		
		previousKeyboardInput = 0;
	
		previousJoystickInputs = new int[nControllers];	
		for(int i = 0; i< previousJoystickInputs.Length; i++) 
		{
			previousJoystickInputs[i] = 0;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		if(gamePaused)
		{
		
			for(int i = 1; i <= nControllers; i++)
			{
				int joystickInput = getJoystickInput(i);
				if(previousJoystickInputs[i-1] == 0){
					selectedMenuButton += joystickInput;
					
					if(selectedMenuButton < 0)
					{
						selectedMenuButton = menuButtons.Length-1;
					}
					else if(selectedMenuButton > menuButtons.Length-1)
					{
						selectedMenuButton = 0;
					}
				}
				previousJoystickInputs[i-1] = joystickInput;
			}	
			
			int keyboardInput = getKeyboardInput();
			
			if(previousKeyboardInput == 0){
				selectedMenuButton += keyboardInput;
				
				if(selectedMenuButton < 0)
				{
					selectedMenuButton = menuButtons.Length-1;
				}
				else if(selectedMenuButton > menuButtons.Length-1)
				{
					selectedMenuButton = 0;
				}
			}
			previousKeyboardInput=keyboardInput;
			
			HighLightSelected();
			
			if (AnyonePressedButton0())
			{
				if(selectedMenuButton == 0)
				{
					UnPauseGame();
					
				}
				if(selectedMenuButton == 1)
				{
					UnPauseGame();
					Application.LoadLevel(0);
				}
			}
			
		}
		
		
	}
	
	private int getJoystickInput(int joystick)
	{
		float verticalInput = Input.GetAxisRaw("P"+joystick+"_Vertical") ;
		if (verticalInput > 0.2f)
		{
			return Mathf.CeilToInt(verticalInput);
		}
		else if(verticalInput < -0.2f)
		{
			return Mathf.FloorToInt(verticalInput);
		}
		return 0;
	}
	
	private int getKeyboardInput()
	{
		float verticalInput = Input.GetAxisRaw("P1Alt_Vertical") ;
		if (verticalInput > 0.2f)
		{
			return Mathf.CeilToInt(verticalInput);
		}
		else if(verticalInput < -0.2f)
		{
			return Mathf.FloorToInt(verticalInput);
		}
		return 0;
	}
	
	private void HighLightSelected()
	{
		for(int i = 0; i<menuButtons.Length; i++)
		{
			if(i == selectedMenuButton)
			{
				menuButtons[i].GetComponent<GUIText>().color = new Color(1,1,1);
			}
			else
			{
				menuButtons[i].GetComponent<GUIText>().color = new Color(0,0.7f,0);
			}
			
		}
		
	}
	public bool AnyonePressedButton0()
	{
		for (int i = 1; i <= nControllers; i++)
		{
			if(Input.GetButtonUp("P" + i.ToString() + "_Button0"))
				return true;
		}
		
		if(Input.GetButtonUp("P1Alt_Button0"))
			return true;
			
		return false;
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

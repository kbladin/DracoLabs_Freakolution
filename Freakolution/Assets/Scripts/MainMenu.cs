using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MainMenu : MonoBehaviour
{
	enum MoveType {Time, Speed}

	static public MainMenu instance; //the instance of our class that will do the work


	public GameObject mainMenu = null;
	public GameObject playMenu = null;
	public GameObject settingsMenu = null;
	public GameObject menuSelect = null;
	
	public float ScreensTransitionTime = 0.5f;

	//constants
	private static int SETTINGS_MENU = 0;
	private static int MAIN_MENU = 1;
	private static int PLAY_MENU = 2;
//	private Vector3 centerOfScreenPos = new Vector3(0,0,0);
	private Vector3 leftOfScreenPos = new Vector3(-200,0,0);
//	private Vector3 leftOfLeftOfScreenPos = new Vector3(-400,0,0);
	private Vector3 rightOfScreenPos = new Vector3(200,0,0);
//	private Vector3 rightOfRightOfScreenPos = new Vector3(400,0,0);
	  
	//variables
	private GameObject[] menuButtons;
	private GameObject[] menuButtonsP1;
	private GameObject[] menuButtonsP2;
	private GameObject[] menuButtonsP3;
	private GameObject[] menuButtonsP4;

	private GameObject currentSelect = null;
	private GameObject currentSelect1 = null;
	private GameObject currentSelect2 = null;
	private GameObject currentSelect3 = null;
	private GameObject currentSelect4 = null;

	private GameObject menuSelect1 = null;
	private GameObject menuSelect2 = null;
	private GameObject menuSelect3 = null;
	private GameObject menuSelect4 = null;

	private GameObject playerFrame1;
	private GameObject playerFrame2;
	private GameObject playerFrame3;
	private GameObject playerFrame4;

	private string[] classes = {"Tank","Engineer","Marskman","Healer"};
	private string[] classDescs = {"Melee","Melee","Range","Range"};
	private string[] chemicals = {"Redium","Greenogen","Bluorine"};

	private bool[] playerPlaying = {false, false, false, false};
	private bool[] playerReady = {false, false, false, false};
	private int[] playerClass;
	private int[] playerChemical;

	private int currentScreen = 0;

	private bool isMusicOn = true;
	private bool isSFXOn = true;

	private int nControllers = 0;

	private float debounce1v = 0.0f;
	private float debounce1h = 0.0f;
	private float debounce2v = 0.0f;
	private float debounce2h = 0.0f;
	private float debounce3v = 0.0f;
	private float debounce3h = 0.0f;
	private float debounce4v = 0.0f;
	private float debounce4h = 0.0f;


	void Awake() {
		instance = this;
	}

	void Start(){
		currentScreen = MAIN_MENU;//PLAY_MENU;//
	
		playerClass = new int[4];
		playerClass[0] = 0;
		playerClass[1] = 0;
		playerClass[2] = 0;
		playerClass[3] = 0;

		playerChemical = new int[4];
		playerChemical[0] = 0;
		playerChemical[1] = 0;
		playerChemical[2] = 0;
		playerChemical[3] = 0;

		menuSelect1 = Instantiate(menuSelect) as GameObject;
		menuSelect2 = Instantiate(menuSelect) as GameObject;
		menuSelect3 = Instantiate(menuSelect) as GameObject;
		menuSelect4 = Instantiate(menuSelect) as GameObject;




		string[] joysticks = Input.GetJoystickNames();
		nControllers = joysticks.Length;
		print(nControllers);


		RefreshMenuButtons();
		GetPlayerFrames();

		ActivatePlayerFrames();
//		foreach(string s in joysticks){
//			print (s);
//		}
	}

	private void GetPlayerFrames(){
		GameObject[] frames = GameObject.FindGameObjectsWithTag("PlayerFrame");

		foreach(GameObject frame in frames){
//			print (frame.name);
			frame.transform.Find("Class").GetComponent<TextMesh>().text = "";
			frame.transform.Find("Desc").GetComponent<TextMesh>().text = "Not Playing";
			frame.transform.Find("Chemical").GetComponent<TextMesh>().text = "";
			frame.transform.Find("Class").gameObject.SetActive(false);
			frame.transform.Find("Chemical").gameObject.SetActive(false);

			if(frame.name == "Player1Frame"){
				playerFrame1 = frame;
			} else if(frame.name == "Player2Frame"){
				playerFrame2 = frame;
			} else if(frame.name == "Player3Frame"){
				playerFrame3 = frame;
			} else if(frame.name == "Player4Frame"){
				playerFrame4 = frame;
			}
		}

	}

	private void ActivatePlayerFrames(){
		if(nControllers == 0){
			playerFrame2.SetActive(false);
			playerFrame3.SetActive(false);
			playerFrame4.SetActive(false);
		} else if(nControllers == 1){
			playerFrame3.SetActive(false);
			playerFrame4.SetActive(false);
		} else if(nControllers == 2){
			playerFrame4.SetActive(false);
		} else if(nControllers == 3){
		} else if(nControllers == 4){
			
		}

	}

	public void ButtonPressed(string btnName, GameObject myCurrentSelect, int player){

		print (btnName);

		int previousScreen = currentScreen;
		if (btnName == "Play"){
			currentScreen = PLAY_MENU;
		} else if (btnName == "Settings"){
			currentScreen = SETTINGS_MENU;
		} else if (btnName == "Back"){
			currentScreen = MAIN_MENU;
		} else if (btnName == "Portrait"){
			int playerNumber = myCurrentSelect.GetComponent<MenuButtonScript>().playerNumber;
			GameObject frame = null;
			if(playerNumber == 1){
				frame = playerFrame1;
			} else if(playerNumber == 2){
				frame = playerFrame2;
			} else if(playerNumber == 3){
				frame = playerFrame3;
			} else if(playerNumber == 4){
				frame = playerFrame4;
			}
			frame.transform.Find("PortraitCoin").GetComponent<PortraitCoin>().FlipCoin();
			if(frame.transform.Find("PortraitCoin").GetComponent<PortraitCoin>().facingCamera){
				frame.transform.Find("Class").GetComponent<TextMesh>().text = classes[playerClass[playerNumber-1]];
				frame.transform.Find("Desc").GetComponent<TextMesh>().text = classDescs[playerNumber-1];
				frame.transform.Find("Chemical").GetComponent<TextMesh>().text = chemicals[playerChemical[playerNumber-1]];
				frame.transform.Find("Class").gameObject.SetActive(true);
				frame.transform.Find("Chemical").gameObject.SetActive(true);
				playerPlaying[playerNumber-1] = true;
			} else {
				frame.transform.Find("Class").gameObject.SetActive(false);
				frame.transform.Find("Chemical").gameObject.SetActive(false);
				frame.transform.Find("Desc").GetComponent<TextMesh>().text = "Not Playing";
				playerPlaying[playerNumber-1] = false;

			}
//			menuSelect1.SetActive(false);
//			menuSelect2.SetActive(false);
//			menuSelect3.SetActive(false);
//			menuSelect4.SetActive(false);
//			currentSelect1 = null;
//			currentSelect2 = null;
//			currentSelect3 = null;
//			currentSelect4 = null;

			RefreshMenuButtons();
			return;
		} else if (btnName == "Class"){
			int playerNumber = myCurrentSelect.GetComponent<MenuButtonScript>().playerNumber;
			GameObject frame = null;
			if(playerNumber == 1){
				frame = playerFrame1;
			} else if(playerNumber == 2){
				frame = playerFrame2;
			} else if(playerNumber == 3){
				frame = playerFrame3;
			} else if(playerNumber == 4){
				frame = playerFrame4;
			}
			playerClass[playerNumber-1]++;
			if(playerClass[playerNumber-1]>3)
				playerClass[playerNumber-1] = 0;
			frame.transform.Find("Class").GetComponent<TextMesh>().text = classes[playerClass[playerNumber-1]];
			frame.transform.Find("Desc").GetComponent<TextMesh>().text = classDescs[playerClass[playerNumber-1]];
			return;
		} else if (btnName == "Chemical"){
			int playerNumber = myCurrentSelect.GetComponent<MenuButtonScript>().playerNumber;
			GameObject frame = null;
			if(playerNumber == 1){
				frame = playerFrame1;
			} else if(playerNumber == 2){
				frame = playerFrame2;
			} else if(playerNumber == 3){
				frame = playerFrame3;
			} else if(playerNumber == 4){
				frame = playerFrame4;
			}
			playerChemical[playerNumber-1]++;
			if(playerChemical[playerNumber-1]>2)
				playerChemical[playerNumber-1] = 0;
			frame.transform.Find("Chemical").GetComponent<TextMesh>().text = chemicals[playerChemical[playerNumber-1]];
			return;
		} else if (btnName == "SettingsSound"){
			isSFXOn = !isSFXOn;
			if(isSFXOn)
				myCurrentSelect.GetComponent<TextMesh>().text = "SFX On";
			else
				myCurrentSelect.GetComponent<TextMesh>().text = "SFX Off";
			GameVariables.isSFXOn = isSFXOn;
			return;

		} else if (btnName == "SettingsMusic"){
			isMusicOn = !isMusicOn;
			if(isMusicOn)
				myCurrentSelect.GetComponent<TextMesh>().text = "Music On";
			else
				myCurrentSelect.GetComponent<TextMesh>().text = "Music Off";
			GameVariables.isMusicOn = isMusicOn;
			return;
		} else  if (btnName == "Start"){
			GameVariables.playersPlaying = new bool[4];
			GameVariables.playerClasses = new string[4];
			GameVariables.playerChemicals = new int[4];


			GameObject frame = null;
			if(player == 1){
				frame = playerFrame1;
			} else if(player == 2){
				frame = playerFrame2;
			} else if(player == 3){
				frame = playerFrame3;
			} else if(player == 4){
				frame = playerFrame4;
			}

			playerReady[player-1] = !playerReady[player-1];
			if(playerReady[player-1])
				frame.transform.Find("ReadyFrame").gameObject.SetActive(true);
			else
				frame.transform.Find("ReadyFrame").gameObject.SetActive(false);

			int nPlayersPlaying = 0;
			int nPlayersReady = 0;
			int regPlayers = 0;
			for(int i=0; i<playerPlaying.Length ;i++){
				if(playerReady[i])
					nPlayersReady++;

				if(playerPlaying[i])
					nPlayersPlaying++;

				if(playerPlaying[i]){
					GameVariables.playersPlaying[regPlayers] = playerPlaying[i];
					GameVariables.playerClasses[regPlayers] = classes[playerClass[i]];
					GameVariables.playerChemicals[regPlayers] = playerChemical[i];
					regPlayers++;
				}
			}
			GameVariables.nPlayers = nPlayersPlaying;


			if(nPlayersReady == nPlayersPlaying)
				Application.LoadLevel(1);

			return;
		} else  if (btnName == "Exit"){
			Application.Quit();
		}

		menuSelect1.SetActive(false);
		menuSelect2.SetActive(false);
		menuSelect3.SetActive(false);
		menuSelect4.SetActive(false);
		currentSelect1 = null;
		currentSelect2 = null;
		currentSelect3 = null;
		currentSelect4 = null;

		if(currentScreen - previousScreen > 0) {
			StartCoroutine(Translation (mainMenu.transform, leftOfScreenPos, ScreensTransitionTime, MoveType.Time));
			StartCoroutine(Translation (settingsMenu.transform, leftOfScreenPos, ScreensTransitionTime, MoveType.Time));
			StartCoroutine(Translation (playMenu.transform, leftOfScreenPos, ScreensTransitionTime, MoveType.Time));
		} else if (currentScreen - previousScreen < 0){
			StartCoroutine(Translation (mainMenu.transform, rightOfScreenPos, ScreensTransitionTime, MoveType.Time));
			StartCoroutine(Translation (settingsMenu.transform, rightOfScreenPos, ScreensTransitionTime, MoveType.Time));
			StartCoroutine(Translation (playMenu.transform, rightOfScreenPos, ScreensTransitionTime, MoveType.Time));
		}
		StartCoroutine(WaitAndRefresh());
	}

	IEnumerator WaitAndRefresh() {
		yield return new WaitForSeconds(ScreensTransitionTime +0.1f);
		RefreshMenuButtons();
	}

	private void RefreshMenuButtons(){

		menuSelect1.SetActive(false);
		menuSelect2.SetActive(false);
		menuSelect3.SetActive(false);
		menuSelect4.SetActive(false);
		currentSelect1 = null;
		currentSelect2 = null;
		currentSelect3 = null;
		currentSelect4 = null;

		FindCurrentMenuButtons();
		if(!playerReady[0]){
			currentSelect1 = findTopObject(menuButtonsP1, currentSelect1);
			if(currentSelect1 != null){
				SetMenuSelector(menuSelect1, currentSelect1);
			}
		}
		if(!playerReady[1]){
			currentSelect2 = findTopObject(menuButtonsP2, currentSelect2);
			if(currentSelect2 != null){
				SetMenuSelector(menuSelect2, currentSelect2);
			}
		}
		if(!playerReady[2]){
			currentSelect3 = findTopObject(menuButtonsP3, currentSelect3);
			if(currentSelect3 != null){
				SetMenuSelector(menuSelect3, currentSelect3);
			}
		}		
		if(!playerReady[3]){
			currentSelect4 = findTopObject(menuButtonsP4, currentSelect4);
			if(currentSelect4 != null){
				SetMenuSelector(menuSelect4, currentSelect4);
			}
		}
		
	}

	public void SetMenuSelectorMouse(){
		SetMenuSelector(menuSelect1, currentSelect1);
	}


	public void SetMenuSelector(){
		if(nControllers == 0) {
		} else if (nControllers == 1) {
			SetMenuSelector(menuSelect1, currentSelect1);

		} else if (nControllers == 2) {
			SetMenuSelector(menuSelect1, currentSelect1);
			SetMenuSelector(menuSelect2, currentSelect2); 

		} else if (nControllers == 3) {
			SetMenuSelector(menuSelect1, currentSelect1);
			SetMenuSelector(menuSelect2, currentSelect2); 
			SetMenuSelector(menuSelect3, currentSelect3);

		} else if (nControllers == 4) {
			SetMenuSelector(menuSelect1, currentSelect1);
			SetMenuSelector(menuSelect2, currentSelect2); 
			SetMenuSelector(menuSelect3, currentSelect3);
			SetMenuSelector(menuSelect4, currentSelect4);
			
		}

	}
	
	public void SetMenuSelector(GameObject myMenuSelect, GameObject myCurrentSelect){
		if(myCurrentSelect == null)
			return;

		int playerNumber = myCurrentSelect.GetComponent<MenuButtonScript>().playerNumber;
		if(playerNumber == 0){
			if(currentSelect1 != null && currentSelect1.GetComponent<MenuButtonScript>().playerNumber == 0){
				menuSelect1.SetActive(false);
			}
			if(currentSelect2 != null && currentSelect2.GetComponent<MenuButtonScript>().playerNumber == 0){
				menuSelect2.SetActive(false);
			}
			if(currentSelect3 != null && currentSelect3.GetComponent<MenuButtonScript>().playerNumber == 0){
				menuSelect3.SetActive(false);
			}
			if(currentSelect4 != null && currentSelect4.GetComponent<MenuButtonScript>().playerNumber == 0){
				menuSelect4.SetActive(false);
			}
		}


		Vector3 menuSelectOffset = myCurrentSelect.GetComponent<MenuButtonScript>().menuSelectOffset;
		Vector3 menuSelectScale = myCurrentSelect.GetComponent<MenuButtonScript>().menuSelectScale;

		myMenuSelect.transform.position = new Vector3(myCurrentSelect.transform.position.x + menuSelectOffset.x, 
		                                              myCurrentSelect.transform.position.y + menuSelectOffset.y,
		                                              myCurrentSelect.transform.position.z + menuSelectOffset.z);
		myMenuSelect.transform.localScale = menuSelectScale;

		myCurrentSelect.GetComponent<MenuButtonScript>().SetSelected(true);
		foreach(GameObject o in menuButtons){
			if(o != myCurrentSelect){
				o.GetComponent<MenuButtonScript>().SetSelected(false);
			}
		}
		myMenuSelect.SetActive(true);
	}

	public void SetSelectedButton(GameObject button){
		currentSelect1 = button;
	}


	void Update() {

		if(Input.GetKey(KeyCode.Escape)) {
			Application.Quit();
		}



		if(nControllers == 0) {
		} else if (nControllers == 1) {
			GetJoystick1Input();
	
		} else if (nControllers == 2) {
			GetJoystick1Input();
			GetJoystick2Input();

		} else if (nControllers == 3) {
			GetJoystick1Input();
			GetJoystick2Input();
			GetJoystick3Input();

		} else if (nControllers == 4) {
			GetJoystick1Input();
			GetJoystick2Input();
			GetJoystick3Input();
			GetJoystick4Input();

		}

		if(Input.GetKeyDown(KeyCode.UpArrow)){
			GameObject target = findTopObject(menuButtonsP1, currentSelect1);
			if(target != null){
				currentSelect1 = target;
				SetMenuSelector(menuSelect1, currentSelect1);
			}
		}

		if(Input.GetKeyDown(KeyCode.DownArrow)){
			GameObject target = findBottomObject(menuButtonsP1, currentSelect1);
			if(target != null){
				currentSelect1 = target;
				SetMenuSelector(menuSelect1, currentSelect1);
			}
		}


		if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)){
			if(currentSelect1 != null)
				ButtonPressed(currentSelect1.GetComponent<MenuButtonScript>().buttonName, currentSelect1, 1);
		}



	}

	GameObject findTopObject(GameObject[] buttons, GameObject myCurrentSelect) {
		GameObject returnObject = null;
		if(buttons == null)
			return null;


		if (myCurrentSelect == null) {
			float maxY = -Mathf.Infinity;
			foreach(GameObject o in buttons){
				if(o.transform.position.y > maxY){
					returnObject = o;
					maxY = o.transform.position.y;
				}
			}
		} else {
			float curY = myCurrentSelect.transform.position.y;
			float minY = Mathf.Infinity;
			foreach(GameObject o in buttons){
				if(o.transform.position.y > curY && o.transform.position.y <= minY){
					returnObject = o;
					minY = o.transform.position.y;

				}
			}
		}
		return returnObject;
	}

	GameObject findBottomObject(GameObject[] buttons, GameObject myCurrentSelect) {

		GameObject returnObject = null;
		if(buttons == null)
			return null;

		if (myCurrentSelect == null) {
			float minY = Mathf.Infinity;
			foreach(GameObject o in buttons){
				if(o.transform.position.y < minY){
					returnObject = o;
					minY = o.transform.position.y;

				}
			}
		} else {
			float curY = myCurrentSelect.transform.position.y;
			float maxY = -Mathf.Infinity;
			foreach(GameObject o in buttons){
				if(o.transform.position.y < curY && o.transform.position.y >= maxY){
					returnObject = o;
					maxY = o.transform.position.y;

				}
			}
		}
		return returnObject;

	}


	void FindCurrentMenuButtons(){
		GameObject[] allButtons = GameObject.FindGameObjectsWithTag("MenuButton");

		GameObject currentMenuObject = null;

		List<GameObject> buttons0 = new List<GameObject>();
		List<GameObject> buttons1 = new List<GameObject>();
		List<GameObject> buttons2 = new List<GameObject>();
		List<GameObject> buttons3 = new List<GameObject>();
		List<GameObject> buttons4 = new List<GameObject>();

		if(currentScreen == SETTINGS_MENU){
			currentMenuObject = settingsMenu;
		} else if(currentScreen == MAIN_MENU){
			currentMenuObject = mainMenu;
		} else if(currentScreen == PLAY_MENU){
			currentMenuObject = playMenu;
		}

		foreach(GameObject g in allButtons){
			if(g.transform.parent == currentMenuObject.transform
			   || g.transform.parent.parent == currentMenuObject.transform){
				buttons0.Add(g);

				if(g.GetComponent<MenuButtonScript>().playerNumber == 1){
					buttons1.Add(g);

				} else if (g.GetComponent<MenuButtonScript>().playerNumber == 2){
					buttons2.Add(g);

				} else if (g.GetComponent<MenuButtonScript>().playerNumber == 3){
					buttons3.Add(g);

				} else if (g.GetComponent<MenuButtonScript>().playerNumber == 4){
					buttons4.Add(g);

				} else {
					buttons1.Add(g);
					buttons2.Add(g);
					buttons3.Add(g);
					buttons4.Add(g);
				}


			}
		}

		menuButtons = buttons0.ToArray();
		menuButtonsP1 = buttons1.ToArray();
		if(nControllers > 0)
			menuButtonsP2 = buttons2.ToArray();
		if(nControllers > 1)
			menuButtonsP3 = buttons3.ToArray();
		if(nControllers > 2)
			menuButtonsP4 = buttons4.ToArray();
	}


	IEnumerator Translation (Transform thisTransform , Vector3 endPos , float value , MoveType moveType) {
		yield return StartCoroutine(Translation (thisTransform, thisTransform.position, thisTransform.position + endPos, value, moveType));
	}
	
	IEnumerator Translation (Transform thisTransform , Vector3 startPos ,Vector3 endPos , float value , MoveType moveType) {
		var rate = (moveType == MoveType.Time)? 1.0f/value : 1.0f/Vector3.Distance(startPos, endPos) * value;
		var t = 0.0f;
		while (t < 1.0f) {
			t += Time.deltaTime * rate;
			thisTransform.position = Vector3.Lerp(startPos, endPos, t);
			yield return null; 
		}

	}

	void GetJoystick1Input(){
		float h = Input.GetAxis("P1Alt_Horizontal");
		float v = Input.GetAxis("P1Alt_Vertical");
		bool up = false;
		bool down = false;
		bool left = false;
		bool right = false;
		debounce1v += Time.deltaTime;
		debounce1h += Time.deltaTime;
		
		if(v > 0.2){
			if(debounce1v > 0.3f){
				up = true;
				debounce1v = 0.0f;
			}
		} else if (v < -0.2) {
			if(debounce1v > 0.3f){
				down = true;
				debounce1v = 0.0f;
			}
		} else {
			debounce1v = 0.3f;
		}
		if(h > 0.2){
			if(debounce1h > 0.3f){
				right = true;
				debounce1h = 0.0f;
			}
		} else if (h < -0.2) {
			if(debounce1h > 0.3f){
				left = true;
				debounce1h = 0.0f;
			}
		} else {
			debounce1h = 0.3f;
		}

		if(!playerReady[0]){
			if(up){
				GameObject target = findTopObject(menuButtonsP1, currentSelect1);
				if(target != null){
					currentSelect1 = target;
					SetMenuSelector(menuSelect1, currentSelect1);
				}
			} else if(down){
				GameObject target = findBottomObject(menuButtonsP1, currentSelect1);
				if(target != null){
					currentSelect1 = target;
					SetMenuSelector(menuSelect1, currentSelect1);
				}
			}
		}
		if (Input.GetKeyDown (KeyCode.Joystick1Button0)){
			if(currentSelect1 != null)

				ButtonPressed(currentSelect1.GetComponent<MenuButtonScript>().buttonName, currentSelect1, 1);
//			print ("btn1 pressed!");

		}
		
		if (Input.GetKeyDown (KeyCode.Joystick1Button2)){
			//Back! Cancel			
			playerReady[0] = false;
			playerFrame1.transform.Find("ReadyFrame").gameObject.SetActive(false);
			currentSelect1 = null;
			GameObject target = findTopObject(menuButtonsP1, currentSelect1);
			if(target != null){
				currentSelect1 = target;
				SetMenuSelector(menuSelect1, currentSelect1);
			}
		}

	}



	void GetJoystick2Input(){
		float h = Input.GetAxis("P2_Horizontal");
		float v = Input.GetAxis("P2_Vertical");
		bool up = false;
		bool down = false;
		bool left = false;
		bool right = false;
		debounce2v += Time.deltaTime;
		debounce2h += Time.deltaTime;
		
		if(v > 0.2){
			if(debounce2v > 0.3f){
				up = true;
				debounce2v = 0.0f;
			}
		} else if (v < -0.2) {
			if(debounce2v > 0.3f){
				down = true;
				debounce2v = 0.0f;
			}
		} else {
			debounce2v = 0.3f;
		}
		if(h > 0.2){
			if(debounce2h > 0.3f){
				right = true;
				debounce2h = 0.0f;
			}
		} else if (h < -0.2) {
			if(debounce2h > 0.3f){
				left = true;
				debounce2h = 0.0f;
			}
		} else {
			debounce2h = 0.3f;
		}
		
		if(!playerReady[1]){

			if(up){
				GameObject target = findTopObject(menuButtonsP2, currentSelect2);
				if(target != null){
					currentSelect2 = target;
					SetMenuSelector(menuSelect2, currentSelect2);
				}
			} else if(down){
				GameObject target = findBottomObject(menuButtonsP2, currentSelect2);
				if(target != null){
					currentSelect2 = target;
					SetMenuSelector(menuSelect2, currentSelect2);
				}
			}
		}	
		if (Input.GetKeyDown (KeyCode.Joystick2Button0)){
			if(currentSelect2 != null)

				ButtonPressed(currentSelect2.GetComponent<MenuButtonScript>().buttonName, currentSelect2,2);

//			print ("btn2 pressed!");
		}
		
		if (Input.GetKeyDown (KeyCode.Joystick2Button2)){
			//Back! Cancel	
			playerReady[1] = false;
			playerFrame2.transform.Find("ReadyFrame").gameObject.SetActive(false);
			currentSelect2 = null;
			GameObject target = findTopObject(menuButtonsP2, currentSelect2);
			if(target != null){
				currentSelect2 = target;
				SetMenuSelector(menuSelect2, currentSelect2);
			}

		}
	}

	void GetJoystick3Input(){
		float h = Input.GetAxis("P3_Horizontal");
		float v = Input.GetAxis("P3_Vertical");
		bool up = false;
		bool down = false;
		bool left = false;
		bool right = false;
		debounce3v += Time.deltaTime;
		debounce3h += Time.deltaTime;
		
		if(v > 0.2){
			if(debounce3v > 0.3f){
				up = true;
				debounce3v = 0.0f;
			}
		} else if (v < -0.2) {
			if(debounce3v > 0.3f){
				down = true;
				debounce3v = 0.0f;
			}
		} else {
			debounce3v = 0.3f;
		}
		if(h > 0.2){
			if(debounce3h > 0.3f){
				right = true;
				debounce3h = 0.0f;
			}
		} else if (h < -0.2) {
			if(debounce3h > 0.3f){
				left = true;
				debounce3h = 0.0f;
			}
		} else {
			debounce3h = 0.3f;
		}
		
		if(!playerReady[2]){

			if(up){
				GameObject target = findTopObject(menuButtonsP3, currentSelect3);
				if(target != null){
					currentSelect3 = target;
					SetMenuSelector(menuSelect3, currentSelect3);
				}
			} else if(down){
				GameObject target = findBottomObject(menuButtonsP3, currentSelect3);
				if(target != null){
					currentSelect3 = target;
					SetMenuSelector(menuSelect3, currentSelect3);
				}
			}
		}
		if (Input.GetKeyDown (KeyCode.Joystick3Button0)){
			if(currentSelect3 != null)

				ButtonPressed(currentSelect3.GetComponent<MenuButtonScript>().buttonName, currentSelect3,3);
//			print ("btn3 pressed!");

		}
		
		if (Input.GetKeyDown (KeyCode.Joystick3Button2)){
			//Back! Cancel			
			playerReady[2] = false;
			playerFrame3.transform.Find("ReadyFrame").gameObject.SetActive(false);
			currentSelect3 = null;
			GameObject target = findTopObject(menuButtonsP3, currentSelect3);
			if(target != null){
				currentSelect3 = target;
				SetMenuSelector(menuSelect3, currentSelect3);
			}

		}
	}

	void GetJoystick4Input(){
		float h = Input.GetAxis("P4_Horizontal");
		float v = Input.GetAxis("P4_Vertical");
		bool up = false;
		bool down = false;
		bool left = false;
		bool right = false;
		debounce4v += Time.deltaTime;
		debounce4h += Time.deltaTime;
		
		if(v > 0.2){
			if(debounce4v > 0.3f){
				up = true;
				debounce4v = 0.0f;
			}
		} else if (v < -0.2) {
			if(debounce4v > 0.3f){
				down = true;
				debounce4v = 0.0f;
			}
		} else {
			debounce4v = 0.3f;
		}
		if(h > 0.2){
			if(debounce4h > 0.3f){
				right = true;
				debounce4h = 0.0f;
			}
		} else if (h < -0.2) {
			if(debounce4h > 0.3f){
				left = true;
				debounce4h = 0.0f;
			}
		} else {
			debounce4h = 0.3f;
		}
		
		if(!playerReady[3]){

			if(up){
				GameObject target = findTopObject(menuButtonsP4, currentSelect4);
				if(target != null){
					currentSelect4 = target;
					SetMenuSelector(menuSelect4, currentSelect4);

				}
			} else if(down){
				GameObject target = findBottomObject(menuButtonsP4, currentSelect4);
				if(target != null){
					currentSelect4 = target;
					SetMenuSelector(menuSelect4, currentSelect4);
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.Joystick4Button0)){
			if(currentSelect4 != null)
				ButtonPressed(currentSelect4.GetComponent<MenuButtonScript>().buttonName, currentSelect4,4);

//			print ("btn4 pressed!");

		}
		
		if (Input.GetKeyDown (KeyCode.Joystick4Button2)){
			//Back! Cancel	
			playerReady[3] = false;
			playerFrame4.transform.Find("ReadyFrame").gameObject.SetActive(false);
			currentSelect4 = null;
			GameObject target = findTopObject(menuButtonsP4, currentSelect4);
			if(target != null){
				currentSelect4 = target;
				SetMenuSelector(menuSelect4, currentSelect4);
				
			}
		}
	}

}
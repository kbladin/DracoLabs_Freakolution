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
	private GameObject currentSelect = null;
	private int currentScreen = 0;

	private bool isMusicOn = true;
	private bool isSFXOn = true;


	void Awake() {
		instance = this;
	}

	void Start(){
		currentScreen = MAIN_MENU;//PLAY_MENU;//

		FindCurrentMenuButtons();
		currentSelect = findTopObject();
		if(currentSelect != null)
			SetMenuSelector();

	}

	public void ButtonPressed(string btnName){

//		print (btnName);

		int previousScreen = currentScreen;
		if (btnName == "Play"){
			currentScreen = PLAY_MENU;
		} else if (btnName == "Settings"){
			currentScreen = SETTINGS_MENU;
		} else if (btnName == "Back"){
			currentScreen = MAIN_MENU;
		} else if (btnName == "SettingsSound"){
			isSFXOn = !isSFXOn;
			if(isSFXOn)
				currentSelect.GetComponent<TextMesh>().text = "SFX On";
			else
				currentSelect.GetComponent<TextMesh>().text = "SFX Off";
			return;

		} else if (btnName == "SettingsMusic"){
			isMusicOn = !isMusicOn;
			if(isMusicOn)
				currentSelect.GetComponent<TextMesh>().text = "Music On";
			else
				currentSelect.GetComponent<TextMesh>().text = "Music Off";
			return;
		} else  if (btnName == "Exit"){
			Application.Quit();
		}

		menuSelect.SetActive(false);
		currentSelect = null;

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
		FindCurrentMenuButtons();
		currentSelect = findTopObject();
		if(currentSelect != null){
			SetMenuSelector(); 
		}
	}

	public void SetMenuSelector(){

		Vector3 menuSelectOffset = currentSelect.GetComponent<MenuButtonScript>().menuSelectOffset;
		Vector3 menuSelectScale = currentSelect.GetComponent<MenuButtonScript>().menuSelectScale;

		menuSelect.transform.position = new Vector3(currentSelect.transform.position.x + menuSelectOffset.x, 
		                                            currentSelect.transform.position.y + menuSelectOffset.y,
		                                            currentSelect.transform.position.z + menuSelectOffset.z);
		menuSelect.transform.localScale = menuSelectScale;

		currentSelect.GetComponent<MenuButtonScript>().SetSelected(true);
		foreach(GameObject o in menuButtons){
			if(o != currentSelect){
				o.GetComponent<MenuButtonScript>().SetSelected(false);
			}
		}
		menuSelect.SetActive(true);
	}

	public void SetSelectedButton(GameObject button){
		currentSelect = button;
	}


	void Update() {

		if(Input.GetKey(KeyCode.Escape)) {
			Application.Quit();
		}

		if(Input.GetKeyDown(KeyCode.UpArrow)){
			GameObject target = findTopObject();
			if(target != null){
				currentSelect = target;
				SetMenuSelector();
			}
		}

		if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)){
			ButtonPressed(currentSelect.GetComponent<MenuButtonScript>().buttonName);
		}

		if(Input.GetKeyDown(KeyCode.DownArrow)){
			GameObject target = findBottomObject();
			if(target != null){
				currentSelect = target;
				SetMenuSelector();
			}
		}

	}

	GameObject findTopObject() {
		GameObject returnObject = null;

		if (currentSelect == null) {
			float maxY = -Mathf.Infinity;
			foreach(GameObject o in menuButtons){
				if(o.transform.position.y > maxY){
					returnObject = o;
					maxY = o.transform.position.y;
				}
			}
		} else {
			float curY = currentSelect.transform.position.y;
			float minY = Mathf.Infinity;
			foreach(GameObject o in menuButtons){
				if(o.transform.position.y > curY && o.transform.position.y <= minY){
					returnObject = o;
					minY = o.transform.position.y;

				}
			}
		}
		return returnObject;
	}

	GameObject findBottomObject() {

		GameObject returnObject = null;
		
		if (currentSelect == null) {
			float minY = Mathf.Infinity;
			foreach(GameObject o in menuButtons){
				if(o.transform.position.y < minY){
					returnObject = o;
					minY = o.transform.position.y;

				}
			}
		} else {
			float curY = currentSelect.transform.position.y;
			float maxY = -Mathf.Infinity;
			foreach(GameObject o in menuButtons){
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

		List<GameObject> buttons = new List<GameObject>();

		if(currentScreen == SETTINGS_MENU){
			currentMenuObject = settingsMenu;
		} else if(currentScreen == MAIN_MENU){
			currentMenuObject = mainMenu;
		} else if(currentScreen == PLAY_MENU){
			currentMenuObject = playMenu;
		}

		foreach(GameObject g in allButtons){
			if(g.transform.parent == currentMenuObject.transform){
				buttons.Add(g);
			}
			if(g.transform.parent.parent == currentMenuObject.transform){
				buttons.Add(g);
			}
		}
		menuButtons = buttons.ToArray();
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

}
public var isQuit = false;

function Start(){
	renderer.material.color = Color.green;

}


function OnMouseEnter(){
	renderer.material.color = Color.white;
//	this.color = Color.red;
}


function OnMouseExit() {
	renderer.material.color = Color.green;
}


function OnMouseUp(){
	if (isQuit == true){
		Application.Quit();
	}
	else {
		Application.LoadLevel(1);
	}
}


function Update() {

	if(Input.GetKey(KeyCode.Escape)) {
		Application.Quit();
	}

}
using UnityEngine;
using System.Collections;

public class PortraitCoin : MonoBehaviour {

	bool rotatePortrait = false;
	public bool facingCamera = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


	}

	void OnMouseUp(){
//		FlipCoin();
	}

	public void FlipCoin(){
		if( !rotatePortrait){
			rotatePortrait = true;		
			facingCamera = !facingCamera;
			StartCoroutine(Rotation (this.transform.up * 180, 1.0f));
		}
	}

	IEnumerator Rotation (Vector3 degrees ,float time ) {

		Quaternion startRotation = this.transform.rotation;
		Quaternion endRotation = this.transform.rotation * Quaternion.Euler(degrees);
		float rate = 1.0f/time;
		float t = 0.0f;
		while (t < 1.0f) {
			t += Time.deltaTime * rate;
			this.transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
			yield return null;
		}
		rotatePortrait = false;
	}


}

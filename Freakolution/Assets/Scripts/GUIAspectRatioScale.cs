/*/
* Script by Devin Curry
	* www.Devination.com
		* www.youtube.com/user/curryboy001
		* Please like and subscribe if you found my tutorials helpful :D
			* Google+ Community: https://plus.google.com/communities/108584850180626452949
				* Twitter: https://twitter.com/Devination3D
				* Facebook Page: https://www.facebook.com/unity3Dtutorialsbydevin
				*
				* Attach this script to a GUITexture or GUIText to fix its scale to the aspect ratio of the current screen
				* Change the values of scaleOnRatio1 (units are in % of screenspace if the screen were square, 0f = 0%, 1f = 100%) to change the desired ratio of the GUI
				* The ratio is width-based so scaleOnRatio1.x will stay the same, scaleOnRatio1.y will based on the screen ratio
				*
				* The most common aspect ratio for COMPUTERS is 16:9 followed by 16:10
					* Possible aspect ratios for MOBILE are:
						* 4:3
							* 3:2
							* 16:10
							* 5:3
							* 16:9
							/*/
using UnityEngine;
using System.Collections;

public class GUIAspectRatioScale : MonoBehaviour 
{
	public Vector2 scaleOnRatio1 = new Vector2(0.1f, 0.1f);
	private Transform myTrans;
	private float widthHeightRatio;
	private float height;
	private GUIText[] guiTexts;
	private float screenWidth;
	private float screenHeight;
	
	
	void Start () 
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		myTrans = transform;
		guiTexts = GetComponentsInChildren<GUIText>();
		widthHeightRatio = (float)Screen.width/Screen.height;
		//height = widthHeightRatio * scaleOnRatio1.y;
		height = transform.localScale.y;
		SetScale();
	}
	//call on an event that tells if the aspect ratio changed
	void Update()
	{
		if(Mathf.Abs(widthHeightRatio - (float)Screen.width/Screen.height)>0.1)
			SetScale();
	
	}
	void SetScale()
	{
		float scaleX = Screen.width / screenWidth;
		float scaleY = Screen.height / screenHeight;
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		Debug.Log("I setscale");
		//find the aspect ratio
		widthHeightRatio = (float)Screen.width/Screen.height;
		float newHeight = widthHeightRatio * scaleOnRatio1.y;
		float trans = (height - newHeight)/2;
		transform.Translate(new Vector3(0,trans,0));
		
		GUIText number;
		for(int i = 0; i < guiTexts.Length; i++)
		{
			number = guiTexts[i];
			Vector2 pixOff = number.pixelOffset;
			int origSize = number.fontSize;
			number.pixelOffset = new Vector2(pixOff.x * scaleX, pixOff.y * scaleY);
			number.fontSize = Mathf.CeilToInt(origSize * scaleX);
		}
		
		
		height = newHeight;
		//Apply the scale. We only calculate y since our aspect ratio is x (width) authoritative: width/height (x/y)
		myTrans.localScale = new Vector3 (scaleOnRatio1.x, height, 1);
	}
}
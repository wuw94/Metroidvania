using UnityEngine;
using System.Collections;

//Text Generator just makes single textboxes once, cannot change it
//Goal for tomorrow, figure out why must print out from OnGUI and how to manipulate that
public class TextGenerator : MonoBehaviour {


	public string funky_string;
	public string funky_string2;
	
	void OnGUI () {
		funky_string = GUI.TextArea (new Rect(0, 0, 200, 100), "Top-left");
		GUI.Box (new Rect (0,0,100,50), "Top-left");
		GUI.Box (new Rect (Screen.width - 100,0,100,50), "Top-right");
		GUI.Box (new Rect (0,Screen.height - 50,100,50), "Bottom-left");
		GUI.Box (new Rect (Screen.width - 100,Screen.height - 50,100,50), "Bottom-right");

	}

}


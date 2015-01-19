using UnityEngine;
using System.Collections;

//Text Generator just makes single textboxes once, cannot change it
//Goal for tomorrow, figure out why must print out from OnGUI and how to manipulate that
public class TextGenerator : MonoBehaviour {
	

	public bool button_passed = false;
	void OnGUI () {
		if (Input.anyKey){
			button_passed = true;
		}
		if (button_passed) {
			Debug.Log("First button was clicked");
			GUI.TextArea (new Rect (0, 0, 100, 50), "Top-left");
			GUI.TextArea (new Rect (Screen.width - 100, 0, 100, 50), "Top-right");
			GUI.TextArea (new Rect (0, Screen.height - 50, 100, 50), "Bottom-left");
			GUI.TextArea (new Rect (Screen.width - 100, Screen.height - 50, 100, 50), "Bottom-right");
		}
		if (button_passed != true) {
			Debug.Log("Changes text bubbles");
			GUI.TextArea (new Rect(0, 0, 200, 100), "Top-left");
		}
	}
}

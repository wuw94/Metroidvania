using UnityEngine;
using System.Collections;

public class FinalButton : MonoBehaviour {

	public bool buttonIsPushed = false;

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player") 
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				buttonIsPushed = true;
				Debug.Log ("Button is pushed");
			}
		}	
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player") 
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				buttonIsPushed = true;
				Debug.Log ("Button is pushed");
			}
		}	
	}
}

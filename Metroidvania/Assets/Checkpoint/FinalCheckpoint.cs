using UnityEngine;
using System.Collections;

public class FinalCheckpoint : MonoBehaviour {

	public FinalButton button;

	public GameObject nextLevel;


	void Start()
	{
		button = GameObject.Find("FinalButton").GetComponent<FinalButton>();

		nextLevel = GameObject.Find("NextLevelScreen");
		nextLevel.SetActive (false);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player") 
		{
			if (button.buttonIsPushed)
			{
				nextLevel.SetActive(true);
			}
		}
	}

	public void NextLevel()
	{
		//Application.LoadLevel(Application.loadedLevel);
	}
}

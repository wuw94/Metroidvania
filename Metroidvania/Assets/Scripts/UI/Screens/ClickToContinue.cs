using UnityEngine;
using System.Collections;

public class ClickToContinue : MonoBehaviour {

	public string scene;

	// Update is called once per frame
	void Update () 
	{
		// Press any key to continue
		//if (Input.anyKeyDown && !loadLock)
		//	LoadScene ();

		// Press anywhere on the screen to continue
		if (Input.GetMouseButtonDown(0))
			LoadScene ();
	}

	void LoadScene()
	{
		Application.LoadLevel (scene);
	}
}

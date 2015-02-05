using UnityEngine;
using System.Collections;

/* TitleScreen.
 * Container for executing code with buttons found in the title screen
 * 
 * -NewGame()- creates a new game, along with default hierarchy, and launches the first scene
 * -LoadGame()- takes to a new page that shows loadable games
 * -Options()- takes to a new page that allows adjusting settings
 * 
 */


public class TitleScreen : MonoBehaviour
{	
	public void NewGame(string sceneToChangeTo)
	{
		GameManager.current_game = new GameManager();

		Application.LoadLevel (sceneToChangeTo);
	}

	public void LoadGame()
	{
		Application.LoadLevel ("Prototype Level");
	}

	public void Options()
	{
		Application.LoadLevel ("OptionsScreen");
	}

	public void Back()
	{
		Application.LoadLevel("TitleScreen");
	}
}

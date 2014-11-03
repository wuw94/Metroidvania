using UnityEngine;
using System.Collections;

/* Menu Button Methods.
 * Container for executing code with buttons found in the main menu
 * 
 * -NewGame()- creates a new game, along with default hierarchy, and launches the first scene
 * -LoadGame()- 
 * 
 * auth Wesley Wu
 */


public class MenuButtonMethods : ButtonManager
{
	public void NewGame(string sceneToChangeTo)
	{
		GameManager.current_game = new GameManager();

		Application.LoadLevel(sceneToChangeTo);
	}

	public void LoadGame()
	{
	}

	public void Options()
	{
	}
}

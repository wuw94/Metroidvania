using UnityEngine;
using System.Collections;

/* Game Manager.
 * Stores the game state, to be accessed during saving and loading
 * 
 * -map_name- stores the name of current map
 * -progression- stores information about how far the player is through the game
 * -player- object of type CharacterManager which stores information about the player
 * 
 * auth Wesley Wu
 */

[System.Serializable]
public sealed class GameManager
{
	public static GameManager current_game; // Holds the game state, accessable from anywhere

	public ProgressionManager progression;
	public PreferenceManager preferences;

	public GameManager()
	{
		this.progression = new ProgressionManager();
		this.preferences = new PreferenceManager();
	}
}

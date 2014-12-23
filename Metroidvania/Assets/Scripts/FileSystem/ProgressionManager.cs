using UnityEngine;
using System.Collections;

/* Progression Manager.
 * Stores information about how far the player is into the story line
 * 
 * character - class variable of CharacterManager; access variables within that class
 * 			 - in this case, movement speeds that will change in during progression
 * map - class variable of Map; access variables within that class
 * 
 * auth Wesley Wu
 */

/// <summary>
/// Stores information about how far the player is into the story line.
/// </summary>
/// <param name="filename"></param>
[System.Serializable]
public sealed class ProgressionManager
{
	/// <summary>
	/// Your game's character data.
	/// </summary>
	public CharacterManager character;
	
	public Map intro_map; // Eventually we'll store all the game's map into a list of this class

	/// <summary>
	/// Constructor.
	/// </summary>
	public ProgressionManager()
	{
		this.character = new CharacterManager();
		this.intro_map = new Map("Intro");
	}
}

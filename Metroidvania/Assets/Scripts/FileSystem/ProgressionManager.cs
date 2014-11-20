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

[System.Serializable]
public sealed class ProgressionManager
{
	public CharacterManager character;
	public Map map; // Eventually we'll store all the game's map into a list of this class

	public ProgressionManager()
	{
		this.character = new CharacterManager();
		this.map = new Map();
	}
}

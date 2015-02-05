using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public CharacterManager player;

	/// <summary>
	/// Dictionary of this game's maps.
	/// string Key, Map value
	/// </summary>
	public Dictionary<string, Map> maps;

	/// <summary>
	/// Name of currently loaded map
	/// </summary>
	public string loaded_map;

	/// <summary>
	/// Constructor.
	/// </summary>
	public ProgressionManager()
	{
		this.player = new CharacterManager();
		this.maps = new Dictionary<string, Map>();
	}
}

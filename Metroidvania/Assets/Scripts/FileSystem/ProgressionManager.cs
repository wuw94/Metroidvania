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
		//AddMaps();
	}

	/// <summary>
	/// Sets up all the game's maps for a new game. These will be changed during gameplay and serialized during save
	/// </summary>
	public void AddMaps()
	{
		foreach (string name in new string[]{"map2"})
		{
			maps.Add(name, new Map(name));
		}
		loaded_map = "map2";
		UnityEngine.Camera.main.GetComponent<TileManager>().BeginChecks();
	}


	public void ChangeMap(string name)
	{
		loaded_map = name;
	}

	public void AddMap(string name, Map map)
	{
		maps.Add(name, map);
		UnityEngine.Camera.main.GetComponent<TileManager>().BeginChecks();
	}
	
}

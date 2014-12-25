using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// Data for a game map.
/// </summary>
[System.Serializable]
public class Map
{

	/// <summary>
	/// Map name.
	/// </summary>
	public string name;

	/// <summary>
	/// Tile data, stored as [row][column]
	/// </summary>
	public TileInfo[][] tiles;

	/// <summary>
	/// XY data of where the player will be placed upon entering map.
	/// </summary>
	public PseudoVector2 spawn_point;

	/// <summary>
	/// Constructor for creating a new Map. Use this when designing maps to receive a blank template.
	/// </summary>
	public Map()
	{
	}

	/// <summary>
	/// Creates a map based on given filename
	/// </summary>
	/// <param name="filename"></param>
	public Map(string filename)
	{
		Debug.Log("Converting File: " + Application.dataPath + "/Maps/" + filename + ".md");
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(Application.dataPath + "/Maps/" + filename + ".md", FileMode.Open);
		CopyDataOver((Map)bf.Deserialize(file));
		Debug.Log("Success: " + Application.dataPath + "/Maps/" + filename + ".md");
		file.Close();
	}
	
	/// <summary>
	/// Copies data from an arbitrary map to this map
	/// </summary>
	/// <param name="map_data"></param>
	private void CopyDataOver(Map map_data)
	{
		this.name = map_data.name;
		this.tiles = map_data.tiles;
		this.spawn_point = map_data.spawn_point;
	}
}

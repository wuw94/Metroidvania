using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq.Expressions;
using System.IO;
using System;

/// <summary>
/// Data for a game map.
/// </summary>
[System.Serializable]
public class Map : ISerializable
{

	/// <summary>
	/// Tile data, stored as [row][column]
	/// </summary>
	public TileInfo[][] tiles;
	
	private short default_tile_x = 100;
	private short default_tile_y = 100;

	
	/// <summary>
	/// XY data of where the player will be placed upon entering map.
	/// </summary>
	public PseudoVector2 spawn_point = new PseudoVector2(0,0);

	/// <summary>
	/// Hashtable of entities (enemies, levers, bombs, items, ladders). Access the entity by entering the class type
	/// </summary>
	public Dictionary<EntityTypes, List<System.ValueType>> entity = new Dictionary<EntityTypes, List<System.ValueType>>();
	
	/// <summary>
	/// Default constructor for creating a new Map. Use this when designing maps to receive a blank template.
	/// </summary>
	public Map()
	{
		CreateTiles();
	}
	
	/// <summary>
	/// Creates a map based on given filename
	/// </summary>
	/// <param name="filename"></param>
	public Map(string filename)
	{
		FileStream file = null;
		try
		{
			Debug.Log("Reading File: " + Application.dataPath + "/Maps/" + filename + ".md");
			BinaryFormatter bf = new BinaryFormatter();
			file = File.Open(Application.dataPath + "/Maps/" + filename + ".md", FileMode.Open);
			Map map_data = (Map)bf.Deserialize(file);
			CopyDataOver(map_data);
			Debug.Log("Success: " + Application.dataPath + "/Maps/" + filename + ".md");
		}
		catch (System.Runtime.Serialization.SerializationException)
		{
			Debug.LogWarning("Problem while deserializing data.");
			throw;
		}
		finally
		{
			if (file != null)
			{
				file.Close();
			}
		}
	}

	
	
	/// <summary>
	/// Creates the TileInfo double array and sets the values to true.
	/// </summary>
	private void CreateTiles()
	{
		tiles = new TileInfo[default_tile_x][];
		for (short i = 0; i < default_tile_x; i++)
		{
			tiles[i] = new TileInfo[default_tile_y];
			for (short j = 0; j < default_tile_y; j++)
			{
				tiles[i][j] = new TileInfo(true, 0);
			}
		}
	}
	
	
	/// <summary>
	/// Copies data from an arbitrary map to this map
	/// </summary>
	/// <param name="map_data"></param>
	private void CopyDataOver(Map map_data)
	{
		this.tiles = map_data.tiles;
		this.spawn_point = map_data.spawn_point;
		this.entity = map_data.entity;
	}


	/// <summary>
	/// Custom serialization method.
	/// </summary>
	/// <param name="info">Info.</param>
	/// <param name="context">Context.</param>
	public virtual void GetObjectData(SerializationInfo info, StreamingContext context) // Called when serializing data
	{
		foreach (System.Reflection.FieldInfo field in typeof(Map).GetFields())
		{
			try
			{
				info.AddValue(field.Name, field.GetValue(this));
			}
			catch
			{
				Debug.LogWarning("Problem while serializing (" + field.Name + ").");
				throw;
			}
		}
	}


	/// <summary>
	/// Custom deserialization method.
	/// </summary>
	/// <param name="info">Info.</param>
	/// <param name="context">Context.</param>
	protected Map(SerializationInfo info, StreamingContext context) // Called when deserializing data
	{
		foreach (System.Reflection.FieldInfo field in typeof(Map).GetFields())
		{
			try
			{
				field.SetValue(this, info.GetValue(field.Name, field.FieldType));
			}
			catch
			{
				Debug.LogWarning("Old map data doesn't have (" + field.Name + ") info. Replacing with default data...");
				field.SetValue(this, field.GetValue(new Map()));
			}
		}
	}
	

	
}

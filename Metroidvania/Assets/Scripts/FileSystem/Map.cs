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
	/// <para>Number of types of entities stored. Types:</para>
	/// <para>0: Player</para>
	/// <para>1: Lever</para>
	/// <para>2: UpdraftGoo</para>
	/// <para>3: DependantPlatform</para>
	/// <para>4: Button</para>
	/// </summary>
	//public readonly byte type_count = 5;

	/// <summary>
	/// All the entities in the current map.
	/// </summary>
	public List<ArrayList> entities;

	/// <summary>
	/// Default constructor for creating a new Map. Use this when designing maps to receive a blank template.
	/// </summary>
	public Map()
	{
		CreateTiles();

		entities = new List<ArrayList>();
		for (byte i = 0; i < ResourceDirectory.resource.Count; i++)
		{
			entities.Add(new ArrayList());
		}
		entities[0].Add(((GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/Mobiles/Player/Player", typeof(GameObject)), new Vector3(0,0,0), Quaternion.identity)).GetComponent<Player>());
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
		this.entities = map_data.entities;
	}


	/// <summary>
	/// Converts the entities (which are objects within the map) into pseudo_entities.
	/// We require this to extract necessary data because GameObjects are not serializable.
	/// </summary>
	private void ConvertToPseudoGameObject()
	{
		byte j = 0;
		try
		{
			while (j < ResourceDirectory.resource.Count)
			{
				for (byte i = 0; i < entities[j].Count; i++)
				{
					entities[j][i] = Activator.CreateInstance(typeof(PseudoGameObject<>).MakeGenericType(new Type[]{entities[j][i].GetType()}), new object[]{entities[j][i]});
				}
				j++;
			}
		}
		catch (ArgumentOutOfRangeException)
		{
			for (byte i = j; i < ResourceDirectory.resource.Count; i++)
			{
				entities.Add(new ArrayList());
			}
		}
	}


	/// <summary>
	/// Converts pseudo_entities (which only contain object data) into entities
	/// We require this to convert object data into GameObjects.
	/// </summary>
	public void ConvertToGameObject()
	{
		byte j = 0;
		try
		{
			while (j < ResourceDirectory.resource.Count)
			{
				for (byte i = 0; i < entities[j].Count; i++)
				{
					Type t = entities[j][i].GetType().GetGenericArguments()[0];
					entities[j][i] = typeof(PseudoGameObject<>).MakeGenericType(new Type[]{t}).GetMethod("CreateGameObject").MakeGenericMethod(t).Invoke(null, new object[]{entities[j][i]});
				}
				j++;
			}
		}
		catch (ArgumentOutOfRangeException)
		{
			ArrayList[] new_entities = new ArrayList[ResourceDirectory.resource.Count];
			for (byte i = j; i < ResourceDirectory.resource.Count; i++)
			{
				entities.Add(new ArrayList());
			}
		}
	}


	/// <summary>
	/// Custom serialization method.
	/// </summary>
	/// <param name="info">Info.</param>
	/// <param name="context">Context.</param>
	public virtual void GetObjectData(SerializationInfo info, StreamingContext context) // Called when serializing data
	{
		ConvertToPseudoGameObject();
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

		//this.tiles = (TileInfo[][])info.GetValue("tiles", tiles.GetType());
		//return;

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
		ConvertToGameObject();
	}
	

	
}

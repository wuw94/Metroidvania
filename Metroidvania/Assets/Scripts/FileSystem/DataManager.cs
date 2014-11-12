using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/* Data Manager.
 * Data Manager ties closely with GameManager, and is able to:
 * 1. return a list of game files inside /Saved directory with
 * 		DataManager.ListGames()
 * 
 * 2. Save game state into the /Saved directory with
 * 		DataManager.Save(file_name)
 * 
 * 3. Load a game file inside the /Saved directory with
 * 		DataManager.Load(file_name)
 * 
 * Note:
 * We want to either cap the amount of files capable of being stored in /Saved or create a scrolling feature later on
 * 
 * auth Wesley Wu
 */

public static class DataManager
{
	public static List<string> ListGames()
	{
		return new List<string>();
		//TODO: return a list of strings of the filenames inside /Saved
	}
	
	public static void Save(string file_name)
	{
		if (!Directory.Exists(Application.persistentDataPath + "/Saved")) // Create a directory /Saved if it doesn't already exist
		{
			Directory.CreateDirectory(Application.persistentDataPath + "/Saved");
		}
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/Saved/" + file_name + ".gd");
		bf.Serialize(file, GameManager.current_game);
		Debug.Log("File saved at " + Application.persistentDataPath + "/Saved/" + file_name + ".gd");
		file.Close();
	}

	public static void Load(string file_name)
	{
		GameManager.current_game = new GameManager();
		if (File.Exists(Application.persistentDataPath + "/Saved/" + file_name + ".gd"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/Saved/" + file_name + ".gd", FileMode.Open);
			GameManager.current_game = (GameManager)bf.Deserialize(file);
			Debug.Log("File loaded at " + Application.persistentDataPath + "/Saved/" + file_name + ".gd");
			file.Close();
		}
	}
}

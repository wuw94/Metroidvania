using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/* Save Load.
 * Allows the game state to be saved via currentGame, which is of type Game
 * Also controls the reading of game files inside the /Saved directory
 * -savedGames- stores a list of game states found in the /Saved directory
 * -Save(string)- stores the current game state into a .gd (game data) file in the /Saved directory
 * -Load(string)- loads a game state from the /Saved directory
 * -file_name- name of file that will be shoved into the /Saved directory
 * 
 * auth Wesley Wu
 */

public static class DataManager {

	public static List<GameManager> saved_games = new List<GameManager>();

	private static void UpdateGames() // Helper function that updates the list saved_games
	{

	}
	
	public static void Save(string file_name)
	{
		if (!Directory.Exists(Application.persistentDataPath + "/Saved")) // Create a directory /Saved if it doesn't already exist
		{
			Directory.CreateDirectory(Application.persistentDataPath + "/Saved");
		}

		saved_games.Add(GameManager.current_game);
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/Saved/" + file_name + ".gd");
		bf.Serialize(file, DataManager.saved_games);
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
			DataManager.saved_games = (List<GameManager>)bf.Deserialize(file);
			Debug.Log("File loaded at " + Application.persistentDataPath + "/Saved/" + file_name + ".gd");
			file.Close();
			GameManager.current_game = saved_games[saved_games.Count-1]; //This is loading the most recent of the game files
		}
		Debug.Log (saved_games.Count);
	}
}

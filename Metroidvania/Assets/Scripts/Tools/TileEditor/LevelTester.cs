using UnityEngine;
using System.Collections;

public class LevelTester : MonoBehaviour {

	CameraManager camera_manager;
	// Use this for initialization
	void Start()
	{
		ReformatGame();
		camera_manager = (CameraManager)gameObject.AddComponent("CameraManager");
		if (PlayerPrefs.GetString("Map Name") != "")
		{
			try
			{
				GameManager.current_game.progression.AddMap("LevelTesterMap", new Map(PlayerPrefs.GetString("Map Name")));
			}
			catch
			{
				Debug.LogWarning("Could not load file: " + PlayerPrefs.GetString("Map Name"));
				throw;
			}
			camera_manager.player = (Player)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[0][0];
			camera_manager.returnToPlayer();
		}

	}
	

	/// <summary>
	/// When TileEditor scene is being run, game needs to be reformatted to allow new editing capabilities
	/// </summary>
	private void ReformatGame()
	{
		Debug.Log("LevelTester Scene detected. Reformatting current_game session...");
		GameManager.current_game = new GameManager();
		GameManager.current_game.progression.loaded_map = "LevelTesterMap";
		Debug.Log("Done");
	}
}

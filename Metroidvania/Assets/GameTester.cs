using UnityEngine;
using System.Collections;

public class GameTester : MonoBehaviour
{
	CameraManager camera_manager;
		
	void Start()
	{
		ReformatGame();
		camera_manager = (CameraManager)gameObject.AddComponent("CameraManager");
		camera_manager.player = (Player)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[0][0];
		camera_manager.returnToPlayer();

	}
	
	void OnGUI()
	{
		if (GUI.Button(new Rect(0,0,200,20), "Save"))
		{
			DataManager.Save("test");
		}
		if (GUI.Button(new Rect(0,20,200,20), "Load"))
		{
			DataManager.Load("test");
		}
	}

	
	/// <summary>
	/// When TileEditor scene is being run, game needs to be reformatted to allow new editing capabilities
	/// </summary>
	private void ReformatGame()
	{
		Debug.Log("GameTester Scene detected. Reformatting current_game session...");
		GameManager.current_game = new GameManager();
		Debug.Log("Done");
	}
}

using UnityEngine;
using System.Collections;

/* RenderingSystem.
 * 
 * IMPORTANT! Put this on the main camera of your scene. It will detect which scene is loaded and load the correct child scripts
 * 
 * RenderingSystem keeps track of whether your camera size has changed (from zooming) or moved (x or y direction)
 * 
 * If the current scene is TileEditor, a TileEdidor script will be attached to the camera
 * - We do this so all the script loading isn't prefab based, but automated.
 * 
 * Otherwise, a CameraManager script will be attached to the camera
 * - This is for individual levels.
 * 
 */

public class RenderingSystem : MonoBehaviour
{
	protected Vector3 screen_size; // width, height, orthographic size

	// x:left, y:right, z:bottom, w:top
	public static Vector4 unit_absolute; // amount of tiles that could be shown absolutely (disregard being outside tile bounds)
	public static Vector4 unit_shown; // amount of tiles that are shown (considering being outside tile bounds)
	// unit_shown is guaranteed to be within the bounds of row/columns

	
	private bool tiles_loaded = false;



	void Start()
	{
		gameObject.AddComponent("TileManager");
		gameObject.AddComponent("TextGenerator");
		if (Application.loadedLevelName == "LevelTester")
		{
			gameObject.AddComponent("LevelTester");
		}
		if (Application.loadedLevelName == "TileEditor")
		{
			gameObject.AddComponent("TileEditor");
		}

		BeginChecks();
	}

	void Update()
	{
		if (GameManager.current_game != null && GameManager.current_game.progression.loaded_map != null)
		{
			unit_shown.x = Mathf.Clamp(unit_absolute.x-1, 0, GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].tiles[0].Length);
			unit_shown.y = Mathf.Clamp(unit_absolute.y+1, 0, GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].tiles[0].Length);
			unit_shown.z = Mathf.Clamp(unit_absolute.z-1, 0, GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].tiles.Length);
			unit_shown.w = Mathf.Clamp(unit_absolute.w+1, 0, GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].tiles.Length);
		}
	}


	public void BeginChecks()
	{
		tiles_loaded = true;
		StartCoroutine(CheckSizeChanged());
		StartCoroutine(CheckScreenMoved());
	}


	private IEnumerator CheckSizeChanged()
	{
		while (true)
		{
			if (screen_size != new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, Camera.main.orthographicSize))
			{
				OnSizeChanged();
			}
			yield return null;
		}
	}


	private IEnumerator CheckScreenMoved()
	{
		while (true)
		{
			if (unit_absolute.x != Mathf.FloorToInt(Camera.main.ScreenToWorldPoint(Camera.main.ScreenToViewportPoint(new Vector3(0,0,0))).x) ||
			    unit_absolute.z != Mathf.FloorToInt(Camera.main.ScreenToWorldPoint(Camera.main.ScreenToViewportPoint(new Vector3(0,0,0))).y))
			{
				OnScreenMoved();
			}
			yield return null;
		}
	}


	/// <summary>
	/// Raises the size changed event.
	/// </summary>
	public virtual void OnSizeChanged()
	{
		screen_size.x = Camera.main.pixelWidth;
		screen_size.y = Camera.main.pixelHeight;
		screen_size.z = Camera.main.orthographicSize;
	}

	/// <summary>
	/// Raises the screen moved event.
	/// </summary>
	public virtual void OnScreenMoved()
	{
		unit_absolute.x = Mathf.FloorToInt(Camera.main.ScreenToWorldPoint(new Vector3(0,0,0)).x);
		unit_absolute.y = Mathf.CeilToInt(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth,Camera.main.pixelHeight,0)).x);
		unit_absolute.z = Mathf.FloorToInt(Camera.main.ScreenToWorldPoint(new Vector3(0,0,0)).y);
		unit_absolute.w = Mathf.CeilToInt(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth,Camera.main.pixelHeight,0)).y);
	}
}

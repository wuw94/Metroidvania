using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/* TileEditor
 * 
 * IMPORTANT! Do not put this on any object. It will place itself based on whether the scene name is "Tile Editor"
 * 
 */
public class TileEditor : MonoBehaviour
{
	private Map current_map; // The current map being edited
	private string map_name = ""; // name of map
	private GameObject spawn_point_indicator;
	private GameObject tile_indicator;
	
	private string[] paint_mode = new string[]{"Tile", "Spawn"};
	private int paint_mode_index = 0;
	private bool show_tab = true;
	private Rect window_rect = new Rect(0,0,Screen.width,Screen.height); // for displaying GUI
	private float camera_speed;
	private int FPS;


	void Start()
	{
		spawn_point_indicator = (GameObject)Instantiate(Resources.Load("Prefabs/TileEditor/SpawnPointIndicator", typeof(GameObject)), new Vector3(0,0,0), transform.rotation);
		spawn_point_indicator.gameObject.renderer.material.color = new Color(1,1,1,0);
		tile_indicator = (GameObject)Instantiate(Resources.Load("Prefabs/TileEditor/TileIndicator", typeof(GameObject)), new Vector3(0,0,0), transform.rotation);
		tile_indicator.gameObject.renderer.material.color = new Color(1,1,1,0.1f);
		StartCoroutine(UpdateFPS());
	}

	void OnGUI()
	{
		if (show_tab)
		{
			window_rect = GUI.Window(0, window_rect, TabWindowFunction, "Tile Editor");
		}
		else
		{
			int mouseX = (int)(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
			int mouseY = (int)(Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
			GUI.Label(new Rect(Input.mousePosition.x + 10,Screen.height - Input.mousePosition.y + 10,100, 20), "(" + mouseX + ", " + mouseY + ")");
		}
		if (!GetComponent<TileManager>().read_done)
		{
			window_rect = GUI.Window(0, window_rect, IntroWindowFunction, "Tile Editor");
		}
		GUI.Label(new Rect(Screen.width - 120, 20, Screen.width, 20), "Tiles in view: " + (RenderingSystem.unit_shown.y - RenderingSystem.unit_shown.x) * (RenderingSystem.unit_shown.w - RenderingSystem.unit_shown.z));
		GUI.Label(new Rect(Screen.width - 70, 40, Screen.width, 20), "FPS: " + FPS);
	}


	private IEnumerator UpdateFPS()
	{
		while (true)
		{
			FPS = (int)(1 / Time.deltaTime);
			yield return new WaitForSeconds(0.5f);
		}
	}


	void IntroWindowFunction(int windowID)
	{
		GUI.Label(new Rect(40,20,window_rect.width, 40), "Define a map name to load:");
		map_name = GUI.TextField(new Rect(10,40,Screen.width/2,20), map_name);
		
		if (GUI.Button(new Rect(10,60,100,40), "New"))
		{
			GetComponent<TileManager>().LoadAll();
			GetComponent<RenderingSystem>().LoadedDone();
		}
		if (GUI.Button(new Rect(Screen.width/2 - 90,60,100,40), "Load"))
		{
			try
			{
				current_map = new Map(map_name);
			}
			catch
			{
				Debug.LogWarning("Could not load file: " + map_name);
				throw;
			}

			GetComponent<TileManager>().LoadAll(current_map);
			GetComponent<RenderingSystem>().LoadedDone();

			spawn_point_indicator.transform.position = new Vector2(current_map.spawn_point.x, current_map.spawn_point.y);
		}
	}



	void TabWindowFunction(int windowID)
	{
		// Left side
		GUI.Label(new Rect(5,20,window_rect.width, 40), "Loaded Map:\n   <" + map_name + ".md>");
		if (GUI.Button(new Rect(5,60,100,40), "Save")) // Save Map object into file
		{
			if (Directory.Exists(Application.dataPath + "/Maps"))
			{
				Save();
			}
			else
			{
				Debug.LogWarning("There is no path (" + Application.dataPath + "/Maps), so data will not be saved");
			}
		}
		if (GUI.Button(new Rect(5,100,100,40), "Save and Test"))
		{
			// TODO: do save and run file
			//print("Saved: " + Application.dataPath + "/Maps/" + map_name + ".md");
		}
		if (GUI.Button(new Rect(20,Screen.height-25,100,20), "Tile"))
		{
			paint_mode_index = 0;
		}
		if (GUI.Button(new Rect(20,Screen.height-45,100,20), "Spawn"))
		{
			paint_mode_index = 1;
		}
		if (GUI.Button(new Rect(20,Screen.height-65,100,20), "n/a"))
		{
		}
		if (GUI.Button(new Rect(20,Screen.height-85,100,20), "n/a"))
		{
		}
		if (GUI.Button(new Rect(20,Screen.height-105,100,20), "n/a"))
		{
		}
		GUI.Label(new Rect(10,Screen.height-25-20*paint_mode_index,10, 20), "x");


		// Right side
		GUI.Label(new Rect(window_rect.width/2, 60, window_rect.width/2-5, 20), "<TAB> - show/hide instructions");
		GUI.Label(new Rect(window_rect.width/2, 80, window_rect.width/2-5, 20), "<Arrow Keys> - move camera around");
		GUI.Label(new Rect(window_rect.width/2, 100, window_rect.width/2-5, 20), "<Mouse Scroll> - Zoom in/out");
		GUI.Label(new Rect(window_rect.width/2, 120, window_rect.width/2-5, 20), "<Q> to paste at mouse position");
		GUI.Label(new Rect(window_rect.width/2, 140, window_rect.width/2-5, 20), "<W> to disable at mouse position");
	}


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab) && GetComponent<TileManager>().read_done)
		{
			show_tab = !show_tab;
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			transform.position += new Vector3(camera_speed,0,0);
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			transform.position += new Vector3(-camera_speed,0,0);
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			transform.position += new Vector3(0,camera_speed,0);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			transform.position += new Vector3(0,-camera_speed,0);
		}
		manageZoom();
		managePaint();
	}

	void manageZoom()
	{
		if (Input.GetAxis("Mouse ScrollWheel") <= 0 || Time.deltaTime < 0.15f)
		{
			GetComponent<Camera>().orthographicSize += Input.GetAxis("Mouse ScrollWheel");
		}
		else
		{
			Debug.LogWarning("Restricted from zooming out due to lag! Frame rate: " + 1/Time.deltaTime);
		}
		if (GetComponent<Camera>().orthographicSize < 0)
		{
			GetComponent<Camera>().orthographicSize = 0.01f;
		}
		camera_speed = GetComponent<Camera>().orthographicSize/30;
	}


	void managePaint()
	{
		if (GetComponent<TileManager>().read_done)
		{
			int mouseX = (int)(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
			int mouseY = (int)(Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
			tile_indicator.transform.position = new Vector3(mouseX, mouseY, -9);

			if (Input.GetKey(KeyCode.Q))
			{
				if (paint_mode[paint_mode_index] == "Tile")
				{
					GetComponent<TileManager>().UpdateTiles(mouseY, mouseX, true);
				}
				if (paint_mode[paint_mode_index] == "Spawn")
				{
					spawn_point_indicator.transform.position = new Vector2(mouseX,mouseY);
					spawn_point_indicator.renderer.material.color = new Color(1,1,1,0.5f);
				}
			}
			else if (Input.GetKey(KeyCode.W))
			{
				if (paint_mode[paint_mode_index] == "Tile")
				{
					GetComponent<TileManager>().UpdateTiles(mouseY, mouseX, false);
				}
				if (paint_mode[paint_mode_index] == "Spawn")
				{
					spawn_point_indicator.transform.position = new Vector2(mouseX,mouseY);
					spawn_point_indicator.renderer.material.color = new Color(1,1,1,0.5f);
				}
			}
			else
			{
				spawn_point_indicator.renderer.material.color = new Color(1,1,1,1);
			}
		}
	}


	public void SaveScene()
	{
		print(Application.dataPath + "/Scenes/TileEditorScenes/" + map_name + ".unity");
	}


	/* Save()
	 * - Called when "Save Map" button is pressed.
	 * - Gets current tiles from TileManager
	 * - Gets spawn_point coordinates
	 * 
	 */
	private void Save()
	{
		Map new_map = new Map();
		new_map.name = map_name;
		new_map.tiles = GetComponent<TileManager>().tiles;
		new_map.spawn_point = new Vector2((int)spawn_point_indicator.transform.position.x, (int)spawn_point_indicator.transform.position.y);
		
		// Serialize Map object into a file
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.dataPath + "/Maps/" + map_name + ".md");
		bf.Serialize(file, new_map);
		Debug.Log("Saved: " + Application.dataPath + "/Maps/" + map_name + ".md");
		file.Close();
	}
}

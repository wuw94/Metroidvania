using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using System.Threading;

/* TileEditor
 * 
 * IMPORTANT! Do not put this on any object. It will place itself if the scene name is "Tile Editor"
 * 
 */
public class TileEditor : MonoBehaviour
{
	private string map_name = ""; // name of map
	private GameObject selection = null;
	
	public Hashtable indicators = new Hashtable();

	//------------------<Tools>------------------
	private EntityTypes[] tools = new EntityTypes[]{EntityTypes.Tile, EntityTypes.Spawn, EntityTypes.Interactive};
	private int tools_index = 0;

	// Terrains tools
	private int terrain_tool_index = 0;

	// Tile tool
	private string[] tile_types = new string[]{"GrassTile", "Test"};
	private int tile_tool_index = 0;

	// Interactive tools
	private int interactive_tool_index = 0;
	//-----------------</Tools>------------------

	private bool show_tab = true;
	private Rect full_window_rect = new Rect(0,0,Screen.width,Screen.height); // for displaying GUI
	private Rect left_window_rect = new Rect(0,0,Screen.width - 200,Screen.height);
	private Rect right_window_rect = new Rect(Screen.width-200,0,200,Screen.height);
	private float camera_speed;
	private int FPS;


	void Start()
	{
		ReformatGame();
		indicators[EntityTypes.Spawn] = (GameObject)Instantiate(Resources.Load("Prefabs/TileEditor/SpawnPointIndicator", typeof(GameObject)), new Vector3(0,0,0), transform.rotation);
		((GameObject)indicators[EntityTypes.Spawn]).gameObject.renderer.material.color = new Color(1,1,1,0);
		indicators[EntityTypes.Tile] = (GameObject)Instantiate(Resources.Load("Prefabs/TileEditor/TileIndicator", typeof(GameObject)), new Vector3(0,0,0), transform.rotation);
		((GameObject)indicators[EntityTypes.Tile]).gameObject.renderer.material.color = new Color(1,1,1,0.1f);
		indicators[EntityTypes.Interactive] = new List<GameObject>();
		StartCoroutine(UpdateFPS());
	}

	void OnGUI()
	{
		if (GetComponent<TileManager>().read_done)
		{
			if (show_tab)
			{
				left_window_rect = GUI.Window(0, left_window_rect, TabWindowFunction, "Tile Editor");
			}
			else
			{
				int mouseX = (int)(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
				int mouseY = (int)(Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
				GUI.Label(new Rect(Input.mousePosition.x + 10,Screen.height - Input.mousePosition.y + 10,100, 20), "(" + mouseX + ", " + mouseY + ")");
			}
			right_window_rect = GUI.Window(1, right_window_rect, DescriptionWindowFunction, "Info");
		}
		else
		{
			full_window_rect = GUI.Window(0, full_window_rect, IntroWindowFunction, "Tile Editor");
		}
	}


	private IEnumerator UpdateFPS()
	{
		while (true)
		{
			FPS = (int)(1 / Time.deltaTime);
			yield return new WaitForSeconds(0.5f);
		}
	}
	
	/// <summary>
	/// Window shown in the beginning of Tile Editor. Define the map name, and whether to load or create a new Map.
	/// </summary>
	void IntroWindowFunction(int windowID)
	{
		GUI.Label(new Rect(40,20,full_window_rect.width, 40), "Define a map name to load:");
		map_name = GUI.TextField(new Rect(10,40,Screen.width/2,20), map_name);
		
		if (GUI.Button(new Rect(10,60,100,40), "New"))
		{
			GameManager.current_game.progression.maps.Add("TileEditorMap", new Map());
			GetComponent<TileManager>().LoadAll();
			GetComponent<RenderingSystem>().LoadedDone();
		}
		if (GUI.Button(new Rect(Screen.width/2 - 90,60,100,40), "Load"))
		{
			try
			{
				GameManager.current_game.progression.maps.Add("TileEditorMap", new Map(map_name));
			}
			catch
			{
				Debug.LogWarning("Could not load file: " + map_name);
				throw;
			}

			GetComponent<TileManager>().LoadAll((Map)(GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map])); //TODO
			GetComponent<RenderingSystem>().LoadedDone();

			((GameObject)indicators[EntityTypes.Spawn]).transform.position = new Vector2(((Map)(GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map])).spawn_point.x,
			                                                       ((Map)(GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map])).spawn_point.y);
		}
	}



	/// <summary>
	/// Window shown when Tab pressed when editing tile.
	/// </summary>
	void TabWindowFunction(int windowID)
	{
		// Left side
		GUI.Label(new Rect(5,20,full_window_rect.width, 40), "Loaded Map:\n   <" + map_name + ".md>");
		if (GUI.Button(new Rect(5,60,100,40), "Save")) // Save Map object into file
		{
			if (Directory.Exists(Application.dataPath + "/Maps"))
			{
				Save();
			}
			else
			{
				Debug.LogError("There is no path (" + Application.dataPath + "/Maps), so data will not be saved");
			}
		}
		if (GUI.Button(new Rect(5,100,100,40), "Save and Test"))
		{
			if (Directory.Exists(Application.dataPath + "/Maps"))
			{
				Save();
				PlayerPrefs.SetString("Map Name", map_name);
				Application.LoadLevel("LevelTester");
			}
			else
			{
				Debug.LogError("There is no path (" + Application.dataPath + "/Maps), so data will not be saved");
			}
		}

		//-----------------------------<Tier 1 Options>-----------------------------
		if (GUI.Button(new Rect(20,260,100,20), "Terrains"))
		{
			tools_index = 0;
		}
		if (GUI.Button(new Rect(20,280,100,20), "Spawn"))
		{
			tools_index = 1;
		}
		if (GUI.Button(new Rect(20,300,100,20), "Interactive"))
		{
			tools_index = 2;
		}
		if (GUI.Button(new Rect(20,320,100,20), "n/a"))
		{
		}
		if (GUI.Button(new Rect(20,340,100,20), "n/a"))
		{
		}
		if (GUI.Button(new Rect(20,360,100,20), "n/a"))
		{
		}
		if (GUI.Button(new Rect(20,380,100,20), "n/a"))
		{
		}
		if (GUI.Button(new Rect(20,400,100,20), "n/a"))
		{
		}
		if (GUI.Button(new Rect(20,420,100,20), "n/a"))
		{
		}
		GUI.Label(new Rect(10,260+20*tools_index,10, 20), "x");
		//----------------------------</Tier 1 Options>-----------------------------

		GUI.Label(new Rect(20, 160, Screen.width/2-5, 20), "<TAB> - show/hide instructions");
		GUI.Label(new Rect(20, 180, Screen.width/2-5, 20), "<Arrow Keys> - move camera around");
		GUI.Label(new Rect(20, 200, Screen.width/2-5, 20), "<Mouse Scroll> - Zoom in/out");
		GUI.Label(new Rect(20, 220, Screen.width/2-5, 20), "Specific tools details to the right");

		if (tools_index == 0) // Terrain tools
		{
			if (GUI.Button(new Rect(140,260,100,20), "Tile"))
			{
				terrain_tool_index = 0;
			}
			if (GUI.Button(new Rect(140,280,100,20), "n/a"))
			{
			}
			if (terrain_tool_index == 0)
			{
				GUI.Label(new Rect(280, 40, Screen.width/2-5, 20), "Tile <" + tile_types[tile_tool_index] + ">");
				GUI.Label(new Rect(280, 60, Screen.width/2-5, 20), "<Q> to paste at mouse position");
				GUI.Label(new Rect(280, 80, Screen.width/2-5, 20), "<W> to disable at mouse position");
				GUI.Label(new Rect(280, 100, Screen.width/2-5, 20), "- Creates or removes tiles at mouse location");
				GUI.Label(new Rect(280, 120, Screen.width/2-5, 20), "- Switch tile type with second tier option");
				if (GUI.Button(new Rect(260,260,100,20), tile_types[0]))
				{
					tile_tool_index = 0;
				}
				if (GUI.Button(new Rect(260,280,100,20), tile_types[1]))
				{
					tile_tool_index = 1;
				}
				GUI.Label(new Rect(250,260+20*tile_tool_index,10, 20), "x");
			}
			GUI.Label(new Rect(130,260+20*terrain_tool_index,10, 20), "x");
		}
		else if (tools_index == 1)
		{
			GUI.Label(new Rect(280, 40, Screen.width/2-5, 20), "Spawn Location");
			GUI.Label(new Rect(280, 60, Screen.width/2-5, 20), "<Q> or <W> to place at mouse position");
			GUI.Label(new Rect(280, 80, Screen.width/2-5, 20), "- Sets the default spawn location for a player entering this map");
		}
		else if (tools_index == 2)
		{
			if (GUI.Button(new Rect(140,260,100,20), "Lever"))
			{
				interactive_tool_index = 0;
			}
			if (GUI.Button(new Rect(140,280,100,20), "n/a"))
			{
			}
			if (interactive_tool_index == 0)
			{
				GUI.Label(new Rect(280, 40, Screen.width/2-5, 20), "Lever");
				GUI.Label(new Rect(280, 60, Screen.width/2-5, 20), "<Q> to paste at mouse position");
				GUI.Label(new Rect(280, 80, Screen.width/2-5, 20), "<W> to remove at mouse position");
				GUI.Label(new Rect(280, 100, Screen.width/2-5, 20), "- Left click a lever to open a menu for assigning");
				GUI.Label(new Rect(280, 120, Screen.width/2-5, 20), "  the entities affected by a particular lever");
			}
			GUI.Label(new Rect(130,260+20*interactive_tool_index,10, 20), "x");
		}
	}


	void DescriptionWindowFunction(int windowID)
	{
		GUI.Label(new Rect(20, 20, 180, 20), "Tiles in view: " + (RenderingSystem.unit_shown.y - RenderingSystem.unit_shown.x) * (RenderingSystem.unit_shown.w - RenderingSystem.unit_shown.z));
		GUI.Label(new Rect(20, 40, 180, 20), "FPS: " + FPS);
		if (selection != null)
		{
			GUI.Label(new Rect(40, 100, 180, 20), "Press ESC to deselect");
			GUI.Label(new Rect(20, 140, 180, 20), "Position: (" + selection.transform.position.x + ", " + selection.transform.position.y + ")");
			if (selection.name == "LeverIndicator(Clone)")
			{
				GUI.Label(new Rect(10, 80, 180, 20), "Selected: Lever");
			}
			else if (selection.name == "MacroTile(Clone)")
			{
				GUI.Label(new Rect(10, 80, 180, 20), "Selected: Tile");
			}
			else if (selection.name == "SpawnPointIndicator(Clone)")
			{
				GUI.Label(new Rect(10, 80, 180, 20), "Selected: Spawn Location");
			}
		}
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
		manageTool();
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


	void manageTool()
	{
		if (GetComponent<TileManager>().read_done)
		{
			int mouseX = (int)(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
			int mouseY = (int)(Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
			((GameObject)indicators[EntityTypes.Tile]).transform.position = new Vector3(mouseX, mouseY, -9);

			Tool_Selection(mouseX, mouseY);
			Tool_Tile(mouseX, mouseY);
			Tool_Spawn(mouseX, mouseY);
			Tool_Lever(mouseX, mouseY);
		}
	}

	private void Tool_Selection(int mouseX, int mouseY)
	{
		if (Input.GetMouseButton(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
			if (selection == null && hit.collider != null)
			{
				selection = hit.collider.gameObject;
				UnityEditor.Selection.objects = new GameObject[]{selection};
			}
		}
		if (Input.GetKey(KeyCode.Escape))
		{
			selection = null;
			UnityEditor.Selection.objects = new GameObject[0];
		}
	}
	
	private void Tool_Tile(int mouseX, int mouseY)
	{
		if (tools_index == 0 && tile_tool_index == 0) // Tile tool
		{
			if (Input.GetKey(KeyCode.Q))
			{
				RaycastHit2D hit = Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
				if (hit.transform == null)
				{
					GetComponent<TileManager>().UpdateTiles(mouseY, mouseX, true);
				}
			}
			if (Input.GetKey(KeyCode.W))
			{
				GetComponent<TileManager>().UpdateTiles(mouseY, mouseX, false);
			}
		}
	}

	private void Tool_Spawn(int mouseX, int mouseY)
	{
		((GameObject)indicators[EntityTypes.Spawn]).renderer.material.color = new Color(1,1,1,1);
		if (tools_index == 1) // Spawn tool
		{
			if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.W))
			{
				((GameObject)indicators[EntityTypes.Spawn]).transform.position = new Vector2(mouseX,mouseY);
				((Map)(GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map])).spawn_point = new Vector2(mouseX, mouseY);
				((GameObject)indicators[EntityTypes.Spawn]).renderer.material.color = new Color(1,1,1,0.5f);
			}
		}
	}

	private void Tool_Lever(int mouseX, int mouseY)
	{
		if (tools_index == 2 && interactive_tool_index == 0)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				RaycastHit2D hit = Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
				if (hit.transform == null)
				{
					GameObject lever = (GameObject)Instantiate(Resources.Load("Prefabs/TileEditor/LeverIndicator", typeof(GameObject)), new Vector3(mouseX,mouseY, -9), transform.rotation);
					((List<GameObject>)indicators[EntityTypes.Interactive]).Add(lever);
					for (int i = 0; i < ((List<GameObject>)indicators[EntityTypes.Interactive]).Count; i++)
					{
						((List<GameObject>)indicators[EntityTypes.Interactive])[i].GetComponent<LeverIndicator>().info.self = i;
					}

					// we need a way to add in more valuable data (xy position, which platforms it is linked to)
					//((ArrayList)((Map)(GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map])).entity[EntityTypes.Interactive]).Add(new PseudoVector2(lever.transform.position.x, lever.transform.position.y));
				}
			}
			if (Input.GetKey(KeyCode.W))
			{		
				RaycastHit2D hit = Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
				if (hit.transform != null && hit.transform.name == "LeverIndicator(Clone)")
				{
					List<GameObject> interactives = ((List<GameObject>)indicators[EntityTypes.Interactive]);
					GameObject lever = interactives[interactives.IndexOf(hit.transform.gameObject)];
					interactives.Remove(lever);
					Destroy(lever);

					for (int i = 0; i < ((List<GameObject>)indicators[EntityTypes.Interactive]).Count; i++)
					{
						((List<GameObject>)indicators[EntityTypes.Interactive])[i].GetComponent<LeverIndicator>().info.self = i;
					}
				}
			}
		}
	}


	/// <summary>
	/// When TileEditor scene is being run, game needs to be reformatted to allow new editing capabilities
	/// </summary>
	private void ReformatGame()
	{
		Debug.Log("TileEditor Scene detected. Reformatting current_game session...");
		GameManager.current_game = new GameManager();
		GameManager.current_game.progression.loaded_map = "TileEditorMap";
		Debug.Log("Done");
	}


	public void SaveScene()
	{
		print(Application.dataPath + "/Scenes/TileEditorScenes/" + map_name + ".unity");
	}


	/// <summary>
	/// Called when "Save Map" button is pressed.
	/// </summary>
	private void Save()
	{
		// Serialize Map object into a file
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.dataPath + "/Maps/" + map_name + ".md");
		bf.Serialize(file, GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map]);
		Debug.Log("Saved: " + Application.dataPath + "/Maps/" + map_name + ".md");
		file.Close();
	}
}

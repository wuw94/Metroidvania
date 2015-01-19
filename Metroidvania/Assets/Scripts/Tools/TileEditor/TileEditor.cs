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
	public class Tool
	{
		public int index = 0;
	}
	public class Tools : Tool
	{
		public TerrainTool terrain_tool = new TerrainTool();
		public SpawnTool spawn_tool = new SpawnTool();
		public InteractiveTool interactive_tool = new InteractiveTool();

		public class TerrainTool : Tool
		{
			public TileTool tile_tool = new TileTool();
		
			public class TileTool : Tool
			{
			}
		}

		public class SpawnTool : Tool
		{
		}

		public class InteractiveTool : Tool
		{
			public LeverTool lever_tool = new LeverTool();

			public class LeverTool : Tool
			{
				public bool adding = false;
				public List<string> addable = new List<string>{"LeverIndicator(Clone)"};
			}
		}

	}


















	private string map_name = ""; // name of map

	// Selection
	public GameObject selection = null;
	private List<string> possible_selection = new List<string>(){"SpawnPointIndicator(Clone)","LeverIndicator(Clone)"};
	private bool is_dragging = false;
	private Vector2 click_offset = new Vector2(0.5f,0.5f);


	public Dictionary<EntityTypes, List<GameObject>> indicators = new Dictionary<EntityTypes, List<GameObject>>();

	private Tools tools = new Tools();

	private string[] tile_types = new string[]{"GrassTile", "Test"};

	private bool show_tab = true;
	private Rect full_window_rect = new Rect(0,0,Screen.width,Screen.height); // for displaying GUI
	private Rect left_window_rect = new Rect(0,0,Screen.width - 200,Screen.height);
	private Rect right_window_rect = new Rect(Screen.width-200,0,200,Screen.height);
	private float camera_speed;
	private int FPS;


	void Start()
	{
		ReformatGame();
		foreach (EntityTypes t in System.Enum.GetValues(typeof(EntityTypes)))
		{
			indicators.Add(t, new List<GameObject>());
		}
		indicators[EntityTypes.Spawn].Add((GameObject)Instantiate(Resources.Load("Prefabs/TileEditor/SpawnPointIndicator", typeof(GameObject)), new Vector3(0,0,0), transform.rotation));
		indicators[EntityTypes.Spawn][0].gameObject.renderer.material.color = new Color(1,1,1,0);
		indicators[EntityTypes.Tile].Add((GameObject)Instantiate(Resources.Load("Prefabs/TileEditor/TileIndicator", typeof(GameObject)), new Vector3(0,0,0), transform.rotation));
		indicators[EntityTypes.Tile][0].gameObject.renderer.material.color = new Color(1,1,1,0.1f);
		//indicators[EntityTypes.Interactive] = new List<GameObject>();
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
			if (selection != null)
			{
				right_window_rect = GUI.Window(1, right_window_rect, DescriptionWindowFunction, "Info");
			}
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

			GetComponent<TileManager>().LoadAll(GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map]);
			GetComponent<RenderingSystem>().LoadedDone();

			indicators[EntityTypes.Spawn][0].transform.position = new Vector2(GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].spawn_point.x,
			                                                                  GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].spawn_point.y);
		}
	}



	/// <summary>
	/// Window shown when Tab pressed when editing tile.
	/// </summary>
	void TabWindowFunction(int windowID)
	{
		GUI.Label(new Rect(600, 20, 180, 20), "Tiles in view: " + (RenderingSystem.unit_shown.y - RenderingSystem.unit_shown.x) * (RenderingSystem.unit_shown.w - RenderingSystem.unit_shown.z));
		GUI.Label(new Rect(600, 40, 180, 20), "FPS: " + FPS);
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
			tools.index = 0;
		}
		if (GUI.Button(new Rect(20,280,100,20), "Spawn"))
		{
			tools.index = 1;
		}
		if (GUI.Button(new Rect(20,300,100,20), "Interactive"))
		{
			tools.index = 2;
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
		GUI.Label(new Rect(10,260+20*tools.index,10, 20), "x");
		//----------------------------</Tier 1 Options>-----------------------------

		GUI.Label(new Rect(20, 160, Screen.width/2-5, 20), "<TAB> - show/hide instructions");
		GUI.Label(new Rect(20, 180, Screen.width/2-5, 20), "<Arrow Keys> - move camera around");
		GUI.Label(new Rect(20, 200, Screen.width/2-5, 20), "<Mouse Scroll> - Zoom in/out");
		GUI.Label(new Rect(20, 220, Screen.width/2-5, 20), "Specific tools details to the right");

		if (tools.index == 0) // Terrain tools
		{
			if (GUI.Button(new Rect(140,260,100,20), "Tile"))
			{
				tools.terrain_tool.index = 0;
			}
			if (GUI.Button(new Rect(140,280,100,20), "n/a"))
			{
			}
			if (tools.terrain_tool.index == 0)
			{
				GUI.Label(new Rect(280, 40, Screen.width/2-5, 20), "Tile <" + TileManager.tile_type[tools.terrain_tool.tile_tool.index] + ">");
				GUI.Label(new Rect(280, 60, Screen.width/2-5, 20), "<Q> to paste at mouse position");
				GUI.Label(new Rect(280, 80, Screen.width/2-5, 20), "<W> to disable at mouse position");
				GUI.Label(new Rect(280, 100, Screen.width/2-5, 20), "- Creates or removes tiles at mouse location");
				GUI.Label(new Rect(280, 120, Screen.width/2-5, 20), "- Switch tile type with second tier option");
				for (int i = 0; i < TileManager.tile_type.Length; i++)
				{
					if (GUI.Button(new Rect(260,260 + i*20,100,20), TileManager.tile_type[i]))
					{
						tools.terrain_tool.tile_tool.index = i;
					}
				}
				GUI.Label(new Rect(250,260+20*tools.terrain_tool.tile_tool.index,10, 20), "x");
			}
			GUI.Label(new Rect(130,260+20*tools.terrain_tool.index,10, 20), "x");
		}
		else if (tools.index == 1)
		{
			GUI.Label(new Rect(280, 40, Screen.width/2-5, 20), "Spawn Location");
			GUI.Label(new Rect(280, 60, Screen.width/2-5, 20), "<Q> or <W> to place at mouse position");
			GUI.Label(new Rect(280, 80, Screen.width/2-5, 20), "- Sets the default spawn location for a player entering this map");
		}
		else if (tools.index == 2)
		{
			if (GUI.Button(new Rect(140,260,100,20), "Lever"))
			{
				tools.interactive_tool.index = 0;
			}
			if (GUI.Button(new Rect(140,280,100,20), "n/a"))
			{
			}
			if (tools.interactive_tool.index == 0)
			{
				GUI.Label(new Rect(280, 40, Screen.width/2-5, 20), "Lever");
				GUI.Label(new Rect(280, 60, Screen.width/2-5, 20), "<Q> to paste at mouse position");
				GUI.Label(new Rect(280, 80, Screen.width/2-5, 20), "<W> to remove at mouse position");
				GUI.Label(new Rect(280, 100, Screen.width/2-5, 20), "- Left click a lever to open a menu for assigning");
				GUI.Label(new Rect(280, 120, Screen.width/2-5, 20), "  the entities affected by a particular lever");
			}
			GUI.Label(new Rect(130,260+20*tools.interactive_tool.index,10, 20), "x");
		}
		GUI.Label(new Rect(10,left_window_rect.height - 20, 500, 20), "Memory Allocated: " + System.GC.GetTotalMemory(false).ToString());
	}


	void DescriptionWindowFunction(int windowID)
	{
		if (selection != null)
		{
			GUI.Label(new Rect(40, 40, 180, 20), "Press ESC to deselect");
			GUI.Label(new Rect(20, 80, 180, 20), "Position: (" + selection.transform.position.x + ", " + selection.transform.position.y + ")");
			if (selection.name == "LeverIndicator(Clone)")
			{
				GUI.Label(new Rect(10, 20, 180, 20), "Selected: Lever");
				GUI.Label(new Rect(10, 120, 180, 20), "Affects: " + selection.GetComponent<LeverIndicator>().affecting.Count + " items");
				if (GUI.Button(new Rect(140,124,38,16), tools.interactive_tool.lever_tool.adding ? "?" : "add"))
				{
					tools.interactive_tool.lever_tool.adding = !tools.interactive_tool.lever_tool.adding;
				}
				for (int i = 0; i < selection.GetComponent<LeverIndicator>().affecting.Count; i++)
				{
					GUI.Label(new Rect(20, 140 + 20*i, 180, 20), "Item(" + selection.GetComponent<LeverIndicator>().affecting[i].transform.position.x + ", " + selection.GetComponent<LeverIndicator>().affecting[i].transform.position.y + ")");
					if (GUI.Button(new Rect(150,144 + 20*i,20,16), "x"))
					{
						selection.GetComponent<LeverIndicator>().affecting.RemoveAt(i);
					}
				}
			}
			else if (selection.name == "SpawnPointIndicator(Clone)")
			{
				GUI.Label(new Rect(10, 20, 180, 20), "Selected: Spawn Location");
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
		if (Input.GetKey(KeyCode.Minus))
		{
			Camera.main.orthographicSize -= 2;
		}
		else if (Input.GetKey(KeyCode.Equals) && Time.deltaTime < 0.15f)
		{
			Camera.main.orthographicSize += 2;
		}
		else if (Input.GetAxis("Mouse ScrollWheel") <= 0 || Time.deltaTime < 0.15f)
		{
			Camera.main.orthographicSize += Input.GetAxis("Mouse ScrollWheel");
		}
		else
		{
			Debug.LogWarning("Restricted from zooming out due to lag! Frame rate: " + 1/Time.deltaTime);
		}
		if (Camera.main.orthographicSize < 0)
		{
			Camera.main.orthographicSize = 0.01f;
		}
		camera_speed = Camera.main.orthographicSize/30;
	}


	void manageTool()
	{
		if (GetComponent<TileManager>().read_done)
		{
			Vector2 mouse = new Vector2((int)(Camera.main.ScreenToWorldPoint(Input.mousePosition).x),
			                            (int)(Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
			indicators[EntityTypes.Tile][0].transform.position = new Vector3(mouse.x, mouse.y, -9);

			Tool_Selection(mouse);
			Tool_Tile(mouse);
			Tool_Spawn(mouse);
			Tool_Lever(mouse);
		}
	}

	private void Tool_Selection(Vector2 mouse)
	{
		if (Input.GetMouseButton(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);

			if (tools.interactive_tool.lever_tool.adding)
			{
				if (hit.collider != null && tools.interactive_tool.lever_tool.addable.Contains(hit.collider.name))
				{
					tools.interactive_tool.lever_tool.adding = false;
					selection.GetComponent<LeverIndicator>().affecting.Add(hit.collider.gameObject);
				}
			}
			else
			{
			
				if (!is_dragging && selection != null && hit.collider != null && selection.Equals(hit.collider.gameObject))
				{
					is_dragging = true;
				}
				if (selection == null && hit.collider != null)
				{
					if (possible_selection.Contains(hit.collider.gameObject.name))
					{
						selection = hit.collider.gameObject;
						UnityEditor.Selection.objects = new GameObject[]{selection};
					}
				}
				if (is_dragging)
				{
					selection.transform.position = new Vector3((Camera.main.ScreenToWorldPoint(Input.mousePosition).x - click_offset.x),
					                                           (Camera.main.ScreenToWorldPoint(Input.mousePosition).y - click_offset.y),
					                                           selection.transform.position.z);
				}
			}
		}
		else
		{
			is_dragging = false;
			if (selection != null)
			{
				selection.transform.position = new Vector3(Mathf.RoundToInt(selection.transform.position.x),
				                                           Mathf.RoundToInt(selection.transform.position.y),
				                                           selection.transform.position.z);
			}
		}
		if (Input.GetKey(KeyCode.Escape))
		{
			selection = null;
			UnityEditor.Selection.objects = new GameObject[0];
		}
	}
	
	private void Tool_Tile(Vector2 mouse)
	{
		if (tools.index == 0 && tools.terrain_tool.tile_tool.index == 0) // Tile tool
		{
			if (Input.GetKey(KeyCode.Q))
			{
				RaycastHit2D hit = Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
				if (hit.transform == null)
				{
					GetComponent<TileManager>().UpdateTiles(mouse, true);
				}
			}
			if (Input.GetKey(KeyCode.W))
			{
				GetComponent<TileManager>().UpdateTiles(mouse, false);
			}
		}
	}

	private void Tool_Spawn(Vector2 mouse)
	{
		indicators[EntityTypes.Spawn][0].renderer.material.color = new Color(1,1,1,1);
		if (tools.index == 1) // Spawn tool
		{
			if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.W))
			{
				indicators[EntityTypes.Spawn][0].transform.position = mouse;
				((Map)(GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map])).spawn_point = mouse;
				indicators[EntityTypes.Spawn][0].renderer.material.color = new Color(1,1,1,0.5f);
			}
		}
	}

	private void Tool_Lever(Vector2 mouse)
	{
		if (tools.index == 2 && tools.interactive_tool.index == 0)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				RaycastHit2D hit = Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
				if (hit.transform == null)
				{
					GameObject lever = (GameObject)Instantiate(Resources.Load("Prefabs/TileEditor/LeverIndicator", typeof(GameObject)), new Vector3(mouse.x,mouse.y, -9), transform.rotation);
					indicators[EntityTypes.Interactive].Add(lever);

					// we need a way to add in more valuable data (xy position, which platforms it is linked to)
					//((ArrayList)((Map)(GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map])).entity[EntityTypes.Interactive]).Add(new PseudoVector2(lever.transform.position.x, lever.transform.position.y));
				}
			}
			if (Input.GetKey(KeyCode.W))
			{		
				RaycastHit2D hit = Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
				if (hit.transform != null && hit.transform.name == "LeverIndicator(Clone)")
				{
					List<GameObject> interactives = indicators[EntityTypes.Interactive];
					GameObject lever = interactives[interactives.IndexOf(hit.transform.gameObject)];
					interactives.Remove(lever);
					Destroy(lever);
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

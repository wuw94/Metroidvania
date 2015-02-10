using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;


/* TileEditor
 * 
 * IMPORTANT! Do not put this on any object. It will place itself if the scene name is "Tile Editor"
 * 
 */
public sealed class TileEditor : MonoBehaviour
{
	public class Tool
	{
		public int index = 0;
	}
	public sealed class Tools : Tool
	{
		public TerrainTool terrain_tool = new TerrainTool();
		public SpawnTool spawn_tool = new SpawnTool();
		public InteractiveTool interactive_tool = new InteractiveTool();
		public EntityTool entity_tool = new EntityTool();
		public TextGeneratorTool text_generator_tool = new TextGeneratorTool();

		public sealed class TerrainTool : Tool
		{
			public TileTool tile_tool = new TileTool();
		
			public sealed class TileTool : Tool
			{
			}
		}

		public sealed class SpawnTool : Tool
		{
		}

		public sealed class InteractiveTool : Tool
		{
			public LeverTool lever_tool = new LeverTool();
			public DependantPlatformTool dependant_platform_tool = new DependantPlatformTool();
			public ButtonTool button_tool = new ButtonTool();
			public ItemTool item_tool = new ItemTool();

			public sealed class LeverTool : Tool
			{
				public bool adding = false;
				public List<string> addable = new List<string>{"DependantPlatform(Clone)"};
			}

			public sealed class DependantPlatformTool : Tool
			{
			}

			public sealed class ButtonTool : Tool
			{
				public bool adding = false;
				public List<string> addable = new List<string>{"DependantPlatform(Clone)"};
			}

			public sealed class ItemTool : Tool
			{

			}
		}

		public sealed class EntityTool : Tool
		{
			public UpdraftGooTool updraft_goo_tool = new UpdraftGooTool();

			public sealed class UpdraftGooTool : Tool
			{
			}
		}

		public sealed class TextGeneratorTool : Tool
		{
		}

	}
















	private string map_name = ""; // name of map

	// Selection
	public GameObject selection = null;
	private List<string> possible_selection = new List<string>(){"Player(Clone)","Lever(Clone)", "Button(Clone)"};
	private bool is_dragging = false;
	private Vector2 click_offset = new Vector2(0.5f,0.5f);
	
	public GameObject grid_highlight;

	private Tools tools = new Tools();
	
	private bool show_tab = true;
	private Rect full_window_rect = new Rect(0,0,Screen.width,Screen.height); // for displaying GUI
	private Rect left_window_rect = new Rect(0,0,Screen.width - 200,Screen.height);
	private Rect right_window_rect = new Rect(Screen.width-200,0,200,Screen.height);
	private float camera_speed;
	private int FPS;


	void Start()
	{
		ReformatGame();
		grid_highlight = (GameObject)Instantiate(Resources.Load("Prefabs/TileEditor/GridHighlight", typeof(GameObject)), new Vector3(0,0,0), transform.rotation);
		grid_highlight.renderer.material.color = new Color(1,1,1,0.1f);


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
		if (GUI.Button(new Rect(20,320,100,20), "Entity"))
		{
			tools.index = 3;
		}
		if (GUI.Button(new Rect(20,340,100,20), "TextGenerator"))
		{
			tools.index = 4;
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
			if (GUI.Button(new Rect(140,280,100,20), "DependantPlatform"))
			{
				tools.interactive_tool.index = 1;
			}
			if (GUI.Button(new Rect(140, 300,100,20), "Button"))
			{
				tools.interactive_tool.index = 2;
			}
			if (GUI.Button(new Rect(140, 320,100,20), "Item"))
			{
				tools.interactive_tool.index = 3;
			}
			if (tools.interactive_tool.index == 0)
			{
				GUI.Label(new Rect(280, 40, Screen.width/2-5, 20), "Lever");
				GUI.Label(new Rect(280, 60, Screen.width/2-5, 20), "<Q> to paste at mouse position");
				GUI.Label(new Rect(280, 80, Screen.width/2-5, 20), "<W> to remove at mouse position");
				GUI.Label(new Rect(280, 100, Screen.width/2-5, 20), "- Left click a lever to open a menu for assigning");
				GUI.Label(new Rect(280, 120, Screen.width/2-5, 20), "  the entities affected by a particular lever");
			}
			if (tools.interactive_tool.index == 1)
			{
				GUI.Label(new Rect(280, 40, Screen.width/2-5, 20), "DependantPlatform");
				GUI.Label(new Rect(280, 60, Screen.width/2-5, 20), "<Q> to paste at mouse position");
				GUI.Label(new Rect(280, 80, Screen.width/2-5, 20), "<W> to remove at mouse position");
				GUI.Label(new Rect(280, 100, Screen.width/2-5, 20), "- Creates a platform which is affected by");
				GUI.Label(new Rect(280, 120, Screen.width/2-5, 20), "  a lever");
			}
			if (tools.interactive_tool.index == 2)
			{
				GUI.Label(new Rect(280, 40, Screen.width/2-5, 20), "Button");
				GUI.Label(new Rect(280, 60, Screen.width/2-5, 20), "<Q> to paste at mouse position");
				GUI.Label(new Rect(280, 80, Screen.width/2-5, 20), "<W> to remove at mouse position");
				GUI.Label(new Rect(280, 100, Screen.width/2-5, 20), "- Left click a button to open a menu for assigning");
				GUI.Label(new Rect(280, 120, Screen.width/2-5, 20), "  the entities affected by a particular button");
			}
			if (tools.interactive_tool.index == 3)
			{
				GUI.Label(new Rect(280, 40, Screen.width/2-5, 20), "Item");
				GUI.Label(new Rect(280, 60, Screen.width/2-5, 20), "<Q> to paste at mouse position");
				GUI.Label(new Rect(280, 80, Screen.width/2-5, 20), "<W> to remove at mouse position");
				GUI.Label(new Rect(280, 100, Screen.width/2-5, 20), "- Item, obtainable by player touching");
				GUI.Label(new Rect(280, 120, Screen.width/2-5, 20), "  change type of item through unity editor");
			}
			GUI.Label(new Rect(130,260+20*tools.interactive_tool.index,10, 20), "x");
		}
		else if (tools.index == 3)
		{
			if (GUI.Button(new Rect(140,260,100,20), "Updraft Goo"))
			{
				tools.entity_tool.index = 0;
			}
			if (GUI.Button(new Rect(140,280,100,20), "n/a"))
			{
			}
			if (tools.entity_tool.index == 0)
			{
				GUI.Label(new Rect(280, 40, Screen.width/2-5, 20), "Updraft Goo");
				GUI.Label(new Rect(280, 60, Screen.width/2-5, 20), "<Q> to paste at mouse position");
				GUI.Label(new Rect(280, 80, Screen.width/2-5, 20), "<W> to remove at mouse position");
				GUI.Label(new Rect(280, 100, Screen.width/2-5, 20), "- Sets the spawn location for a new Updraft Goo");
			}
			GUI.Label(new Rect(130,260+20*tools.entity_tool.index,10, 20), "x");
		}
		else if (tools.index == 4)
		{
			GUI.Label(new Rect(280, 40, Screen.width/2-5, 20), "Text Generator");
			GUI.Label(new Rect(280, 60, Screen.width/2-5, 20), "<Q> to place at mouse position");
			GUI.Label(new Rect(280, 80, Screen.width/2-5, 20), "<W> to remove at mouse position");
			GUI.Label(new Rect(280, 100, Screen.width/2-5, 20), "- Creates a module that generates text when player is nearby");
		}
	}


	/// <summary>
	/// Displays information about the current selected object.
	/// </summary>
	/// <param name="windowID">Window I.</param>
	void DescriptionWindowFunction(int windowID)
	{
		if (selection != null)
		{
			GUI.Label(new Rect(40, 40, 180, 20), "Press ESC to deselect");
			GUI.Label(new Rect(20, 80, 180, 20), "Position: (" + selection.transform.position.x + ", " + selection.transform.position.y + ")");
			if (selection.name == "Lever(Clone)")
			{
				GUI.Label(new Rect(10, 20, 180, 20), "Selected: Lever");
				GUI.Label(new Rect(10, 120, 180, 20), "Affects: " + selection.GetComponent<Lever>().platforms.Count + " items");
				if (GUI.Button(new Rect(140,124,38,16), tools.interactive_tool.lever_tool.adding ? "?" : "add"))
				{
					tools.interactive_tool.lever_tool.adding = !tools.interactive_tool.lever_tool.adding;
				}
				for (int i = 0; i < selection.GetComponent<Lever>().platforms.Count; i++)
				{
					try
					{
						GUI.Label(new Rect(20, 140 + 20*i, 180, 20), "Item(" +
						          ((DependantPlatform)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[3][selection.GetComponent<Lever>().platforms[i]]).transform.position.x + 
						          ", " + 
						          ((DependantPlatform)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[3][selection.GetComponent<Lever>().platforms[i]]).transform.position.y +
						          ")");
					}
					catch (System.ArgumentOutOfRangeException e)
					{
						selection.GetComponent<Lever>().platforms.RemoveAt(i);
					}
					if (GUI.Button(new Rect(150,144 + 20*i,20,16), "x"))
					{
						selection.GetComponent<Lever>().platforms.RemoveAt(i);
					}
				}
			}
			else if (selection.name == "Button(Clone)")
			{
				GUI.Label(new Rect(10, 20, 180, 20), "Selected: Button");
				GUI.Label(new Rect(10, 120, 180, 20), "Affects: " + selection.GetComponent<Button>().platforms.Count + " items");
				if (GUI.Button(new Rect(140,124,38,16), tools.interactive_tool.button_tool.adding ? "?" : "add"))
				{
					tools.interactive_tool.button_tool.adding = !tools.interactive_tool.button_tool.adding;
				}
				for (int i = 0; i < selection.GetComponent<Button>().platforms.Count; i++)
				{
					try
					{
						GUI.Label(new Rect(20, 140 + 20*i, 180, 20), "Item(" +
						          ((DependantPlatform)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[3][selection.GetComponent<Button>().platforms[i]]).transform.position.x + 
						          ", " + 
						          ((DependantPlatform)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[3][selection.GetComponent<Button>().platforms[i]]).transform.position.y +
						          ")");
					}
					catch (System.ArgumentOutOfRangeException e)
					{
						selection.GetComponent<Button>().platforms.RemoveAt(i);
					}
					if (GUI.Button(new Rect(150,144 + 20*i,20,16), "x"))
					{
						selection.GetComponent<Button>().platforms.RemoveAt(i);
					}
				}
			}
			else if (selection.name == "DependantPlatform(Clone)")
			{
				GUI.Label(new Rect(10, 20, 180, 20), "Selected: DependantPlatform");
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
			grid_highlight.transform.position = new Vector3(mouse.x, mouse.y, -9);

			Tool_Selection(mouse);
			Tool_Tile(mouse);
			Tool_Spawn(mouse, ResourceDirectory.resource[typeof(Player)].index);
			Tool_Lever(mouse, ResourceDirectory.resource[typeof(Lever)].index);
			Tool_Updraft_Goo(mouse, ResourceDirectory.resource[typeof(UpdraftGoo)].index);
			Tool_Dependant_Platform(mouse, ResourceDirectory.resource[typeof(DependantPlatform)].index);
			Tool_Button(mouse, ResourceDirectory.resource[typeof(Button)].index);
			Tool_TextGenerator(mouse, ResourceDirectory.resource[typeof(TextGeneratorEntity)].index);
			Tool_Item(mouse, ResourceDirectory.resource[typeof(Item)].index);
		}
	}

	private void Tool_Selection(Vector2 mouse)
	{
		if (Input.GetMouseButton(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);

			if (tools.interactive_tool.lever_tool.adding)
			{
				if (hit.collider != null && tools.interactive_tool.lever_tool.addable.Contains(hit.collider.name) && hit.collider.GetComponent<DependantPlatform>() != null)
				{
					tools.interactive_tool.lever_tool.adding = false;

					selection.GetComponent<Lever>().platforms.Add(hit.collider.GetComponent<DependantPlatform>().WhatIndexAmI());
				}
			}
			else if (tools.interactive_tool.button_tool.adding)
			{
				if (hit.collider != null && tools.interactive_tool.button_tool.addable.Contains(hit.collider.name) && hit.collider.GetComponent<DependantPlatform>() != null)
				{
					tools.interactive_tool.button_tool.adding = false;
					
					selection.GetComponent<Button>().platforms.Add(hit.collider.GetComponent<DependantPlatform>().WhatIndexAmI());
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
			if (tools.interactive_tool.lever_tool.adding)
			{
				tools.interactive_tool.lever_tool.adding = false;
			}
			else
			{
				selection = null;
				UnityEditor.Selection.objects = new GameObject[0];
			}
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

	private void Tool_Spawn(Vector2 mouse, int type_index)
	{
		((MonoBehaviour)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[type_index][0]).renderer.material.color = new Color(1,1,1,1);
		if (tools.index == 1) // Spawn tool
		{
			if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.W))
			{
				((MonoBehaviour)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[type_index][0]).transform.position = mouse;
				((MonoBehaviour)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[type_index][0]).renderer.material.color = new Color(1,1,1,0.5f);
			}
		}
	}

	private void Tool_Lever(Vector2 mouse, int type_index)
	{
		if (tools.index == 2 && tools.interactive_tool.index == 0)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				if (mouse.x >= 0 && mouse.y >= 0)
				{
					RaycastHit2D hit = Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
					if (hit.transform == null)
					{
						Lever lever = ((GameObject)Instantiate(Resources.Load(ResourceDirectory.resource[System.Type.GetType("Lever")].path, typeof(GameObject)), new Vector3(mouse.x,mouse.y, -9), transform.rotation)).GetComponent<Lever>();
						GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[type_index].Add(lever);
					}
				}
			}
			if (Input.GetKey(KeyCode.W))
			{		
				RaycastHit2D hit = Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
				if (hit.transform != null && hit.transform.gameObject.GetComponent<Lever>() != null)
				{
					ArrayList interactives = GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[type_index];
					Lever lever = (Lever)interactives[interactives.IndexOf(hit.transform.gameObject.GetComponent<Lever>())];
					GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[type_index].Remove(lever);
					Destroy(lever.gameObject);
				}
			}
		}
	}

	private void Tool_Updraft_Goo(Vector2 mouse, int type_index)
	{
		if (tools.index == 3 && tools.entity_tool.index == 0)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				RaycastHit2D hit = Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
				if (hit.transform == null)
				{
					UpdraftGoo updraft_goo = ((GameObject)Instantiate(Resources.Load(ResourceDirectory.resource[typeof(UpdraftGoo)].path, typeof(GameObject)), new Vector3(mouse.x,mouse.y, -9), transform.rotation)).GetComponent<UpdraftGoo>();
					GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[type_index].Add(updraft_goo);
				}
			}
			if (Input.GetKey(KeyCode.W))
			{		
				RaycastHit2D hit = Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
				if (hit.transform != null && hit.transform.gameObject.GetComponent<UpdraftGoo>() != null)
				{
					ArrayList interactives = GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[type_index];
					UpdraftGoo updraft_goo = (UpdraftGoo)interactives[interactives.IndexOf(hit.transform.gameObject.GetComponent<UpdraftGoo>())];
					GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[type_index].Remove(updraft_goo);
					Destroy(updraft_goo.gameObject);
				}
			}

		}
	}

	private void Tool_Dependant_Platform(Vector2 mouse, int type_index)
	{
		if (tools.index == 2 && tools.interactive_tool.index == 1)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				RaycastHit2D hit = Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
				if (hit.transform == null)
				{
					DependantPlatform dependant_platform = ((GameObject)Instantiate(Resources.Load(ResourceDirectory.resource[typeof(DependantPlatform)].path, typeof(GameObject)), new Vector3(mouse.x,mouse.y, -9), transform.rotation)).GetComponent<DependantPlatform>();
					GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[type_index].Add(dependant_platform);
				}
			}
		}
		if (Input.GetKey(KeyCode.W))
		{		
			RaycastHit2D hit = Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
			if (hit.transform != null && hit.transform.gameObject.GetComponent<DependantPlatform>() != null)
			{
				ArrayList interactives = GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[type_index];
				DependantPlatform dependant_platform = (DependantPlatform)interactives[interactives.IndexOf(hit.transform.gameObject.GetComponent<DependantPlatform>())];
				for (int i = 0; i < GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[1].Count; i++)
				{
					((Lever)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[1][i]).platforms.Remove(dependant_platform.WhatIndexAmI());
					for (int j = 0; j < ((Lever)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[1][i]).platforms.Count; j++)
					{
						if (((Lever)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[1][i]).platforms[j] >= dependant_platform.WhatIndexAmI())
						{
							((Lever)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[1][i]).platforms[j]--;
						}
					}
					
				}
				for (int i = 0; i < GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[4].Count; i++)
				{
					((Button)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[4][i]).platforms.Remove(dependant_platform.WhatIndexAmI());
					for (int j = 0; j < ((Button)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[4][i]).platforms.Count; j++)
					{
						if (((Button)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[4][i]).platforms[j] >= dependant_platform.WhatIndexAmI())
						{
							((Button)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[4][i]).platforms[j]--;
						}
					}
					
				}
				GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[type_index].Remove(dependant_platform);
				Destroy(dependant_platform.gameObject);
			}
		}
	}


	private void Tool_Button(Vector2 mouse, int type_index)
	{
		if (tools.index == 2 && tools.interactive_tool.index == 2)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				if (mouse.x >= 0 && mouse.y >= 0)
				{
					RaycastHit2D hit = Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
					if (hit.transform == null)
					{
						Button button = ((GameObject)Instantiate(Resources.Load(ResourceDirectory.resource[typeof(Button)].path, typeof(GameObject)), new Vector3(mouse.x,mouse.y, -9), transform.rotation)).GetComponent<Button>();
						GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[type_index].Add(button);
					}
				}
			}
			if (Input.GetKey(KeyCode.W))
			{		
				RaycastHit2D hit = Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
				if (hit.transform != null && hit.transform.gameObject.GetComponent<Button>() != null)
				{
					ArrayList interactives = GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[type_index];
					Button button = (Button)interactives[interactives.IndexOf(hit.transform.gameObject.GetComponent<Button>())];
					GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[type_index].Remove(button);
					Destroy(button.gameObject);
				}
			}
		}
	}


	private void Tool_TextGenerator(Vector2 mouse, int type_index)
	{
		if (tools.index == 4)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				TextGeneratorEntity text_generator_entity = ((GameObject)Instantiate(Resources.Load(ResourceDirectory.resource[typeof(TextGeneratorEntity)].path, typeof(GameObject)), new Vector3(mouse.x,mouse.y, -9), transform.rotation)).GetComponent<TextGeneratorEntity>();
				GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[type_index].Add(text_generator_entity);
			}
			if (Input.GetKey(KeyCode.W))
			{
				RaycastHit2D hit = Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
				if (hit.transform != null && hit.transform.gameObject.GetComponent<TextGeneratorEntity>() != null)
				{
					ArrayList interactives = GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[type_index];
					TextGeneratorEntity text_generator_entity = (TextGeneratorEntity)interactives[interactives.IndexOf(hit.transform.gameObject.GetComponent<TextGeneratorEntity>())];
					GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[type_index].Remove(text_generator_entity);
					Destroy(text_generator_entity.gameObject);
				}
			}
		}
	}

	private void Tool_Item(Vector2 mouse, int type_index)
	{
		if (tools.index == 2 && tools.interactive_tool.index == 3)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				Item item = ((GameObject)Instantiate(Resources.Load(ResourceDirectory.resource[typeof(Item)].path, typeof(GameObject)), new Vector3(mouse.x,mouse.y, -9), transform.rotation)).GetComponent<Item>();
				GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[type_index].Add(item);
			}
			if (Input.GetKey(KeyCode.W))
			{
				RaycastHit2D hit = Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
				if (hit.transform != null && hit.transform.gameObject.GetComponent<Item>() != null)
				{
					ArrayList interactives = GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[type_index];
					Item item = (Item)interactives[interactives.IndexOf(hit.transform.gameObject.GetComponent<Item>())];
					GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[type_index].Remove(item);
					Destroy(item.gameObject);
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

		if (!Directory.Exists(Application.dataPath + "/Maps")) // Create a directory /Saved if it doesn't already exist
		{
			Directory.CreateDirectory(Application.dataPath + "/Maps");
		}
		FileStream file = File.Create(Application.dataPath + "/Maps/" + map_name + ".md");
		// Serialize Map object into a file
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(file, GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map]);
		Debug.Log("Saved: " + Application.dataPath + "/Maps/" + map_name + ".md");
		file.Close();
		GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].ConvertToGameObject();
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* TileManager.
 * 
 * IMPORTANT! Do not put this script on any object. RenderingSystem will do it for you
 * 
 * TileManger keeps track of all tile rendering, including:
 * - Loading all needed textures from the resources folder
 * - Loading the current map tiles into this class's tile base.
 * - Changing appearance of individual tiles based on its neighbors
 * - Creating or deleting tile GameObjects based on camera size and position
 * - Consumption and updating of the tile buffer
 * 
 */

public sealed class TileManager : RenderingSystem
{
	public static readonly string[] tile_type = new string[]{"GrassTile"};
	private readonly string[] tile_unload = new string[]{"Inner_Concave","Inner_Corner","Inner_Side_Right","Inner_Side_Top","Inner_Surround","Outer_Concave","Outer_Corner","Outer_Side_Right","Outer_Side_Top","Outer_Surround"};
	private readonly Vector2 pivot = new Vector2(0.5f, 0.5f);
	private readonly sbyte[] clockwise_x_logic = new sbyte[]{0,1,1,1,0,-1,-1,-1};
	private readonly sbyte[] clockwise_y_logic = new sbyte[]{1,1,0,-1,-1,-1,0,1};

	private Dictionary<string, Sprite[]> sprites = new Dictionary<string, Sprite[]>();
	private readonly Object tile_resource = Resources.Load("Prefabs/Immobiles/Tiles/MacroTile", typeof(GameObject));
	private List<TileContainer> tile_pool = new List<TileContainer>();
	private Dictionary<Vector2, TileContainer> displayed_tiles = new Dictionary<Vector2, TileContainer>();

	//helper
	private readonly bool[] default_bool = new bool[8];
	private readonly Sprite[] default_sprite = new Sprite[10];
	

	public bool read_done = false;
	private GameObject tile_folder;


	void Start()
	{
		tile_folder = new GameObject();
		tile_folder.name = "Tiles";
	}


	void Update()
	{}


	/* LoadAll()
	 * - Loads a new map
	 */
	public void LoadAll()
	{
		if (read_done)
		{
			Debug.LogWarning("There is already a map");
		}
		else
		{
			LoadResources();
			StartCoroutine(UpdateShownTiles());
			StartCoroutine(UpdateTileBuffer());
			StartCoroutine(UnloadTilePool());
			read_done = true;
		}
	}

	
	/* LoadAll(Map new_map)
	 * - Takes what is needed from a new_map parameter, loads the rest around it.
	 * - Needed: tiles
	 */
	public void LoadAll(Map new_map)
	{
		if (read_done)
		{
			Debug.LogWarning("There is already a map");
		}
		else
		{
			LoadResources();
			StartCoroutine(UpdateShownTiles());
			StartCoroutine(UpdateTileBuffer());
			StartCoroutine(UnloadTilePool());
			read_done = true;
		}
	}


	/* LoadResources()
	 * - Loads all the tile textures we need from the Resources folder
	 * - Refer to tile_unload to see what names to give these files
	 */
	private void LoadResources()
	{
		foreach (string type in tile_type)
		{
			sprites.Add(type, default_sprite);
			for (byte i = 0; i < 10; i++)
			{
				Texture2D t = (Texture2D)Resources.Load("Tiles/"+type+"/"+tile_unload[i]);
				sprites[type][i] = Sprite.Create(t, new Rect(0,0,t.width,t.height), pivot);
			}
		}
	}



	

	

	/// <summary>
	/// Increases the tile_pool to match the amount of tiles we need to display
	/// </summary>
	private IEnumerator UpdateTileBuffer()
	{
		while (true)
		{
			while (tile_pool.Count == 0 || tile_pool.Count + displayed_tiles.Count < (unit_shown.y-unit_shown.x) * (unit_shown.w-unit_shown.z))
			{
				GameObject new_tile = (GameObject)Instantiate(tile_resource);
				tile_pool.Add(new_tile.GetComponent<TileContainer>());
				new_tile.transform.parent = tile_folder.transform;
			}
			yield return null;
		}
	}


	/* UnloadTilePool()
	 * - Slowly destroys tiles that are not being used
	 */
	private IEnumerator UnloadTilePool()
	{
		while (true)
		{
			if (tile_pool.Count > displayed_tiles.Count / 2)
			{
				for (short i = 0; i < tile_pool.Count/2; i++)
				{
					TileContainer t = tile_pool[0];
					tile_pool.RemoveAt(0);
					Destroy(t.gameObject);
				}
			}
			yield return null;
		}
	}


	/* UpdateShownTiles()
	 * - Looks at how much the camera is showing and modifies displayed_tiles and tile_pool
	 * - Changes to the appearance of tiles is covered in this function
	 */
	private IEnumerator UpdateShownTiles()
	{
		while (true)
		{
			Vector2 coordinate;
			for (byte i = (byte)unit_shown.x; i < unit_shown.y; i++)
			{
				for (byte j = (byte)unit_shown.z; j < unit_shown.w; j++)
				{
					coordinate.x = i;
					coordinate.y = j;
					if (!displayed_tiles.ContainsKey(coordinate) && tile_pool.Count > 0)
					{
						displayed_tiles.Add(coordinate, tile_pool[0]);
						tile_pool.RemoveAt(0);
						displayed_tiles[coordinate].SetDisplaying(true);
						displayed_tiles[coordinate].SetSprite(sprites[tile_type[0]]);
						displayed_tiles[coordinate].is_active = GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].tiles[i][j].active;
						displayed_tiles[coordinate].collider2D.enabled = GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].tiles[i][j].active;
						displayed_tiles[coordinate].SetNeighbors(GetNeighbors(coordinate));
						displayed_tiles[coordinate].transform.position = coordinate;
						displayed_tiles[coordinate].updateAll();

						UpdateTiles(coordinate,GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].tiles[i][j].active);
					}
					if (displayed_tiles.ContainsKey(coordinate) && !displayed_tiles[coordinate].IsVisible())
					{
						displayed_tiles[coordinate].SetDisplaying(false);
						tile_pool.Add(displayed_tiles[coordinate]);
						displayed_tiles.Remove(coordinate);
					}
				}
			}
			yield return null;
		}
	}


	/* GetNeighbors(int row, int column)
	 * - Returns a bool[] of neighboring states
	 * - 8 values, clockwise starting at 12:00
	 */
	private bool[] GetNeighbors(Vector2 coordinate)
	{
		bool[] n = default_bool;
		for (byte i = 0; i < 8; i++)
		{
			try {n[i] = GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].tiles[(int)coordinate.x+clockwise_x_logic[i]][(int)coordinate.y+clockwise_y_logic[i]].active;}
			catch {n[i] = false;}
		}
		return n;
	}

	private bool[] GetNeighbors(int x, int y)
	{
		bool[] n = default_bool;
		for (byte i = 0; i < 8; i++)
		{
			try {n[i] = GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].tiles[x+clockwise_x_logic[i]][y+clockwise_y_logic[i]].active;}
			catch {n[i] = false;}
		}
		return n;
	}


	/* UpdateTiles(int row, int column, bool change_to)
	 * - Not to be confused with UpdateShownTiles()
	 * - Call this each time you're changing a tile, so the surrounding tiles know how to change their appearance
	 */
	public void UpdateTiles(Vector2 coordinate, bool change_to)
	{
		if (!displayed_tiles.ContainsKey(coordinate)) {return;}
		GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].tiles[(int)coordinate.x][(int)coordinate.y].active = change_to;
		displayed_tiles[coordinate].is_active = change_to;
		displayed_tiles[coordinate].SetNeighbors(GetNeighbors(coordinate));

		try
		{
			displayed_tiles[coordinate].SetNeighbors(GetNeighbors(coordinate));
			displayed_tiles[coordinate].updateAll();
			for (byte i = 0; i < 8; i++)
			{
				displayed_tiles[new Vector2(coordinate.x+clockwise_x_logic[i],coordinate.y+clockwise_y_logic[i])].SetNeighbors(GetNeighbors((int)coordinate.x+clockwise_x_logic[i],(int)coordinate.y+clockwise_y_logic[i]));
				displayed_tiles[new Vector2(coordinate.x+clockwise_x_logic[i],coordinate.y+clockwise_y_logic[i])].updateAll();
			}
		}
		catch{}
	}
}

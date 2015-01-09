using UnityEngine;
using System.Collections;

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

public class TileManager : RenderingSystem
{
	private ArrayList tile_pool = new ArrayList();
	private Hashtable displayed_tiles = new Hashtable();

	private Hashtable textures = new Hashtable();
	private readonly string[] tile_type = new string[]{"GrassTile"};
	private readonly string[] tile_unload = new string[]{"Inner_Concave","Inner_Corner","Inner_Side_Right","Inner_Side_Top","Inner_Surround","Outer_Concave","Outer_Corner","Outer_Side_Right","Outer_Side_Top","Outer_Surround"};
	private readonly int[] clockwise_row_logic = new int[]{1,1,0,-1,-1,-1,0,1};
	private readonly int[] clockwise_column_logic = new int[]{0,1,1,1,0,-1,-1,-1};

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
			textures.Add(type, new Texture2D[10]);
			for (int i = 0; i < 10; i++)
			{
				((Texture2D[])textures[type])[i] = (Texture2D)Resources.Load("Tiles/"+type+"/"+tile_unload[i]);
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
				GameObject new_tile = (GameObject)Instantiate(Resources.Load("Prefabs/Tiles/MacroTile", typeof(GameObject)), transform.position, transform.rotation);
				tile_pool.Add(new_tile);
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
				for (int i = 0; i < tile_pool.Count/2; i++)
				{
					GameObject t = (GameObject)tile_pool[0];
					tile_pool.RemoveAt(0);
					Destroy(t);
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
			for (int i = (int)unit_shown.x; i < (int)unit_shown.y; i++)
			{
				for (int j = (int)unit_shown.z; j < (int)unit_shown.w; j++)
				{
					Vector2 coordinate = new Vector2(j,i);
					if (!displayed_tiles.Contains(coordinate) && tile_pool.Count > 0)
					{
						displayed_tiles.Add(coordinate, tile_pool[0]);
						tile_pool.RemoveAt(0);
						((GameObject)displayed_tiles[coordinate]).GetComponent<TileContainer>().SetDisplaying(true);
						((GameObject)displayed_tiles[coordinate]).GetComponent<TileContainer>().SetTexture((Texture2D[])textures[tile_type[0]]);
						((GameObject)displayed_tiles[coordinate]).GetComponent<TileContainer>().is_active = ((Map)(GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map])).tiles[j][i].active;
						((GameObject)displayed_tiles[coordinate]).GetComponent<TileContainer>().collider2D.enabled = ((Map)(GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map])).tiles[j][i].active;
						((GameObject)displayed_tiles[coordinate]).GetComponent<TileContainer>().SetNeighbors(GetNeighbors(j,i));
						((GameObject)displayed_tiles[coordinate]).transform.position = new Vector3(i,j,0);
						((GameObject)displayed_tiles[coordinate]).GetComponent<TileContainer>().updateAll();

						UpdateTiles(j,i,((Map)(GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map])).tiles[j][i].active);
					}
					if (displayed_tiles.Contains(coordinate) && !((GameObject)displayed_tiles[coordinate]).GetComponent<TileContainer>().IsVisible())
					{
						((GameObject)displayed_tiles[coordinate]).GetComponent<TileContainer>().SetDisplaying(false);
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
	private bool[] GetNeighbors(int row, int column)
	{
		bool[] n = new bool[8];
		for (int i = 0; i < 8; i++)
		{
			try {n[i] = ((Map)(GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map])).tiles[row+clockwise_row_logic[i]][column+clockwise_column_logic[i]].active;}
			catch {n[i] = false;}
		}
		return n;
	}


	/* UpdateTiles(int row, int column, bool change_to)
	 * - Not to be confused with UpdateShownTiles()
	 * - Call this each time you're changing a tile, so the surrounding tiles know how to change their appearance
	 */
	public void UpdateTiles(int row, int column, bool change_to)
	{
		Vector2 coordinate = new Vector2(row, column);

		if (!displayed_tiles.Contains(coordinate)) {return;}
		((Map)(GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map])).tiles[row][column].active = change_to;

		((GameObject)displayed_tiles[coordinate]).GetComponent<TileContainer>().is_active = change_to;
		((GameObject)displayed_tiles[coordinate]).GetComponent<TileContainer>().SetNeighbors(GetNeighbors(row,column));

		try
		{
			((GameObject)displayed_tiles[new Vector2(row,column)]).GetComponent<TileContainer>().SetNeighbors(GetNeighbors(row,column));
			((GameObject)displayed_tiles[new Vector2(row,column)]).GetComponent<TileContainer>().updateAll();
			for (int i = 0; i < 8; i++)
			{
				((GameObject)displayed_tiles[new Vector2(row+clockwise_row_logic[i],column+clockwise_column_logic[i])]).GetComponent<TileContainer>().SetNeighbors(GetNeighbors(row+clockwise_row_logic[i],column+clockwise_column_logic[i]));
				((GameObject)displayed_tiles[new Vector2(row+clockwise_row_logic[i],column+clockwise_column_logic[i])]).GetComponent<TileContainer>().updateAll();
			}
		}
		catch{}
	}
}

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
	/// <summary>
	/// Self-described, top, side, bottom, depending on surrounding tiles
	/// O : neighboring tile not occupied (Open)
	/// C : neighboring tile occupied (Closed)
	/// </summary>
	private enum CornerCombination
	{
		CCC, CCO,
		COC, COO,
		OCC, OOC
	};

	

	/// <summary>
	/// All the different types of tiles we have in their string form.
	/// These names are used when searching the Resources folder for artwork.
	/// </summary>
	public static readonly string[] tile_types = new string[]{"TestTile", "LabGround"};

	/// <summary>
	/// A sprite with no art on it
	/// </summary>
	public static readonly Sprite empty_sprite = Sprite.Create((Texture2D)Resources.Load("Tiles/GrassTile/Outer_Surround"), new Rect(0,0,100,100),new Vector2(0,0));

	/// <summary>
	/// Artwork into Texture2D into Color[] for a corner, key CornerCombination.
	/// Used when building sprites, 4 corners per tile.
	/// </summary>
	private Dictionary<string, Dictionary<CornerCombination, Color[]>> tile_corner = new Dictionary<string, Dictionary<CornerCombination, Color[]>>();

	/// <summary>
	/// Sprites for all the possible combination of tiles for each tile type.
	/// Built inside TileManager class, then used as a sprite reserve for Tile class.
	/// </summary>
	public static Dictionary<string, Dictionary<TileCombination, Sprite>> tile_sprite = new Dictionary<string, Dictionary<TileCombination, Sprite>>();

	/// <summary>
	/// Preloaded resource for a tile from Resources/Prefabs/Immobiles/Tiles/MacroTile
	/// </summary>
	private readonly Object tile_resource = Resources.Load("Prefabs/Immobiles/Tiles/Tile", typeof(GameObject));
	
	/// <summary>
	/// Pool of tiles that have been created but have fallen out of view and not currently in use.
	/// </summary>
	private List<Tile> tile_pool = new List<Tile>();

	/// <summary>
	/// Tiles that are currently being displayed with its Vector2 coordinates as its key.
	/// </summary>
	private Dictionary<Vector2, Tile> displayed_tiles = new Dictionary<Vector2, Tile>();

	/// <summary>
	/// Folder to keep tiles organized inside the Unity Editor.
	/// </summary>
	private GameObject tile_folder;


	//helper
	private readonly bool[] default_bool = new bool[8];
	private readonly Sprite[] default_sprite = new Sprite[10];
	private static readonly sbyte[] clockwise_x_logic = new sbyte[]{0,1,1,1,0,-1,-1,-1};
	private static readonly sbyte[] clockwise_y_logic = new sbyte[]{1,1,0,-1,-1,-1,0,1};
	
	
	void Start()
	{
		tile_folder = new GameObject();
		tile_folder.name = "Tiles";
	}


	void Update()
	{}


	/// <summary>
	/// Starts TileManager pooling system.
	/// </summary>
	public void BeginChecks()
	{
		LoadResources();
		StartCoroutine(UpdateShownTiles());
		StartCoroutine(UpdateTileBuffer());
		StartCoroutine(UnloadTilePool());
	}

	



	/// <summary>
	/// Loades textures from Resources folder and builds sprites for each possible combination of artwork.
	/// </summary>
	private void LoadResources()
	{
		foreach (string type in tile_types)
		{
			tile_corner[type] = new Dictionary<CornerCombination, Color[]>();
			tile_corner[type][CornerCombination.CCC] = ((Texture2D)Resources.Load("Tiles/" + type + "/CCC")).GetPixels();
			tile_corner[type][CornerCombination.CCO] = ((Texture2D)Resources.Load("Tiles/" + type + "/CCO")).GetPixels();
			tile_corner[type][CornerCombination.COC] = ((Texture2D)Resources.Load("Tiles/" + type + "/COC")).GetPixels();
			tile_corner[type][CornerCombination.COO] = ((Texture2D)Resources.Load("Tiles/" + type + "/COO")).GetPixels();
			tile_corner[type][CornerCombination.OCC] = ((Texture2D)Resources.Load("Tiles/" + type + "/OCC")).GetPixels();
			tile_corner[type][CornerCombination.OOC] = ((Texture2D)Resources.Load("Tiles/" + type + "/OOC")).GetPixels();
			tile_sprite[type] = new Dictionary<TileCombination, Sprite>();
			foreach (TileCombination c in System.Enum.GetValues(typeof(TileCombination)))
			{
				tile_sprite[type].Add(c, BuildSprite(c, type));
			}
		}

	}


	/// <summary>
	/// Build a sprite based on its TileCombination and type of tile
	/// </summary>
	/// <returns>The sprite.</returns>
	/// <param name="combination">Combination.</param>
	/// <param name="type">Type.</param>
	public Sprite BuildSprite(TileCombination combination, string type)
	{
		string code = combination.ToString();

		Texture2D sampletex = (Texture2D)Resources.Load("Tiles/" + type + "/CCC");
		Texture2D tex = new Texture2D(2*sampletex.width,2*sampletex.height);

		//Top left
		if (code[0].Equals('C') && code[1].Equals('C'))
		{
			tex.SetPixels(0,sampletex.height,sampletex.width,sampletex.height,Flip(tile_corner[type][CornerCombination.CCC],sampletex.width,sampletex.height));
		}
		else if (code[0].Equals('C') && code[1].Equals('O'))
		{
			tex.SetPixels(0,sampletex.height,sampletex.width,sampletex.height,Flip(tile_corner[type][CornerCombination.COC],sampletex.width,sampletex.height));
		}
		else if (code[0].Equals('O') && code[1].Equals('C'))
		{
			tex.SetPixels(0,sampletex.height,sampletex.width,sampletex.height,Flip(tile_corner[type][CornerCombination.OCC],sampletex.width,sampletex.height));
		}
		else if (code[0].Equals('O') && code[1].Equals('O'))
		{
			tex.SetPixels(0,sampletex.height,sampletex.width,sampletex.height,Flip(tile_corner[type][CornerCombination.OOC],sampletex.width,sampletex.height));
		}


		//Top right
		if (code[0].Equals('C') && code[2].Equals('C'))
		{
			tex.SetPixels(sampletex.width,sampletex.height,sampletex.width,sampletex.height,tile_corner[type][CornerCombination.CCC]);
		}
		else if (code[0].Equals('C') && code[2].Equals('O'))
		{
			tex.SetPixels(sampletex.width,sampletex.height,sampletex.width,sampletex.height,tile_corner[type][CornerCombination.COC]);
		}
		else if (code[0].Equals('O') && code[2].Equals('C'))
		{
			tex.SetPixels(sampletex.width,sampletex.height,sampletex.width,sampletex.height,tile_corner[type][CornerCombination.OCC]);
		}
		else if (code[0].Equals('O') && code[2].Equals('O'))
		{
			tex.SetPixels(sampletex.width,sampletex.height,sampletex.width,sampletex.height,tile_corner[type][CornerCombination.OOC]);
		}

		//Bottom left
		if (code[3].Equals('C') && code[1].Equals('C'))
		{
			tex.SetPixels(0,0,sampletex.width,sampletex.height,Flip(tile_corner[type][CornerCombination.CCC],sampletex.width,sampletex.height));
		}
		else if (code[3].Equals('C') && code[1].Equals('O'))
		{
			tex.SetPixels(0,0,sampletex.width,sampletex.height,Flip(tile_corner[type][CornerCombination.COC],sampletex.width,sampletex.height));
		}
		else if (code[3].Equals('O') && code[1].Equals('C'))
		{
			tex.SetPixels(0,0,sampletex.width,sampletex.height,Flip(tile_corner[type][CornerCombination.CCO],sampletex.width,sampletex.height));
		}
		else if (code[3].Equals('O') && code[1].Equals('O'))
		{
			tex.SetPixels(0,0,sampletex.width,sampletex.height,Flip(tile_corner[type][CornerCombination.COO],sampletex.width,sampletex.height));
		}

		//Bottom right
		if (code[3].Equals('C') && code[2].Equals('C'))
		{
			tex.SetPixels(sampletex.width,0,sampletex.width,sampletex.height,tile_corner[type][CornerCombination.CCC]);
		}
		else if (code[3].Equals('C') && code[2].Equals('O'))
		{
			tex.SetPixels(sampletex.width,0,sampletex.width,sampletex.height,tile_corner[type][CornerCombination.COC]);
		}
		else if (code[3].Equals('O') && code[2].Equals('C'))
		{
			tex.SetPixels(sampletex.width,0,sampletex.width,sampletex.height,tile_corner[type][CornerCombination.CCO]);
		}
		else if (code[3].Equals('O') && code[2].Equals('O'))
		{
			tex.SetPixels(sampletex.width,0,sampletex.width,sampletex.height,tile_corner[type][CornerCombination.COO]);
		}


		tex.Apply();
		Sprite s = Sprite.Create(tex, new Rect(0,0,400,400), new Vector2(0,0));
		return s;
	}


	/// <summary>
	/// Flip a Color[] for Texture2D across Y axis.
	/// </summary>
	/// <param name="given">Given.</param>
	/// <param name="width">Width.</param>
	/// <param name="height">Height.</param>
	Color[] Flip(Color[] given, int width, int height)
	{
		Color[] to_return = new Color[given.Length];
		for (int h = 0; h < width * height; h += width)
		{
			for (int w = 0; w < width; w++)
			{
				to_return[h + w] = given[h + width-1 - w];
			}
		}
		return to_return;
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
				tile_pool.Add(new_tile.GetComponent<Tile>());
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
					Tile t = tile_pool[0];
					tile_pool.RemoveAt(0);
					Destroy(t.gameObject);
				}
			}
			yield return null;
		}
	}


	/// <summary>
	/// Looks at how much the camera is showing and modifies displayed_tiles and tile_pool.
	/// Updates tile appearance after change.
	/// </summary>
	/// <returns>The shown tiles.</returns>
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
						displayed_tiles[coordinate].is_active = GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].tiles[i][j].active;
						displayed_tiles[coordinate].GetComponent<Collider2D>().enabled = GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].tiles[i][j].active;
						displayed_tiles[coordinate].SetNeighbors(GetNeighbors(coordinate));
						displayed_tiles[coordinate].transform.position = new Vector3(coordinate.x, coordinate.y, 0);
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


	/// <summary>
	/// Returns a bool[] of neighboring states.
	/// States stored in array, clockwise starting at 12:00 index 0
	/// </summary>
	/// <returns>The neighbors.</returns>
	/// <param name="coordinate">Coordinate.</param>
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


	/// <summary>
	/// Returns a bool[] of neighboring states.
	/// States stored in array, clockwise starting at 12:00 index 0
	/// </summary>
	/// <returns>The neighbors.</returns>
	/// <param name="coordinate">Coordinate.</param>
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


	/// <summary>
	/// Called each time a tile is changed, so surrounding tiles know how to change their appearance
	/// </summary>
	/// <param name="coordinate">Coordinate.</param>
	/// <param name="change_to">If set to <c>true</c> change_to.</param>
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

	public void UpdateAll()
	{
		foreach (Vector2 v in displayed_tiles.Keys)
		{
			displayed_tiles[v].updateAll();
		}
	}
}


/// <summary>
/// Self-described, top, left, right, bottom, Closed/Open depending on surrounding tiles
/// </summary>
public enum TileCombination
{
	CCCC, OCCC, CCCO, OCCO,
	CCOC, OCOC, CCOO, OCOO,
	COCC, OOCC, COCO, OOCO,
	COOC, OOOC, COOO, OOOO
};

using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour
{


	private int tile_rows = 100;
	private int tile_columns = 100;
	private TileInfo[][] tiles; // [row][column], contains information about all tiles.
	
	private ArrayList tile_pool = new ArrayList();
	private Hashtable displayed_tiles = new Hashtable();

	private Hashtable textures = new Hashtable();
	private readonly string[] tile_type = new string[]{"GrassTile"};
	private readonly string[] tile_unload = new string[]{"Inner_Concave","Inner_Corner","Inner_Side_Right","Inner_Side_Top","Inner_Surround","Outer_Concave","Outer_Corner","Outer_Side_Right","Outer_Side_Top","Outer_Surround"};
	private readonly int[] clockwise_row_logic = new int[]{1,1,0,-1,-1,-1,0,1};
	private readonly int[] clockwise_column_logic = new int[]{0,1,1,1,0,-1,-1,-1};

	private Vector3 screen_size; // width, height, orthographic size
	private float unit_width;
	private float unit_height;
	
	// x:left, y:right, z:bottom, w:top
	private Vector4 unit_absolute; // amount of tiles that could be shown absolutely (disregard being outside tile bounds)
	private Vector4 unit_shown; // amount of tiles that are shown (considering being outside tile bounds)
	// unit_shown is guaranteed to be within the bounds of row/columns


	
	void Start()
	{
		LoadResources();
		CreateTiles();
	}


	void Update ()
	{
		if (screen_size != new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, Camera.main.orthographicSize))
		{
			OnSizeChanged();
		}
		if (unit_absolute.x != Mathf.FloorToInt(Camera.main.ScreenToWorldPoint(Camera.main.ScreenToViewportPoint(new Vector3(0,0,0))).x) ||
		    unit_absolute.z != Mathf.CeilToInt(Camera.main.ScreenToWorldPoint(Camera.main.ScreenToViewportPoint(new Vector3(0,0,0))).y))
		{
			OnScreenMoved();
		}


		StartCoroutine("UpdateShownTiles");
		StartCoroutine("UpdateTileBuffer");


		if (Application.loadedLevelName == "TileEditor")
		{
			if (Input.GetKey(KeyCode.Q))
			{
				int mouseX = (int)(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
				int mouseY = (int)(Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
				UpdateTiles(mouseY, mouseX, true);
			}
			if (Input.GetKey(KeyCode.W))
			{
				int mouseX = (int)(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
				int mouseY = (int)(Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
				UpdateTiles(mouseY, mouseX, false);
			}
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


	/* UpdateTileBuffer()
	 * - Increases the tile_pool to match the amount of tiles we need to display
	 * - Called once per update
	 */
	private IEnumerator UpdateTileBuffer()
	{
		if (tile_pool.Count + displayed_tiles.Count < (unit_absolute.y-unit_absolute.x) * (unit_absolute.w-unit_absolute.z))
		{
			tile_pool.Add((GameObject)Instantiate(Resources.Load("Prefabs/Tiles/MacroTile", typeof(GameObject)), transform.position, transform.rotation));
		}
		return null;
	}


	/* CreateTiles()
	 * - Creates the TileInfo double array and sets the values to true.
	 * - Later we'll need to modify this function to be able to load tiles from other sources
	 */
	private void CreateTiles()
	{
		tiles = new TileInfo[tile_rows][];
		for (int i = 0; i < tile_rows; i++)
		{
			tiles[i] = new TileInfo[tile_columns];
			for (int j = 0; j < tile_columns; j++)
			{
				tiles[i][j] = new TileInfo(true, 0);
			}
		}
	}


	/* OnSizeChanged()
	 * - When the camera size changes.
	 */
	private void OnSizeChanged()
	{
		screen_size.x = Camera.main.pixelWidth;
		screen_size.y = Camera.main.pixelHeight;
		screen_size.z = Camera.main.orthographicSize;
		unit_height = Camera.main.orthographicSize * 2;
		unit_width = Camera.main.aspect * unit_height;
		UpdateShownTiles();
	}


	/* OnScreenMoved()
	 * - When the camera changes its position
	 */
	private void OnScreenMoved()
	{
		unit_absolute.x = Mathf.FloorToInt(Camera.main.ScreenToWorldPoint(new Vector3(0,0,0)).x);
		unit_absolute.y = Mathf.CeilToInt(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth,Camera.main.pixelHeight,0)).x);
		unit_absolute.z = Mathf.FloorToInt(Camera.main.ScreenToWorldPoint(new Vector3(0,0,0)).y);
		unit_absolute.w = Mathf.CeilToInt(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth,Camera.main.pixelHeight,0)).y);
		unit_shown.x = Mathf.Clamp(unit_absolute.x-1, 0, tile_columns);
		unit_shown.y = Mathf.Clamp(unit_absolute.y+1, 0, tile_columns);
		unit_shown.z = Mathf.Clamp(unit_absolute.z-1, 0, tile_rows);
		unit_shown.w = Mathf.Clamp(unit_absolute.w+1, 0, tile_rows);
		UpdateShownTiles();
	}


	/* UpdateShownTiles()
	 * - Looks at how much the camera is showing and modifies displayed_tiles and tile_pool
	 * - Changes to the appearance of tiles is covered in this function
	 */
	private IEnumerator UpdateShownTiles()
	{
		for (int i = (int)unit_shown.x; i < (int)unit_shown.y; i++)
		{
			for (int j = (int)unit_shown.z; j < (int)unit_shown.w; j++)
			{
				Vector2 coordinate = new Vector2(j,i);
				if (!displayed_tiles.Contains(coordinate) && (tile_pool.Count > 0))
				{
					displayed_tiles.Add(coordinate, tile_pool[0]);
					tile_pool.RemoveAt(0);
					((GameObject)displayed_tiles[coordinate]).GetComponent<TileContainer>().SetDisplaying(true);
					((GameObject)displayed_tiles[coordinate]).GetComponent<TileContainer>().SetTexture((Texture2D[])textures[tile_type[0]]);
					((GameObject)displayed_tiles[coordinate]).GetComponent<TileContainer>().is_active = tiles[j][i].active;
					((GameObject)displayed_tiles[coordinate]).GetComponent<TileContainer>().SetNeighbors(GetNeighbors(j,i));
					((GameObject)displayed_tiles[coordinate]).transform.position = new Vector3(i,j,transform.position.z);
					((GameObject)displayed_tiles[coordinate]).GetComponent<TileContainer>().updateAll();

					UpdateTiles(j,i,tiles[j][i].active);
				}
				if (displayed_tiles.Contains(coordinate) && !((GameObject)displayed_tiles[coordinate]).GetComponent<TileContainer>().IsVisible())
				{
					((GameObject)displayed_tiles[coordinate]).GetComponent<TileContainer>().SetDisplaying(false);
					tile_pool.Add(displayed_tiles[coordinate]);
					displayed_tiles.Remove(coordinate);
				}
			}
		}
		return null;
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
			try {n[i] = tiles[row+clockwise_row_logic[i]][column+clockwise_column_logic[i]].active;}
			catch {n[i] = false;}
		}
		return n;
	}


	/* UpdateTiles(int row, int column, bool change_to)
	 * - Not to be confused with UpdateShownTiles()
	 * - Call this each time you're changing a tile, so the surrounding tiles know how to change their appearance
	 */
	private void UpdateTiles(int row, int column, bool change_to)
	{
		Vector2 coordinate = new Vector2(row, column);

		if (!displayed_tiles.Contains(coordinate)) {return;}
		tiles[row][column].active = change_to;

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

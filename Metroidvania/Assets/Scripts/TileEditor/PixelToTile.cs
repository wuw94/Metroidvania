using UnityEngine;
using System.Collections;

/*
 * Important!
 * This is an outdated class, we're not using this anymore, but keep this here because I want to look at it for reference
 * -Wes
 */

public class PixelToTile : MonoBehaviour
{
	private int tile_rows = 20;
	private int tile_columns = 20;
	private readonly int pixel_to_units = 100;
	private TileInfo[][] tiles; // [row][column]
	private string type = "GrassTile";

	private Texture2D big_texture;
	private Color32[] test_pix;

	private Texture2D i_concave;
	private Texture2D i_corner;
	private Texture2D i_side_right;
	private Texture2D i_side_top;
	private Texture2D i_surround;
	
	private Texture2D o_concave;
	private Texture2D o_corner;
	private Texture2D o_side_right;
	private Texture2D o_side_top;
	private Texture2D o_surround;

	private Color32[] i_concave_pix;
	private Color32[] i_corner_pix;
	private Color32[] i_side_right_pix;
	private Color32[] i_side_top_pix;
	private Color32[] i_surround_pix;
	
	private Color32[] o_concave_pix;
	private Color32[] o_corner_pix;
	private Color32[] o_side_right_pix;
	private Color32[] o_side_top_pix;
	private Color32[] o_surround_pix;




	void Start()
	{
		System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();


		transform.localScale = new Vector3(0.5f, 0.5f, 1); // we scale down because each tile is essentially 2x2 textures
		big_texture = new Texture2D(2 * pixel_to_units * tile_columns, 2 * pixel_to_units * tile_rows);
		test_pix = ((Texture2D)Resources.Load("Tiles/GrassTile/Inner_Corner")).GetPixels32();

		InstantiateAllAsActive();
		InstantiateResources();

		stopwatch.Start();

		UpdateAllTiles();
		stopwatch.Stop();
		Debug.Log("Time taken: " + stopwatch.Elapsed);




	}
	
	void Update()
	{
		//print (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)));
	}


	void OnMouseOver()
	{
		int tempx = (int)Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)).x;
		int tempy = (int)Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)).y;

		if (Input.GetMouseButton(0))
		{
			if (!tiles[tempy][tempx].active)
			{
				tiles[tempy][tempx].active = true;
				UpdateTile(tempy, tempx);
				ApplyTexture();
			}
		}
		if (Input.GetMouseButton(1))
		{
			if (tiles[tempy][tempx].active)
			{
				tiles[tempy][tempx].active = false;
				UpdateTile(tempy, tempx);
				ApplyTexture();
			}
		}
	}

	private void ApplyTexture()
	{
		big_texture.Apply();
		GetComponent<SpriteRenderer>().sprite = Sprite.Create(big_texture,
		                                                      new Rect(0, 0, big_texture.width, big_texture.height),
		                                                      new Vector2(0, 0));
	}


	private void UpdateAllTiles()
	{
		for (int r = 0; r < tile_rows; r++)
		{
			for (int c = 0; c < tile_columns; c++)
			{
				updateTR(r, c);
				updateBR(r, c);
				updateBL(r, c);
				updateTL(r, c);
			}
		}
		ApplyTexture();
	}

	private void InstantiateResources()
	{
		i_concave = (Texture2D)Resources.Load("Tiles/"+type+"/Inner_Concave");
		i_corner = (Texture2D)Resources.Load("Tiles/"+type+"/Inner_Corner");
		i_side_right = (Texture2D)Resources.Load("Tiles/"+type+"/Inner_Side_Right");
		i_side_top = (Texture2D)Resources.Load("Tiles/"+type+"/Inner_Side_Top");
		i_surround = (Texture2D)Resources.Load("Tiles/"+type+"/Inner_Surround");

		o_concave = (Texture2D)Resources.Load("Tiles/"+type+"/Outer_Concave");
		o_corner = (Texture2D)Resources.Load("Tiles/"+type+"/Outer_Corner");
		o_side_right = (Texture2D)Resources.Load("Tiles/"+type+"/Outer_Side_Right");
		o_side_top = (Texture2D)Resources.Load("Tiles/"+type+"/Outer_Side_Top");
		o_surround = (Texture2D)Resources.Load("Tiles/"+type+"/Outer_Surround");

		i_concave_pix = i_concave.GetPixels32();
		i_corner_pix = i_corner.GetPixels32();
		i_side_right_pix = i_side_right.GetPixels32();
		i_side_top_pix = i_side_top.GetPixels32();
		i_surround_pix = i_surround.GetPixels32();

		o_concave_pix = o_concave.GetPixels32();
		o_corner_pix = o_corner.GetPixels32();
		o_side_right_pix = o_side_right.GetPixels32();
		o_side_top_pix = o_side_top.GetPixels32();
		o_surround_pix = o_surround.GetPixels32();
	}

	private void InstantiateAllAsActive()
	{
		tiles = new TileInfo[tile_rows][];
		
		transform.position = new Vector3(0, 0, transform.position.z);
		for (int i = 0; i < tile_rows; i++)
		{
			tiles[i] = new TileInfo[tile_columns];
			for (int j = 0; j < tile_columns; j++)
			{
				tiles[i][j].active = true;
				tiles[i][j].type = 0;
			}
		}
	}




	private void UpdateTile(int row, int column)
	{
		for (int i = row-1; i <= row+1; i++)
		{
			for (int j = column-1; j <= column+1; j++)
			{
				if (i >= 0 && i < tile_rows && j >= 0 && j < tile_columns)
				{
					updateTR(i, j);
					updateBR(i, j);
					updateBL(i, j);
					updateTL(i, j);
				}
			}
		}
	}

	private void draw(int row, int column, int offset_x, int offset_y, Color32[] pix, int rotation)
	{
		// we want to use SetPixels32() later
		if (rotation == 0)
		{
			for (int i = 0; i < pix.Length; i++)
			{
				big_texture.SetPixel(offset_x + 2*column * pixel_to_units + i%pixel_to_units,
				                     offset_y + 2*row * pixel_to_units + i/pixel_to_units,
				                     pix[i]);
			}
		}
		else if (rotation == 90)
		{
			for (int i = 0; i < pix.Length; i++)
			{
				big_texture.SetPixel(offset_x + 2*column * pixel_to_units + i/pixel_to_units,
				                     offset_y + 2*row * pixel_to_units + pixel_to_units - i%pixel_to_units,
				                     pix[i]);
			}
		}
		else if (rotation == 180)
		{
			for (int i = 0; i < pix.Length; i++)
			{
				big_texture.SetPixel(offset_x + 2*column * pixel_to_units + pixel_to_units - i%pixel_to_units,
				                     offset_y + 2*row * pixel_to_units + pixel_to_units - i/pixel_to_units,
				                     pix[i]);
			}
		}
		else if (rotation == -90)
		{
			for (int i = 0; i < pix.Length; i++)
			{
				big_texture.SetPixel(offset_x + 2*column * pixel_to_units + pixel_to_units - i/pixel_to_units,
				                     offset_y + 2*row * pixel_to_units + i%pixel_to_units,
				                     pix[i]);
			}
		}
	}

	private void updateTR(int row, int column)
	{
		int offset_x = 100;
		int offset_y = 100;

		if (tiles[row][column].active)
		{
			int rotation = 0;
			if (row <= 0 || column <= 0 || row >= tile_rows-1 || column >= tile_columns-1)
			{
				draw(row, column, offset_x, offset_y, i_surround_pix, rotation);
			}
			else
			{
				if (!tiles[row+1][column].active && !tiles[row][column+1].active) // corner
				{
					draw(row, column, offset_x, offset_y, i_corner_pix, rotation);
				}
				else if (tiles[row+1][column].active && tiles[row+1][column+1].active && tiles[row][column+1].active) // surround
				{
					draw(row, column, offset_x, offset_y, i_surround_pix, rotation);
				}
				else if (!tiles[row+1][column+1].active && tiles[row+1][column].active && tiles[row][column+1].active) // concave
				{
					draw(row, column, offset_x, offset_y, i_concave_pix, rotation);
				}
				else if (!tiles[row+1][column].active) // top side
				{
					draw(row, column, offset_x, offset_y, i_side_top_pix, rotation);
				}
				else // right side
				{
					draw(row, column, offset_x, offset_y, i_side_right_pix, rotation);
				}
			}
		}
		else
		{
			int rotation = 180;
			if (row == 0 || column == 0 || row == tile_rows-1 || column == tile_columns-1)
			{
				draw(row, column, offset_x, offset_y, o_surround_pix, rotation);
			}
			else
			{
				if (!tiles[row+1][column].active && !tiles[row+1][column+1].active && !tiles[row][column+1].active) // surround
				{
					draw(row, column, offset_x, offset_y, o_surround_pix, rotation);
				}
				else if (tiles[row+1][column].active && tiles[row][column+1].active) // concave
				{
					draw(row, column, offset_x, offset_y, o_concave_pix, rotation);
				}
				else if (!tiles[row+1][column].active && !tiles[row][column+1].active && tiles[row+1][column+1].active) // corner
				{
					draw(row, column, offset_x, offset_y, o_corner_pix, rotation);
				}
				else if (!tiles[row][column+1].active && tiles[row+1][column].active) // bottom
				{
					draw(row, column, offset_x, offset_y, o_side_top_pix, rotation);
				}
				else // left
				{
					draw(row, column, offset_x, offset_y, o_side_right_pix, rotation);
				}
			}
		}
	}


	private void updateBR(int row, int column)
	{
		int offset_x = 100;
		int offset_y = 0;
		if (tiles[row][column].active)
		{
			int rotation = 90;
			if (row == 0 || column == 0 || row == tile_rows-1 || column == tile_columns-1)
			{
				draw(row, column, offset_x, offset_y, i_surround_pix, rotation);
			}
			else
			{
				if (!tiles[row][column+1].active && !tiles[row-1][column].active) // corner
				{
					draw(row, column, offset_x, offset_y, i_corner_pix, rotation);
				}
				else if (tiles[row][column+1].active && tiles[row-1][column+1].active && tiles[row-1][column].active) // surround
				{
					draw(row, column, offset_x, offset_y, i_surround_pix, rotation);
				}
				else if (!tiles[row-1][column+1].active && tiles[row][column+1].active && tiles[row-1][column].active) // concave
				{
					draw(row, column, offset_x, offset_y, i_concave_pix, rotation);
				}
				else if (!tiles[row][column+1].active) // right side
				{
					draw(row, column, offset_x, offset_y, i_side_top_pix, rotation);
				}
				else // bottom side
				{
					draw(row, column, offset_x, offset_y, i_side_right_pix, rotation);
				}
			}
		}
		else
		{
			int rotation = -90;
			if (row == 0 || column == 0 || row == tile_rows-1 || column == tile_columns-1)
			{
				draw(row, column, offset_x, offset_y, o_surround_pix, rotation);
			}
			else
			{
				if (!tiles[row-1][column].active && !tiles[row-1][column+1].active && !tiles[row][column+1].active) // surround
				{
					draw(row, column, offset_x, offset_y, o_surround_pix, rotation);
				}
				else if (tiles[row-1][column].active && tiles[row][column+1].active) // concave
				{
					draw(row, column, offset_x, offset_y, o_concave_pix, rotation);
				}
				else if (!tiles[row-1][column].active && !tiles[row][column+1].active && tiles[row-1][column+1].active) // corner
				{
					draw(row, column, offset_x, offset_y, o_corner_pix, rotation);
				}
				else if (!tiles[row][column+1].active && tiles[row-1][column].active) // bottom
				{
					draw(row, column, offset_x, offset_y, o_side_right_pix, rotation);
				}
				else // left
				{
					draw(row, column, offset_x, offset_y, o_side_top_pix, rotation);
				}
			}
		}
	}

	private void updateBL(int row, int column)
	{
		int offset_x = 0;
		int offset_y = 0;
		if (tiles[row][column].active)
		{
			int rotation = 180;
			if (row == 0 || column == 0 || row == tile_rows-1 || column == tile_columns-1)
			{
				draw(row, column, offset_x, offset_y, i_surround_pix, rotation);
			}
			else
			{
				if (!tiles[row-1][column].active && !tiles[row][column-1].active) // corner
				{
					draw(row, column, offset_x, offset_y, i_corner_pix, rotation);
				}
				else if (tiles[row-1][column].active && tiles[row-1][column-1].active && tiles[row][column-1].active) // surround
				{
					draw(row, column, offset_x, offset_y, i_surround_pix, rotation);
				}
				else if (!tiles[row-1][column-1].active && tiles[row-1][column].active && tiles[row][column-1].active) // concave
				{
					draw(row, column, offset_x, offset_y, i_concave_pix, rotation);
				}
				else if (!tiles[row-1][column].active) // bottom side
				{
					draw(row, column, offset_x, offset_y, i_side_top_pix, rotation);
				}
				else // left side
				{
					draw(row, column, offset_x, offset_y, i_side_right_pix, rotation);
				}
			}
		}
		else
		{
			int rotation = 0;
			if (row == 0 || column == 0 || row == tile_rows-1 || column == tile_columns-1)
			{
				draw(row, column, offset_x, offset_y, o_surround_pix, rotation);
			}
			else
			{
				if (!tiles[row-1][column].active && !tiles[row-1][column-1].active && !tiles[row][column-1].active) // surround
				{
					draw(row, column, offset_x, offset_y, o_surround_pix, rotation);
				}
				else if (tiles[row-1][column].active && tiles[row][column-1].active) // concave
				{
					draw(row, column, offset_x, offset_y, o_concave_pix, rotation);
				}
				else if (!tiles[row-1][column].active && !tiles[row][column-1].active && tiles[row-1][column-1].active) // corner
				{
					draw(row, column, offset_x, offset_y, o_corner_pix, rotation);
				}
				else if (!tiles[row][column-1].active && tiles[row-1][column].active) // top
				{
					draw(row, column, offset_x, offset_y, o_side_top_pix, rotation);
				}
				else // right
				{
					draw(row, column, offset_x, offset_y, o_side_right_pix, rotation);
				}
			}
		}
	}


	private void updateTL(int row, int column)
	{
		int offset_x = 0;
		int offset_y = 100;
		if (tiles[row][column].active)
		{
			int rotation = -90;
			if (row == 0 || column == 0 || row == tile_rows-1 || column == tile_columns-1)
			{
				draw(row, column, offset_x, offset_y, i_surround_pix, rotation);
			}
			else
			{
				if (!tiles[row][column-1].active && !tiles[row+1][column].active) // corner
				{
					draw(row, column, offset_x, offset_y, i_corner_pix, rotation);
				}
				else if (tiles[row][column-1].active && tiles[row+1][column-1].active && tiles[row+1][column].active) // surround
				{
					draw(row, column, offset_x, offset_y, i_surround_pix, rotation);
				}
				else if (!tiles[row+1][column-1].active && tiles[row][column-1].active && tiles[row+1][column].active) // concave
				{
					draw(row, column, offset_x, offset_y, i_concave_pix, rotation);
				}
				else if (!tiles[row][column-1].active) // left side
				{
					draw(row, column, offset_x, offset_y, i_side_top_pix, rotation);
				}
				else // top side
				{
					draw(row, column, offset_x, offset_y, i_side_right_pix, rotation);
				}
			}
		}
		else
		{
			int rotation = 90;
			if (row == 0 || column == 0 || row == tile_rows-1 || column == tile_columns-1)
			{
				draw(row, column, offset_x, offset_y, o_surround_pix, rotation);
			}
			else
			{
				if (!tiles[row][column-1].active && !tiles[row+1][column-1].active && !tiles[row+1][column].active) // surround
				{
					draw(row, column, offset_x, offset_y, o_surround_pix, rotation);
				}
				else if (tiles[row][column-1].active && tiles[row+1][column].active) // concave
				{
					draw(row, column, offset_x, offset_y, o_concave_pix, rotation);
				}
				else if (!tiles[row][column-1].active && !tiles[row+1][column].active && tiles[row+1][column-1].active) // corner
				{
					draw(row, column, offset_x, offset_y, o_corner_pix, rotation);
				}
				else if (!tiles[row+1][column].active && tiles[row][column-1].active) // right
				{
					draw(row, column, offset_x, offset_y, o_side_top_pix, rotation);
				}
				else // bottom
				{
					draw(row, column, offset_x, offset_y, o_side_right_pix, rotation);
				}
			}
		}
	}
}

using UnityEngine;
using System.Collections;

/* TileContainer
 * 
 * 1 unit width, 1 unit height
 * Holds the 4 corners of a tile, dictates which image to display based on information about its neighboring tiles
 * Can change image set based on a given texture pack. Doesn't update itself until UpdateAll() is called
 * 
 * These are created only on start, and only enough to fill up a screen. TileManager will manage which ones are being displayed and
 * give them the correct coordinates, neighbor states, and texture.
 * 
 */

public class TileContainer : MonoBehaviour
{

	private Texture2D[] texture = new Texture2D[10]; // Pack of textures acquired through SetTexture(Texture2D[] t)
	private bool[] neighbors = new bool[8]; // Data about its neighbors acquired through SetNeighbors(bool[] n)

	public bool is_active;
	public bool displaying = false; // Whether this tile is currently being displayed on the screen. If not, it's inside a pool of unused GameObjects
	
	private GameObject[] microtiles = new GameObject[4]; // TR, BR, BL, TL
	private readonly int[] angles = new int[]{0,-90,-180,90}; // angle of rotation for each tile type
	private readonly int[][] sections = new int[][]{new int[]{0,1,2}, new int[]{2,3,4}, new int[]{4,5,6}, new int[]{6,7,0}};


	void Start()
	{
		InstantiateMicroTiles();
		updateAll();
	}

	private void InstantiateMicroTiles()
	{
		microtiles[0] = (GameObject)Instantiate(Resources.Load("Prefabs/Tiles/MicroTile", typeof(GameObject)), transform.position + new Vector3(0.75f,0.75f,0), transform.rotation);
		microtiles[0].GetComponent<TileDisplay>().rotation = 0;
		microtiles[0].transform.parent = transform;
		
		microtiles[1] = (GameObject)Instantiate(Resources.Load("Prefabs/Tiles/MicroTile", typeof(GameObject)), transform.position + new Vector3(0.75f,0.25f,0), transform.rotation);
		microtiles[1].GetComponent<TileDisplay>().rotation = 0;
		microtiles[1].transform.parent = transform;
		
		microtiles[2] = (GameObject)Instantiate(Resources.Load("Prefabs/Tiles/MicroTile", typeof(GameObject)), transform.position + new Vector3(0.25f,0.25f,0), transform.rotation);
		microtiles[2].GetComponent<TileDisplay>().rotation = 0;
		microtiles[2].transform.parent = transform;
		
		microtiles[3] = (GameObject)Instantiate(Resources.Load("Prefabs/Tiles/MicroTile", typeof(GameObject)), transform.position + new Vector3(0.25f,0.75f,0), transform.rotation);
		microtiles[3].GetComponent<TileDisplay>().rotation = 0;
		microtiles[3].transform.parent = transform;
	}

	public void SetTexture(Texture2D[] t)
	{
		texture = t;
	}

	public void SetNeighbors(bool[] n)
	{
		neighbors = n;
	}

	public void SetDisplaying(bool d)
	{
		displaying = d;
	}

	public bool IsVisible()
	{
		for (int i = 0; i < 4; i++)
		{
			if (microtiles[i].GetComponent<TileDisplay>().IsVisible())
			{
				return true;
			}
		}
		return false;
	}

	public void updateAll()
	{
		if (displaying)
		{
			for (int i = 0; i < 4; i++)
			{
				if (is_active)
				{
					if (!neighbors[sections[i][0]] && !neighbors[sections[i][2]]) // corner
					{
						microtiles[i].GetComponent<TileDisplay>().setImage(texture[1], angles[i]);
					}
					else if (neighbors[sections[i][0]] && neighbors[sections[i][1]] && neighbors[sections[i][2]]) // surround
					{
						microtiles[i].GetComponent<TileDisplay>().setImage(texture[4], angles[i]);
					}
					else if (neighbors[sections[i][0]] && neighbors[sections[i][2]] && !neighbors[sections[i][1]]) // concave
					{
						microtiles[i].GetComponent<TileDisplay>().setImage(texture[0], angles[i]);
					}
					else if (!neighbors[sections[i][0]]) // top side
					{
						microtiles[i].GetComponent<TileDisplay>().setImage(texture[3], angles[i]);
					}
					else if (!neighbors[sections[i][2]]) // right side
					{
						microtiles[i].GetComponent<TileDisplay>().setImage(texture[2], angles[i]);
					}
				}
				else
				{
					if (!neighbors[sections[i][0]] && !neighbors[sections[i][1]] && !neighbors[sections[i][2]]) // surround
					{
						microtiles[i].GetComponent<TileDisplay>().setImage(texture[9], angles[i] + 180);
					}
					else if (neighbors[sections[i][0]] && neighbors[sections[i][2]]) // concave
					{
						microtiles[i].GetComponent<TileDisplay>().setImage(texture[5], angles[i] + 180);
					}
					else if (neighbors[sections[i][1]] && !neighbors[sections[i][0]] && !neighbors[sections[i][2]]) // corner
					{
						microtiles[i].GetComponent<TileDisplay>().setImage(texture[6], angles[i] + 180);
					}
					else if (neighbors[sections[i][0]] && !neighbors[sections[i][2]]) // bottom
					{
						microtiles[i].GetComponent<TileDisplay>().setImage(texture[8], angles[i] + 180);
					}
					else if (neighbors[sections[i][2]] && !neighbors[sections[i][0]]) // left
					{
						microtiles[i].GetComponent<TileDisplay>().setImage(texture[7], angles[i] + 180);
					}
				}
			}
		}
		else
		{
			/*
			for (int i = 0; i < 4; i++)
			{
				microtiles[i].GetComponent<TileDisplay>().setImage(texture[9], angles[i] + 180);
			}
			*/
		}
	}
}

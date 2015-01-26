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

public sealed class TileContainer : MonoBehaviour
{

	private Sprite[] sprite = new Sprite[10];
	private bool[] neighbors = new bool[8]; // Data about its neighbors acquired through SetNeighbors(bool[] n)

	public bool is_active;
	public bool displaying = false; // Whether this tile is currently being displayed on the screen. If not, it's inside a pool of unused GameObjects
	
	private TileDisplay[] microtiles = new TileDisplay[4]; // TR, BR, BL, TL
	private readonly short[] angles = new short[]{0,-90,-180,90}; // angle of rotation for each tile type
	private readonly short[][] sections = new short[][]{new short[]{0,1,2}, new short[]{2,3,4}, new short[]{4,5,6}, new short[]{6,7,0}};

	private byte i;

	void Start()
	{
		InstantiateMicroTiles();
		updateAll();
	}

	private void InstantiateMicroTiles()
	{
		microtiles[0] = ((GameObject)Instantiate(Resources.Load("Prefabs/Tiles/MicroTile", typeof(GameObject)), transform.position + new Vector3(0.75f,0.75f,0), transform.rotation)).GetComponent<TileDisplay>();
		microtiles[0].rotation = 0;
		microtiles[0].transform.parent = transform;
		
		microtiles[1] = ((GameObject)Instantiate(Resources.Load("Prefabs/Tiles/MicroTile", typeof(GameObject)), transform.position + new Vector3(0.75f,0.25f,0), transform.rotation)).GetComponent<TileDisplay>();
		microtiles[1].rotation = 0;
		microtiles[1].transform.parent = transform;
		
		microtiles[2] = ((GameObject)Instantiate(Resources.Load("Prefabs/Tiles/MicroTile", typeof(GameObject)), transform.position + new Vector3(0.25f,0.25f,0), transform.rotation)).GetComponent<TileDisplay>();
		microtiles[2].rotation = 0;
		microtiles[2].transform.parent = transform;
		
		microtiles[3] = ((GameObject)Instantiate(Resources.Load("Prefabs/Tiles/MicroTile", typeof(GameObject)), transform.position + new Vector3(0.25f,0.75f,0), transform.rotation)).GetComponent<TileDisplay>();
		microtiles[3].rotation = 0;
		microtiles[3].transform.parent = transform;
	}

	public void SetSprite(Sprite[] t)
	{
		sprite = t;
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
		for (i = 0; i < 4; i++)
		{
			if (microtiles[i].IsVisible())
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
			for (i = 0; i < 4; i++)
			{
				if (is_active)
				{
					if (!neighbors[sections[i][0]] && !neighbors[sections[i][2]]) // corner
					{
						microtiles[i].setImage(sprite[1], angles[i]);
					}
					else if (neighbors[sections[i][0]] && neighbors[sections[i][1]] && neighbors[sections[i][2]]) // surround
					{
						microtiles[i].setImage(sprite[4], angles[i]);
					}
					else if (neighbors[sections[i][0]] && neighbors[sections[i][2]] && !neighbors[sections[i][1]]) // concave
					{
						microtiles[i].setImage(sprite[0], angles[i]);
					}
					else if (!neighbors[sections[i][0]]) // top side
					{
						microtiles[i].setImage(sprite[3], angles[i]);
					}
					else if (!neighbors[sections[i][2]]) // right side
					{
						microtiles[i].setImage(sprite[2], angles[i]);
					}
				}
				else
				{
					if (!neighbors[sections[i][0]] && !neighbors[sections[i][1]] && !neighbors[sections[i][2]]) // surround
					{
						microtiles[i].setImage(sprite[9], angles[i] + 180);
					}
					else if (neighbors[sections[i][0]] && neighbors[sections[i][2]]) // concave
					{
						microtiles[i].setImage(sprite[5], angles[i] + 180);
					}
					else if (neighbors[sections[i][1]] && !neighbors[sections[i][0]] && !neighbors[sections[i][2]]) // corner
					{
						microtiles[i].setImage(sprite[6], angles[i] + 180);
					}
					else if (neighbors[sections[i][0]] && !neighbors[sections[i][2]]) // bottom
					{
						microtiles[i].setImage(sprite[8], angles[i] + 180);
					}
					else if (neighbors[sections[i][2]] && !neighbors[sections[i][0]]) // left
					{
						microtiles[i].setImage(sprite[7], angles[i] + 180);
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

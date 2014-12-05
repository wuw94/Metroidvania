using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour
{
	public bool is_active;
	public GameObject[] neighbors = new GameObject[8];
	
	private bool[] neighbor_states = new bool[8]; // starting at 0:top, clockwise to 7:topleft
	private ArrayList neighbor_active = new ArrayList(); // contains ints of all the active neighbors
	private ArrayList neighbor_inactive = new ArrayList(); // contains ints of all the inactive neighbors

	private GameObject TR;
	private GameObject BR;
	private GameObject BL;
	private GameObject TL;

	void Start()
	{
		is_active = true;
		TR = (GameObject)Instantiate(Resources.Load("Prefabs/Tiles/MicroTile", typeof(GameObject)), transform.position + new Vector3(2,2,0), transform.rotation);
		TR.transform.parent = transform;
		BR = (GameObject)Instantiate(Resources.Load("Prefabs/Tiles/MicroTile", typeof(GameObject)), transform.position + new Vector3(2,0,0), transform.rotation);
		BR.transform.parent = transform;
		BL = (GameObject)Instantiate(Resources.Load("Prefabs/Tiles/MicroTile", typeof(GameObject)), transform.position + new Vector3(0,0,0), transform.rotation);
		BL.transform.parent = transform;
		TL = (GameObject)Instantiate(Resources.Load("Prefabs/Tiles/MicroTile", typeof(GameObject)), transform.position + new Vector3(0,2,0), transform.rotation);
		TL.transform.parent = transform;
	}

	void Update()
	{
		updateNeighborStates();
		updateTR();
		updateBR();
		updateBL();
		updateTL();
	}

	void updateNeighborStates()
	{
		neighbor_active.Clear();
		neighbor_inactive.Clear();
		for (int i = 0; i < 8; i++)
		{
			if (neighbors[i] != null)
			{
				neighbor_states[i] = neighbors[i].GetComponent<TileManager>().is_active;
				if (neighbor_states[i])
				{
					neighbor_active.Add(i);
				}
				else
				{
					neighbor_inactive.Add(i);
				}
			}
			else
			{
				neighbor_inactive.Add(i);
			}
		}
	}

	bool intersects(int[] small, ArrayList large)
	{
		for (int i = 0; i < small.Length; i++)
		{
			if (!large.Contains(small[i]))
			{
				return false;
			}
		}
		return true;
	}

	void updateTR()
	{

		if (is_active)
		{
			if (intersects(new int[]{0,2}, neighbor_inactive)) // corner
			{
				TR.GetComponent<TileDisplay>().frame = new Vector2(0,4);
			}
			else if (intersects(new int[]{0,1,2}, neighbor_active)) // inside
			{
				TR.GetComponent<TileDisplay>().frame = new Vector2(2,2);
			}
			else if (intersects(new int[]{1}, neighbor_inactive) && 
			         intersects(new int[]{0,2}, neighbor_active)) // concave
			{
				TR.GetComponent<TileDisplay>().frame = new Vector2(3,3);
			}
			else if (intersects(new int[]{0}, neighbor_inactive)) // top side
			{
				TR.GetComponent<TileDisplay>().frame = new Vector2(4,4);
			}
			else if (intersects(new int[]{2}, neighbor_inactive)) // right side
			{
				TR.GetComponent<TileDisplay>().frame = new Vector2(0,3);
			}

		}
		else
		{
			if (intersects(new int[]{0,1,2}, neighbor_inactive)) // empty
			{
				TR.GetComponent<TileDisplay>().frame = new Vector2(4,0);
			}
			else if (intersects(new int[]{0,2}, neighbor_active)) // concave
			{
				TR.GetComponent<TileDisplay>().frame = new Vector2(2,1);
			}
			else if (intersects(new int[]{0,2}, neighbor_inactive) &&
			    intersects(new int[]{1}, neighbor_active)) // corner
			{
				TR.GetComponent<TileDisplay>().frame = new Vector2(1,0);
			}
			else if (intersects(new int[]{2}, neighbor_inactive) &&
			         intersects(new int[]{0}, neighbor_active)) // bottom
			{
				TR.GetComponent<TileDisplay>().frame = new Vector2(3,2);
			}
			else if (intersects(new int[]{0}, neighbor_inactive) &&
			         intersects(new int[]{2}, neighbor_active)) // left
			{
				TR.GetComponent<TileDisplay>().frame = new Vector2(4,2);
			}
		}
	}

	void updateBR()
	{
		if (is_active)
		{
			if (intersects(new int[]{2,4}, neighbor_inactive)) // corner
			{
				BR.GetComponent<TileDisplay>().frame = new Vector2(1,4);
			}
			else if (intersects(new int[]{2,3,4}, neighbor_active)) // inside
			{
				BR.GetComponent<TileDisplay>().frame = new Vector2(2,2);
			}
			else if (intersects(new int[]{3}, neighbor_inactive) &&
			         intersects(new int[]{2,4}, neighbor_active)) // concave
			{
				BR.GetComponent<TileDisplay>().frame = new Vector2(4,3);
			}
			else if (intersects(new int[]{2}, neighbor_inactive)) // right side
			{
				BR.GetComponent<TileDisplay>().frame = new Vector2(0,3);
			}
			else if (intersects(new int[]{4}, neighbor_inactive)) // bottom side
			{
				BR.GetComponent<TileDisplay>().frame = new Vector2(1,3);
			}

		}
		else
		{
			if (intersects(new int[]{2,3,4}, neighbor_inactive)) // empty
			{
				BR.GetComponent<TileDisplay>().frame = new Vector2(4,0);
			}
			else if (intersects(new int[]{2,4}, neighbor_active)) // concave
			{
				BR.GetComponent<TileDisplay>().frame = new Vector2(3,1);
			}
			else if (intersects(new int[]{2,4}, neighbor_inactive) &&
			         intersects(new int[]{3}, neighbor_active)) // corner
			{
				BR.GetComponent<TileDisplay>().frame = new Vector2(2,0);
			}
			else if (intersects(new int[]{4}, neighbor_inactive) &&
			         intersects(new int[]{2}, neighbor_active)) // left
			{
				BR.GetComponent<TileDisplay>().frame = new Vector2(4,2);
			}
			else if (intersects(new int[]{2}, neighbor_inactive) &&
			         intersects(new int[]{4}, neighbor_active)) // bottom
			{
				BR.GetComponent<TileDisplay>().frame = new Vector2(0,1);
			}
		}
	}

	void updateBL()
	{
		if (is_active)
		{
			if (intersects(new int[]{4,6}, neighbor_inactive)) // corner
			{
				BL.GetComponent<TileDisplay>().frame = new Vector2(2,4);
			}
			else if (intersects(new int[]{4,5,6}, neighbor_active)) // inside
			{
				BL.GetComponent<TileDisplay>().frame = new Vector2(2,2);
			}
			else if (intersects(new int[]{5}, neighbor_inactive) &&
			         intersects(new int[]{4,6}, neighbor_active)) // concave
			{
				BL.GetComponent<TileDisplay>().frame = new Vector2(0,2);
			}
			else if (intersects(new int[]{4}, neighbor_inactive)) // bottom side
			{
				BL.GetComponent<TileDisplay>().frame = new Vector2(1,3);
			}
			else if (intersects(new int[]{6}, neighbor_inactive)) // left side
			{
				BL.GetComponent<TileDisplay>().frame = new Vector2(2,3);
			}

		}
		else
		{
			if (intersects(new int[]{4,5,6}, neighbor_inactive)) // empty
			{
				BL.GetComponent<TileDisplay>().frame = new Vector2(4,0);
			}
			else if (intersects(new int[]{4,6}, neighbor_active)) // concave
			{
				BL.GetComponent<TileDisplay>().frame = new Vector2(4,1);
			}
			else if (intersects(new int[]{4,6}, neighbor_inactive) &&
			         intersects(new int[]{5}, neighbor_active)) // corner
			{
				BL.GetComponent<TileDisplay>().frame = new Vector2(3,0);
			}
			else if (intersects(new int[]{6}, neighbor_inactive) &&
			         intersects(new int[]{4}, neighbor_active)) // top
			{
				BL.GetComponent<TileDisplay>().frame = new Vector2(0,1);
			}
			else if (intersects(new int[]{4}, neighbor_inactive) &&
			         intersects(new int[]{6}, neighbor_active)) // right
			{
				BL.GetComponent<TileDisplay>().frame = new Vector2(1,1);
			}
		}
	}

	void updateTL()
	{
		if (is_active)
		{
			if (intersects(new int[]{6,0}, neighbor_inactive)) // corner
			{
				TL.GetComponent<TileDisplay>().frame = new Vector2(3,4);
			}
			else if (intersects(new int[]{6,7,0}, neighbor_active)) // inside
			{
				TL.GetComponent<TileDisplay>().frame = new Vector2(2,2);
			}
			else if (intersects(new int[]{7}, neighbor_inactive) &&
			         intersects(new int[]{6,0}, neighbor_active)) // concave
			{
				TL.GetComponent<TileDisplay>().frame = new Vector2(1,2);
			}
			else if (intersects(new int[]{6}, neighbor_inactive)) // left side
			{
				TL.GetComponent<TileDisplay>().frame = new Vector2(2,3);
			}
			else if (intersects(new int[]{0}, neighbor_inactive)) // top side
			{
				TL.GetComponent<TileDisplay>().frame = new Vector2(4,4);
			}

		}
		else
		{
			if (intersects(new int[]{6,7,0}, neighbor_inactive)) // empty
			{
				TL.GetComponent<TileDisplay>().frame = new Vector2(4,0);
			}
			else if (intersects(new int[]{6,0}, neighbor_active)) // concave
			{
				TL.GetComponent<TileDisplay>().frame = new Vector2(0,0);
			}
			else if (intersects(new int[]{6,0}, neighbor_inactive) &&
			         intersects(new int[]{7}, neighbor_active)) // corner
			{
				TL.GetComponent<TileDisplay>().frame = new Vector2(4,0);
			}
			else if (intersects(new int[]{0}, neighbor_inactive) &&
			         intersects(new int[]{6}, neighbor_active)) // right
			{
				TL.GetComponent<TileDisplay>().frame = new Vector2(1,1);
			}
			else if (intersects(new int[]{6}, neighbor_inactive) &&
			         intersects(new int[]{0}, neighbor_active)) // bottom
			{
				TL.GetComponent<TileDisplay>().frame = new Vector2(0,1);
			}
		}
	}
}

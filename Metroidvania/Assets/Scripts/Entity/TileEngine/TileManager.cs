using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour
{

	public string type = "GrassTile";
	public bool is_active;
	private bool is_Editor_Scene = false;
	public GameObject[] neighbors = new GameObject[8];
	
	private bool[] neighbor_states = new bool[8]; // starting at 0:top, clockwise to 7:topleft
	private ArrayList neighbor_active = new ArrayList(); // contains ints of all the active neighbors
	private ArrayList neighbor_inactive = new ArrayList(); // contains ints of all the inactive neighbors

	private GameObject TR;
	private GameObject BR;
	private GameObject BL;
	private GameObject TL;

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


	void Start()
	{
		if (Application.loadedLevelName == "TileEditor")
		{
			is_Editor_Scene = true;
		}
		TR = (GameObject)Instantiate(Resources.Load("Prefabs/Tiles/MicroTile", typeof(GameObject)), transform.position + new Vector3(1,1,0), transform.rotation);
		TR.GetComponent<TileDisplay>().rotation = 0;
		TR.transform.parent = transform;

		BR = (GameObject)Instantiate(Resources.Load("Prefabs/Tiles/MicroTile", typeof(GameObject)), transform.position + new Vector3(1,0,0), transform.rotation);
		BR.GetComponent<TileDisplay>().rotation = 0;
		BR.transform.parent = transform;

		BL = (GameObject)Instantiate(Resources.Load("Prefabs/Tiles/MicroTile", typeof(GameObject)), transform.position + new Vector3(0,0,0), transform.rotation);
		BL.GetComponent<TileDisplay>().rotation = 0;
		BL.transform.parent = transform;

		TL = (GameObject)Instantiate(Resources.Load("Prefabs/Tiles/MicroTile", typeof(GameObject)), transform.position + new Vector3(0,1,0), transform.rotation);
		TL.GetComponent<TileDisplay>().rotation = 0;
		TL.transform.parent = transform;

		i_concave = (Texture2D)Resources.Load("Tiles/"+type+"/Inner_Concave");
		i_corner = (Texture2D)Resources.Load("Tiles/"+type+"/Inner_Corner");
		i_side_right = (Texture2D)Resources.Load("Tiles/"+type+"/Inner_Side_Right");
		i_side_top = (Texture2D)Resources.Load("Tiles/"+type+"/Inner_Side_Top");
		i_surround = (Texture2D)Resources.Load("Tiles/"+type+"/Inner_Surround");
		o_concave = (Texture2D)Resources.Load("Tiles/"+type+"/Outer_Concave");
		o_corner = (Texture2D)Resources.Load("Tiles/"+type+"/Outer_Corner");
		o_side_right = (Texture2D)Resources.Load("Tiles/"+type+"/Outer_Side_Right");
		o_side_top = (Texture2D)Resources.Load("Tiles/"+type+"/Outer_Side_Top");
		if (is_Editor_Scene)
		{
			o_surround = (Texture2D)Resources.Load("Tiles/"+type+"/Editor_Mode");
		}
		else
		{
			o_surround = (Texture2D)Resources.Load("Tiles/"+type+"/Outer_Surround");
		}

		updateAll();
	}
	
	void OnMouseOver()
	{
		if (Input.GetKey(KeyCode.Q))
		{
			type = Camera.main.GetComponent<TileEditor>().tile_type;
			i_concave = (Texture2D)Resources.Load("Tiles/"+type+"/Inner_Concave");
			i_corner = (Texture2D)Resources.Load("Tiles/"+type+"/Inner_Corner");
			i_side_right = (Texture2D)Resources.Load("Tiles/"+type+"/Inner_Side_Right");
			i_side_top = (Texture2D)Resources.Load("Tiles/"+type+"/Inner_Side_Top");
			i_surround = (Texture2D)Resources.Load("Tiles/"+type+"/Inner_Surround");
			o_concave = (Texture2D)Resources.Load("Tiles/"+type+"/Outer_Concave");
			o_corner = (Texture2D)Resources.Load("Tiles/"+type+"/Outer_Corner");
			o_side_right = (Texture2D)Resources.Load("Tiles/"+type+"/Outer_Side_Right");
			o_side_top = (Texture2D)Resources.Load("Tiles/"+type+"/Outer_Side_Top");
			updateAll();
		}
		if (Input.GetMouseButton(0))
		{
			is_active = false;
			for (int i = 0; i < 8; i++)
			{
				neighbors[i].GetComponent<TileManager>().updateAll();
			}
		}
		if (Input.GetMouseButton(1))
		{
			is_active = true;
			for (int i = 0; i < 8; i++)
			{
				neighbors[i].GetComponent<TileManager>().updateAll();
			}
		}
		updateAll();
	}

	private void updateAll()
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

	private bool intersects(int[] small, ArrayList large)
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

	private void updateTR()
	{

		if (is_active)
		{
			if (intersects(new int[]{0,2}, neighbor_inactive)) // corner
			{
				TR.GetComponent<TileDisplay>().setImage(i_corner, 0);
			}
			else if (intersects(new int[]{0,1,2}, neighbor_active)) // surround
			{
				TR.GetComponent<TileDisplay>().setImage(i_surround, 0);
			}
			else if (intersects(new int[]{1}, neighbor_inactive) && 
			         intersects(new int[]{0,2}, neighbor_active)) // concave
			{
				TR.GetComponent<TileDisplay>().setImage(i_concave, 0);
			}
			else if (intersects(new int[]{0}, neighbor_inactive)) // top side
			{
				TR.GetComponent<TileDisplay>().setImage(i_side_top, 0);
			}
			else if (intersects(new int[]{2}, neighbor_inactive)) // right side
			{
				TR.GetComponent<TileDisplay>().setImage(i_side_right, 0);
			}

		}
		else
		{
			if (intersects(new int[]{0,1,2}, neighbor_inactive)) // surround
			{
				TR.GetComponent<TileDisplay>().setImage(o_surround, 180);
			}
			else if (intersects(new int[]{0,2}, neighbor_active)) // concave
			{
				TR.GetComponent<TileDisplay>().setImage(o_concave, 180);
			}
			else if (intersects(new int[]{0,2}, neighbor_inactive) &&
			    intersects(new int[]{1}, neighbor_active)) // corner
			{
				TR.GetComponent<TileDisplay>().setImage(o_corner, 180);
			}
			else if (intersects(new int[]{2}, neighbor_inactive) &&
			         intersects(new int[]{0}, neighbor_active)) // bottom
			{
				TR.GetComponent<TileDisplay>().setImage(o_side_top, 180);
			}
			else if (intersects(new int[]{0}, neighbor_inactive) &&
			         intersects(new int[]{2}, neighbor_active)) // left
			{
				TR.GetComponent<TileDisplay>().setImage(o_side_right, 180);
			}
		}
	}

	private void updateBR()
	{
		if (is_active)
		{
			if (intersects(new int[]{2,4}, neighbor_inactive)) // corner
			{
				BR.GetComponent<TileDisplay>().setImage(i_corner, -90);
			}
			else if (intersects(new int[]{2,3,4}, neighbor_active)) // surround
			{
				BR.GetComponent<TileDisplay>().setImage(i_surround, -90);
			}
			else if (intersects(new int[]{3}, neighbor_inactive) &&
			         intersects(new int[]{2,4}, neighbor_active)) // concave
			{
				BR.GetComponent<TileDisplay>().setImage(i_concave, -90);
			}
			else if (intersects(new int[]{2}, neighbor_inactive)) // right side
			{
				BR.GetComponent<TileDisplay>().setImage(i_side_top, -90);
			}
			else if (intersects(new int[]{4}, neighbor_inactive)) // bottom side
			{
				BR.GetComponent<TileDisplay>().setImage(i_side_right, -90);
			}

		}
		else
		{
			if (intersects(new int[]{2,3,4}, neighbor_inactive)) // surround
			{
				BR.GetComponent<TileDisplay>().setImage(o_surround, 90);
			}
			else if (intersects(new int[]{2,4}, neighbor_active)) // concave
			{
				BR.GetComponent<TileDisplay>().setImage(o_concave, 90);
			}
			else if (intersects(new int[]{2,4}, neighbor_inactive) &&
			         intersects(new int[]{3}, neighbor_active)) // corner
			{
				BR.GetComponent<TileDisplay>().setImage(o_corner, 90);
			}
			else if (intersects(new int[]{4}, neighbor_inactive) &&
			         intersects(new int[]{2}, neighbor_active)) // left
			{
				BR.GetComponent<TileDisplay>().setImage(o_side_top, 90);
			}
			else if (intersects(new int[]{2}, neighbor_inactive) &&
			         intersects(new int[]{4}, neighbor_active)) // bottom
			{
				BR.GetComponent<TileDisplay>().setImage(o_side_right, 90);
			}
		}
	}

	private void updateBL()
	{
		if (is_active)
		{
			if (intersects(new int[]{4,6}, neighbor_inactive)) // corner
			{
				BL.GetComponent<TileDisplay>().setImage(i_corner, 180);
			}
			else if (intersects(new int[]{4,5,6}, neighbor_active)) // surround
			{
				BL.GetComponent<TileDisplay>().setImage(i_surround, 180);
			}
			else if (intersects(new int[]{5}, neighbor_inactive) &&
			         intersects(new int[]{4,6}, neighbor_active)) // concave
			{
				BL.GetComponent<TileDisplay>().setImage(i_concave, 180);
			}
			else if (intersects(new int[]{4}, neighbor_inactive)) // bottom side
			{
				BL.GetComponent<TileDisplay>().setImage(i_side_top, 180);
			}
			else if (intersects(new int[]{6}, neighbor_inactive)) // left side
			{
				BL.GetComponent<TileDisplay>().setImage(i_side_right, 180);
			}

		}
		else
		{
			if (intersects(new int[]{4,5,6}, neighbor_inactive)) // surround
			{
				BL.GetComponent<TileDisplay>().setImage(o_surround, 0);
			}
			else if (intersects(new int[]{4,6}, neighbor_active)) // concave
			{
				BL.GetComponent<TileDisplay>().setImage(o_concave, 0);
			}
			else if (intersects(new int[]{4,6}, neighbor_inactive) &&
			         intersects(new int[]{5}, neighbor_active)) // corner
			{
				BL.GetComponent<TileDisplay>().setImage(o_corner, 0);
			}
			else if (intersects(new int[]{6}, neighbor_inactive) &&
			         intersects(new int[]{4}, neighbor_active)) // top
			{
				BL.GetComponent<TileDisplay>().setImage(o_side_top, 0);
			}
			else if (intersects(new int[]{4}, neighbor_inactive) &&
			         intersects(new int[]{6}, neighbor_active)) // right
			{
				BL.GetComponent<TileDisplay>().setImage(o_side_right, 0);
			}
		}
	}

	private void updateTL()
	{
		if (is_active)
		{
			if (intersects(new int[]{6,0}, neighbor_inactive)) // corner
			{
				TL.GetComponent<TileDisplay>().setImage(i_corner, 90);
			}
			else if (intersects(new int[]{6,7,0}, neighbor_active)) // surround
			{
				TL.GetComponent<TileDisplay>().setImage(i_surround, 90);
			}
			else if (intersects(new int[]{7}, neighbor_inactive) &&
			         intersects(new int[]{6,0}, neighbor_active)) // concave
			{
				TL.GetComponent<TileDisplay>().setImage(i_concave, 90);
			}
			else if (intersects(new int[]{6}, neighbor_inactive)) // left side
			{
				TL.GetComponent<TileDisplay>().setImage(i_side_top, 90);
			}
			else if (intersects(new int[]{0}, neighbor_inactive)) // top side
			{
				TL.GetComponent<TileDisplay>().setImage(i_side_right, 90);
			}

		}
		else
		{
			if (intersects(new int[]{6,7,0}, neighbor_inactive)) // surround
			{
				TL.GetComponent<TileDisplay>().setImage(o_surround, -90);
			}
			else if (intersects(new int[]{6,0}, neighbor_active)) // concave
			{
				TL.GetComponent<TileDisplay>().setImage(o_concave, -90);
			}
			else if (intersects(new int[]{6,0}, neighbor_inactive) &&
			         intersects(new int[]{7}, neighbor_active)) // corner
			{
				TL.GetComponent<TileDisplay>().setImage(o_corner, -90);
			}
			else if (intersects(new int[]{0}, neighbor_inactive) &&
			         intersects(new int[]{6}, neighbor_active)) // right
			{
				TL.GetComponent<TileDisplay>().setImage(o_side_top, -90);
			}
			else if (intersects(new int[]{6}, neighbor_inactive) &&
			         intersects(new int[]{0}, neighbor_active)) // bottom
			{
				TL.GetComponent<TileDisplay>().setImage(o_side_right, -90);
			}
		}
	}
}

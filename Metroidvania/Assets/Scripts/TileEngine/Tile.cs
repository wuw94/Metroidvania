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

public sealed class Tile : MonoBehaviour
{
	private bool[] neighbors = new bool[8]; // Data about its neighbors acquired through SetNeighbors(bool[] n)

	public bool is_active;
	public bool displaying = false; // Whether this tile is currently being displayed on the screen. If not, it's inside a pool of unused GameObjects

	private byte i;

	void Start()
	{
		updateAll();
		transform.localScale = new Vector3(0.25f,0.25f,1);
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
		return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), GetComponent<Renderer>().bounds);
	}

	public void SetIndoor()
	{
		gameObject.layer = LayerMask.NameToLayer("Foreground (Indoor)");
	}

	public void SetOutdoor()
	{
		gameObject.layer = LayerMask.NameToLayer("Foreground (Outdoor)");
	}

	public void updateAll()
	{
		if (is_active)
		{
			byte t = GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].tiles[(int)transform.position.x][(int)transform.position.y].type;
			gameObject.layer = (GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].tiles[(int)transform.position.x][(int)transform.position.y].outdoor)
				? LayerMask.NameToLayer("Foreground (Outdoor)") : LayerMask.NameToLayer("Foreground (Indoor)");

			string c = ((neighbors[0]) ? "C" : "O") + ((neighbors[6]) ? "C" : "O") + ((neighbors[2]) ? "C" : "O") + ((neighbors[4]) ? "C" : "O");
			GetComponent<SpriteRenderer>().sprite = TileManager.tile_sprite[TileManager.tile_types[t]][(TileCombination)System.Enum.Parse(typeof(TileCombination), c)];
		}
		else
		{
			GetComponent<SpriteRenderer>().sprite = TileManager.empty_sprite;
		}
	}
}

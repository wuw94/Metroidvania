using UnityEngine;
using System.Collections;

public class TileGenerator : MonoBehaviour
{
	int rows = 30;
	int columns = 100;
	GameObject[][] tiles;

	void Start()
	{
		GameObject[][] tiles = new GameObject[rows][];
		transform.position = new Vector3(0, 0, transform.position.z);
		for (int i = 0; i < rows; i++)
		{
			tiles[i] = new GameObject[columns];
			for (int j = 0; j < columns; j++)
			{
				tiles[i][j] = (GameObject)Instantiate(Resources.Load("Prefabs/Tiles/MacroTile", typeof(GameObject)), transform.position + new Vector3(2*j,2*i,0), transform.rotation);
			}
		}
		for (int i = 1; i < rows-1; i++)
		{
			for (int j = 1; j < columns-1; j++)
			{
				tiles[i][j].GetComponent<TileManager>().neighbors[0] = tiles[i+1][j];
				tiles[i][j].GetComponent<TileManager>().neighbors[1] = tiles[i+1][j+1];
				tiles[i][j].GetComponent<TileManager>().neighbors[2] = tiles[i][j+1];
				tiles[i][j].GetComponent<TileManager>().neighbors[3] = tiles[i-1][j+1];
				tiles[i][j].GetComponent<TileManager>().neighbors[4] = tiles[i-1][j];
				tiles[i][j].GetComponent<TileManager>().neighbors[5] = tiles[i-1][j-1];
				tiles[i][j].GetComponent<TileManager>().neighbors[6] = tiles[i][j-1];
				tiles[i][j].GetComponent<TileManager>().neighbors[7] = tiles[i+1][j-1];
				tiles[i][j].GetComponent<TileManager>().is_active = true;
			}
		}
	}
	
	void Update()
	{
	}
}

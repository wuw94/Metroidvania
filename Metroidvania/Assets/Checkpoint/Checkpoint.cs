using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
	// Player Lives
	public int playerLives = 4;

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Player")
		{
			// Assigns new checkpoint's x and y coordinates for player to spawn
			RespawnSystem.currentCheckpoint = gameObject;
			Debug.Log ("Reached Checkpoint: (" + transform.position.x + ", " + transform.position.y + ")");
		}
	}

}

using UnityEngine;
using System.Collections;

public class RespawnSystem : MonoBehaviour {

	public static GameObject currentCheckpoint;

	public static void Respawn(GameObject player)
	{
		Checkpoint checkpoint = currentCheckpoint.GetComponent<Checkpoint>();
		checkpoint.playerLives--;
		if (checkpoint.playerLives > 0)
		{
			Debug.Log("Lives: " + checkpoint.playerLives);
			player.transform.position = checkpoint.transform.position;
		}
		else
		{
			Debug.Log("You have died");
			// Death script here
		}
	}

}

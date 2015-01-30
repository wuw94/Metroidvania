using UnityEngine;
using System.Collections;

/*
 * This script is for enemy walls and platforms.
 * 
 * It toggles their active and inactiveness depending on
 * 		- Their color (blue or green)
 * 		- The recording state (recording or normal)
 * 
 * Green 
 * - active in normal mode
 * - inactive in recording mode
 * 
 * Blue 
 * - active in recording mode
 * - inactive in normal mode
 * 
 * Attach this script to a separate game object
 * that has an array of enemy platforms and walls and
 * toggle their activeness within a loop.
 * 
 */

public class EnemyWallAndPlatform : Recordable 
{
	public GameObject[] blueObjects;
	public GameObject[] greenObjects;

	public override void NormalUpdate()
	{
		// Changes anything of blue objects within this loop during normal state
		for (int i = 0; i < blueObjects.Length; i++) 
		{
			blueObjects [i].transform.localScale = new Vector3 (1, 1, 1);
		}

		// Changes anything of green objects within this loop during normal state
		for (int i = 0; i < greenObjects.Length; i++) 
		{
			greenObjects [i].transform.localScale = new Vector3 (.5f, 3, 1);
		}
	}

	public override void Record()
	{
		// Changes anything of blue objects within this loop during recording state
		for (int i = 0; i < blueObjects.Length; i++) 
		{
			blueObjects [i].transform.localScale = new Vector3 (.5f, 3, 1);
		}

		// Changes anything of green objects within this loop during recording state
		for (int i = 0; i < greenObjects.Length; i++) 
		{
			greenObjects [i].transform.localScale = new Vector3 (1, 1, 1);
		}
	}
}

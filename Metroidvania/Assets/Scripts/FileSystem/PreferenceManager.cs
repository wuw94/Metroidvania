using UnityEngine;
using System.Collections;

/* Preferences Manager.
 * Stores information about the game preferences:
 * 1. Key bindings
 * 
 * auth Wesley Wu
 */

[System.Serializable]
public sealed class PreferenceManager
{
	public KeyCode IN_LEFT = KeyCode.LeftArrow;
	public KeyCode IN_RIGHT = KeyCode.RightArrow;
	public KeyCode IN_UP = KeyCode.UpArrow;
	public KeyCode IN_DOWN = KeyCode.DownArrow;
	public KeyCode IN_JUMP = KeyCode.Space;

	public PreferenceManager()
	{
	}
}

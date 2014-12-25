using UnityEngine;
using System.Collections;

/* Preferences Manager.
 * Stores information about the game preferences:
 * 1. Key bindings
 * 
 * Script Functionalities:
 * IN_LEFT - left hotkey
 * IN_RIGHT - right hotkey
 * IN_UP - up hotkey
 * IN_DOWN - down hotkey
 * IN_JUMP - space hotkey
 * 
 * auth Wesley Wu
 */

/// <summary>
/// Stores information about the game preferences.
/// </summary>
[System.Serializable]
public sealed class PreferenceManager
{
	/// <summary>
	/// Move left KeyCode.
	/// </summary>
	public KeyCode IN_LEFT = KeyCode.LeftArrow;

	/// <summary>
	/// Move right KeyCode.
	/// </summary>
	public KeyCode IN_RIGHT = KeyCode.RightArrow;

	/// <summary>
	/// Move up KeyCode.
	/// </summary>
	public KeyCode IN_UP = KeyCode.UpArrow;

	/// <summary>
	/// Move down KeyCode.
	/// </summary>
	public KeyCode IN_DOWN = KeyCode.DownArrow;

	/// <summary>
	/// Jump KeyCode.
	/// </summary>
	public KeyCode IN_JUMP = KeyCode.Space;

	/// <summary>
	/// Perform action KeyCode.
	/// </summary>
	public KeyCode IN_ACTION = KeyCode.Q;

	/// <summary>
	/// Activate time shift KeyCode.
	/// </summary>
	public KeyCode IN_TIME_SHIFT = KeyCode.W;

	/// <summary>
	/// Attack KeyCode
	/// </summary>
	public KeyCode IN_ATTACK = KeyCode.E;

	/// <summary>
	/// Undo KeyCode
	/// </summary>
	public KeyCode IN_UNDO = KeyCode.R;

	/// <summary>
	/// Constructor.
	/// </summary>
	public PreferenceManager()
	{
	}
}

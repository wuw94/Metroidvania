using UnityEngine;
using System.Collections;

/* Character Manager.
 * Stores information about the player's character
 * 
 * auth Wesley Wu
 */

[System.Serializable]
public sealed class CharacterManager
{
	public float move_speed_max;
	public float move_speed_accel_ground;
	public float move_speed_accel_air;
	public float jump_speed;
	
	public CharacterManager()
	{
	}
}

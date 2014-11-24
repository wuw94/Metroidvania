using UnityEngine;
using System.Collections;

/* Character Manager.
 * Stores information about the player's character
 * 
 * auth Wesley Wu
 * 
 * Script Functionalities
 * 
 * Global Variables:
 * move_speed_max - movement speed of max
 * move_speed_accel_ground - movement speed on ground
 * move_speed_accel_air - movement speed in air
 * jump_speed - height level when jumping
 * 
 */

[System.Serializable]
public sealed class CharacterManager
{
	public float move_speed_max;
	public float move_speed_accel_ground;
	public float move_speed_accel_air;
	public float jump_speed;
	public float health;
	
	public CharacterManager()
	{
	}
}

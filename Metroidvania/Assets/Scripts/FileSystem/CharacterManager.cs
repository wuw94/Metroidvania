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

/// <summary>
/// Stores information about the character.
/// </summary>
[System.Serializable]
public sealed class CharacterManager
{
	public float move_speed_max;
	public float move_speed_accel_ground;
	public float move_speed_accel_air;
	public float jump_speed;
	public float health;
	public float health_max = 100;

	/// <summary>
	/// Constructor.
	/// </summary>
	public CharacterManager()
	{
	}

	/// <summary>
	/// Changes health of player. Give it a negative amount if you want to damage the character.
	/// </summary>
	public void changeHealth(float amount)
	{
		health += amount;
		if (amount < 0 && health < 0)
		{
				manageDeath();
		}
		else if (amount > health_max && health > health_max)
		{
			health = health_max;
		}
	}

	/// <summary>
	/// Cleans up data when player dies.
	/// </summary>
	private void manageDeath()
	{
		// reset health and stuff to necessary values
		// restart the stage, and it will deal with reading values
		Recordable.record_index = 0;
		Application.LoadLevel(Application.loadedLevelName);
	}
}

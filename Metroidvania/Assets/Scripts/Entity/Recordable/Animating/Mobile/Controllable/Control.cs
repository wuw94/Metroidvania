using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Controllable.
 * Objects that inherit from the Controllable class are able to:
 * 1. Move based on certain keystrokes
 * 2. Check how much they moved in the last frame with previousLocation and transform.position
 * 
 * Script Functionalities
 * 
 * Functions:
 * checkMovementInputs() - movement input speed determined by CharacterManager from ProgressionManager from GameManager
 * Input.GetKey() - call function when certain key is pressed
 * 
 * In Parameter Arguments:
 * c - variable name for CharacterManager script
 * 	 - allows characterManager to be called through ProgressionManager through GameManager
 * GameManager.current_game.preferences.IN_LEFT 
 * 		- calls function in PreferenceManager when player hits left key
 * GameManager.current_game.preferences.IN_RIGHT 
 * 		- calls function in PreferenceManager when player hits right key
 * GameManager.current_game.preferences.IN_JUMP 
 * 		- calls function in PreferenceManager when player hits jump key
 * c.move_speed_max = current_game.progression.character.move_speed_max = msm
 * 		- character max movement speed depending on player progression
 * c.move_speed_accel_ground = current_game.progression.character.move_speed_accel_ground = msag
 * 		- character movement speed on ground depending on player progression
 * c.move_speed_accel_air = current_game.progression.character.move_speed_accel_air = msaa
 * 		- character movement speed in air depending on player progression
 * c.jump_speed = current_game.progression.character.jump_speed = js
 * 		- character jump speed depending on player progression
 */

public class Control : Animating
{
	public bool IN_ACTION = false;
	public bool can_check_action = true;
	public bool IN_TIME_SHIFT = false;
	public bool can_check_time_shift = true;

	public bool IN_LEFT = false;
	public bool IN_RIGHT = false;
	public bool IN_UP = false;
	public bool IN_DOWN = false;

	public bool IN_JUMP = false;
	public bool can_check_jump = true;
	public bool IN_ATTACK = false;

	public override void NormalUpdate()
	{
		base.NormalUpdate();
		if (IN_RIGHT)
		{
			this_info.facingRight = true;
		}
		else if (IN_LEFT)
		{
			this_info.facingRight = false;
		}
	}
}

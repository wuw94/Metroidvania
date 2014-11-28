using UnityEngine;
using System.Collections;

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

public class Controllable : Mobile
{
	void OnTriggerStay2D(Collider2D col)
	{
		if (Input.GetKeyDown(GameManager.current_game.preferences.IN_ACTION))
		{
			col.gameObject.GetComponent<Recordable>().Action();
		}
	}

	public void checkTimeShift()
	{
		if (Recordable.operation_mode == 0)
		{
			if (Input.GetKey(GameManager.current_game.preferences.IN_TIME_SHIFT))
			{
				if (Time.timeScale > 0.3f)
				{
					Time.timeScale-= 0.02f;
				}
				else
				{
					Time.timeScale = 1;
					ChangeOperationMode();
				}
			}
			else if (Time.timeScale < 1)
			{
				Time.timeScale += 0.1f;
			}
			else if (Time.timeScale > 1)
			{
				Time.timeScale = 1;
			}
		}
		else
		{
			if (Input.GetKeyDown(GameManager.current_game.preferences.IN_TIME_SHIFT))
			{
				if (Recordable.change_mode_cd == 0)
				{
					ChangeOperationMode();
				}
			}
		}
	}

	public void checkMovementInputs(CharacterManager c)
	{
		if (Recordable.record_index < Recordable.recorded_states_max - 1)
		{
			if (Input.GetKey(GameManager.current_game.preferences.IN_LEFT))
			{
				moveLeft(c.move_speed_max, c.move_speed_accel_ground, c.move_speed_accel_air);
			}
			if (Input.GetKey(GameManager.current_game.preferences.IN_RIGHT))
			{
				moveRight(c.move_speed_max, c.move_speed_accel_ground, c.move_speed_accel_air);
			}
			if (Input.GetKey(GameManager.current_game.preferences.IN_JUMP))
			{
				jump(c.jump_speed);
			}
		}
	}



	public override void NormalUpdate()
	{
		base.NormalUpdate();
	}

	public override void Record()
	{
		base.Record();
	}

	public override void Rewind()
	{
		base.Rewind();
	}

	public override void Playback()
	{
		base.Playback();
	}

}

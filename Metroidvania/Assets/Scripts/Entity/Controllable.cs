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
	private bool IN_ACTION = false;
	private bool can_check_action = true;
	private bool IN_TIME_SHIFT = false;
	private bool can_check_time_shift = true;
	private bool IN_LEFT = false;
	private bool IN_RIGHT = false;
	private bool IN_JUMP = false;
	private bool IN_ATTACK = false;

	private ArrayList current_collisions = new ArrayList();

	void OnTriggerEnter2D(Collider2D col)
	{
		if (!current_collisions.Contains(col))
		{
			current_collisions.Add(col);
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (current_collisions.Contains(col))
		{
			current_collisions.Remove(col);
		}
	}


	void OnGUI()
	{
		if (Event.current.type == EventType.KeyDown)
		{
			if (Event.current.keyCode == GameManager.current_game.preferences.IN_ACTION && can_check_action)
			{
				IN_ACTION = true;
				can_check_action = false;
			}
			else if (Event.current.keyCode == GameManager.current_game.preferences.IN_TIME_SHIFT && can_check_time_shift)
			{
				IN_TIME_SHIFT = true;
				can_check_time_shift = false;
			}
			else if (Event.current.keyCode == GameManager.current_game.preferences.IN_LEFT)
			{
				IN_LEFT = true;
			}
			else if (Event.current.keyCode == GameManager.current_game.preferences.IN_RIGHT)
			{
				IN_RIGHT = true;
			}
			else if (Event.current.keyCode == GameManager.current_game.preferences.IN_JUMP)
			{
				IN_JUMP = true;
			}
			else if (Event.current.keyCode == GameManager.current_game.preferences.IN_ATTACK)
			{
				IN_ATTACK = true;
			}
		}
		else if (Event.current.type == EventType.KeyUp)
		{
			if (Event.current.keyCode == GameManager.current_game.preferences.IN_ACTION)
			{
				IN_ACTION = false;
				can_check_action = true;
			}
			else if (Event.current.keyCode == GameManager.current_game.preferences.IN_TIME_SHIFT)
			{
				IN_TIME_SHIFT = false;
				can_check_time_shift = true;
			}
			else if (Event.current.keyCode == GameManager.current_game.preferences.IN_LEFT)
			{
				IN_LEFT = false;
			}
			else if (Event.current.keyCode == GameManager.current_game.preferences.IN_RIGHT)
			{
				IN_RIGHT = false;
			}
			else if (Event.current.keyCode == GameManager.current_game.preferences.IN_JUMP)
			{
				IN_JUMP = false;
			}
			else if (Event.current.keyCode == GameManager.current_game.preferences.IN_ATTACK)
			{
				IN_ATTACK = false;
			}
		}
	}

	public void checkAction()
	{
		if (IN_ACTION)
		{
			IN_ACTION = false;
			for (int i = 0; i < current_collisions.Count; i++)
			{
				if (((Collider2D)current_collisions[i]).gameObject.GetComponent<Recordable>() != null)
				{
					((Collider2D)current_collisions[i]).gameObject.GetComponent<Recordable>().Action();
				}
			}
		}
	}

	public void checkTimeShift()
	{
		if (Recordable.operation_mode == 0)
		{
			if (IN_TIME_SHIFT)
			{
				if (Time.timeScale > 0.3f)
				{
					Time.timeScale-= 0.02f;
				}
				else
				{
					Time.timeScale = 1;
					IN_TIME_SHIFT = false;
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
			if (IN_TIME_SHIFT)
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
			if (IN_JUMP && grounded)
			{
				jump(c.jump_speed);
			}
			else if (IN_LEFT)
			{
				moveLeft(c.move_speed_max, c.move_speed_accel_ground, c.move_speed_accel_air);
			}
			else if (IN_RIGHT)
			{
				moveRight(c.move_speed_max, c.move_speed_accel_ground, c.move_speed_accel_air);
			}
			else if (IN_ATTACK)
			{
				Attack(c.move_speed_max, c.move_speed_accel_ground, c.move_speed_accel_air);
			}
			else
			{
				noInput();
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

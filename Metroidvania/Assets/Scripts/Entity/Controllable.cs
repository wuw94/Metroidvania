using UnityEngine;
using System.Collections;

/* Controllable.
 * Objects that inherit from the Controllable class are able to:
 * 1. Move based on certain keystrokes
 * 2. Check how much they moved in the last frame with previousLocation and transform.position
 */

public class Controllable : Mobile
{

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
		else
		{
			rigidbody2D.isKinematic = true;
		}
	}

	public void checkTimeShift()
	{
		if (Input.GetKeyDown(GameManager.current_game.preferences.IN_TIME_SHIFT))
		{
			if (Recordable.change_mode_cd == 0)
			{
				ChangeOperationMode();
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

	public override void RecordAct()
	{
		base.RecordAct();
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

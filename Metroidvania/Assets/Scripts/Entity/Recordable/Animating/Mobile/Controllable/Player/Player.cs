using UnityEngine;
using System.Collections;

/* Player.
 * Always put this on the player prefab, but make sure there is only one on the scene at any given time.
 * Not to be put on the clone of player. There's another script for that
 * 
 * Script Functionalities
 * 
 * Functions:
 * Start() - calls functions when game is started
 * GameManager.SetGameAll() - set player stats when game is started up
 * 
 */

public class Player : Mobile
{
	// Animating
	public Sprite[] still_sprites;
	public Sprite[] jump_sprites;
	public Sprite[] move_sprites;


	private bool clone_created = false;
	private bool indicator_created = false;
	private int heal = 2;

	void Start()
	{
		base.Start();
		GameManager.SetGameAll();
	}

	void OnGUI()
	{
		Check();
	}



	public override void Damage(float amount)
	{
		GameManager.current_game.progression.player.changeHealth(-amount);
	}


	public override void NormalUpdate()
	{
		base.NormalUpdate();
		indicator_created = false;
		checkAction ();
		GameManager.current_game.progression.player.changeHealth(heal);
	}

	public void checkAction()
	{
		if (IN_ACTION)
		{
			IN_ACTION = false;
			for (int i = 0; i < current_collisions.Count; i++)
			{
				if (current_collisions[i].gameObject.GetComponent<Recordable>() != null)
				{
					current_collisions[i].gameObject.GetComponent<Recordable>().Action();
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
	


	private void Check()
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
			else if (Event.current.keyCode == GameManager.current_game.preferences.IN_UP)
			{
				IN_UP = true;
			}
			else if (Event.current.keyCode == GameManager.current_game.preferences.IN_DOWN)
			{
				IN_DOWN = true;
			}
			else if (Event.current.keyCode == GameManager.current_game.preferences.IN_JUMP && can_check_jump)
			{
				IN_JUMP = true;
				can_check_jump = false;
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
			else if (Event.current.keyCode == GameManager.current_game.preferences.IN_UP)
			{
				IN_UP = false;
			}
			else if (Event.current.keyCode == GameManager.current_game.preferences.IN_DOWN)
			{
				IN_DOWN = false;
			}
			else if (Event.current.keyCode == GameManager.current_game.preferences.IN_JUMP)
			{
				IN_JUMP = false;
				can_check_jump = true;
			}
			else if (Event.current.keyCode == GameManager.current_game.preferences.IN_ATTACK)
			{
				IN_ATTACK = false;
			}
		}
	}


	public override void AnimationLogic()
	{
		if (IN_LEFT)
		{
			if (grounded)
			{
				ChangeLoop(move_sprites);
			}
			else
			{
				ChangeLoop(jump_sprites);
			}
		}
		if (IN_RIGHT)
		{
			if (grounded)
			{
				ChangeLoop(move_sprites);
			}
			else
			{
				ChangeLoop(jump_sprites);
			}
		}
		if (!IN_JUMP && !IN_LEFT && !IN_RIGHT && !IN_UP && !IN_DOWN && !IN_ATTACK)
		{
			if (grounded)
			{
				ChangeLoop(still_sprites);
			}
			else
			{
				ChangeLoop(jump_sprites);
			}
		}
	}
}

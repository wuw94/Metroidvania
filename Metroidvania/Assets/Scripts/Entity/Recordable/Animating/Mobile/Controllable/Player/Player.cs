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

public class Player : Controllable
{
	private bool clone_created = false;
	private bool indicator_created = false;
	private int heal = 2;

	void Start()
	{
		base.Start();
		//GameManager.current_game = new GameManager();
		GameManager.SetGameAll();


		isPlayer = GetType() == typeof(Player);
	}


	public override void Damage(float amount)
	{
		GameManager.current_game.progression.player.changeHealth(-amount);
	}
	
	public override void NormalUpdate()
	{
		base.NormalUpdate();
		indicator_created = false;
		checkAction();
		checkTimeShift();
		checkMovementInputs(GameManager.current_game.progression.player);
		GameManager.current_game.progression.player.changeHealth(heal);
		//Debug.Log(grounded);
	}

	public override void Record()
	{
		base.Record();
		clone_created = false;
		checkAction();
		checkTimeShift();
		checkMovementInputs(GameManager.current_game.progression.player);
		GameManager.current_game.progression.player.changeHealth(heal);

		if (!indicator_created)
		{
			indicator_created = true;
			GameObject indicator = (GameObject)Instantiate(Resources.Load("Prefabs/Mobiles/Player/PlayerRecordingIndicator", typeof(GameObject)));
			indicator.transform.position = transform.position;
			indicator.GetComponent<PlayerRecordingIndicator>().this_info.animState = this_info.animState;
		}
	}
	
	public override void Rewind()
	{
		base.Rewind();
		GameManager.current_game.progression.player.changeHealth(heal);
	}

	public override void Playback()
	{
		NormalUpdate();
		if (!clone_created)
		{
			clone_created = true;
			GameObject clone = (GameObject)Instantiate(Resources.Load("Prefabs/Mobiles/Player/PlayerClone", typeof(GameObject)));
			clone.GetComponent<PlayerClone>().setRecordedStates(recorded_states);
		}
	}




}

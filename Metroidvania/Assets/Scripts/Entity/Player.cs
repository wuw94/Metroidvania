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
 * FixedUpdate() - used for adjusting rigidbody components in fixed consistency
 * 				   in this case, it maintains consistency on movement speed
 * GameManager.SetGameAll() - set player stats when game is started up
 * 
 */

public class Player : Controllable
{
	private Vector3 previousPosition;
	private bool clone_created = false;



	void Start()
	{
		GameManager.SetGameAll(10, 5, 0.5f, 5);
		GameManager.current_game.progression.character.health = 100;
	}
	
	public override void NormalUpdate()
	{
		base.NormalUpdate();
		checkTimeShift();
		checkMovementInputs(GameManager.current_game.progression.character);

		previousPosition = transform.position;

	}

	public override void Record()
	{
		base.Record();
		clone_created = false;
		Recordable.moved = previousPosition != transform.position;
		checkTimeShift();
		checkMovementInputs(GameManager.current_game.progression.character);


		previousPosition = transform.position;
	}

	public override void RecordAct()
	{
		base.RecordAct();
		Recordable.moved = previousPosition != transform.position;
		checkTimeShift();
		checkMovementInputs(GameManager.current_game.progression.character);


		previousPosition = transform.position;
	}

	public override void Rewind()
	{
		base.Rewind();
	}

	public override void Playback()
	{
		NormalUpdate();
		if (!clone_created)
		{
			clone_created = true;
			GameObject clone = (GameObject)Instantiate(Resources.Load("Prefabs/PlayerClone", typeof(GameObject)));
			clone.GetComponent<PlayerClone>().recorded_states = recorded_states;
		}
	}




}

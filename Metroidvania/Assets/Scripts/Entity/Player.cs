using UnityEngine;
using System.Collections;

/* Player.
 * Always put this on the player prefab, but make sure there is only one on the scene at any given time.
 * Not to be put on the clone of player. There's another script for that
 * 
 */

public class Player : Controllable
{
	private Vector3 previousPosition;

	void Start()
	{
		GameManager.SetGameAll(5, 5, 0.5f, 5);
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
		checkTimeShift();
		checkMovementInputs(GameManager.current_game.progression.character);

		Recordable.moved = previousPosition != transform.position;
		previousPosition = transform.position;
	}

	public override void RecordAct()
	{
		base.RecordAct();
		checkTimeShift();
		checkMovementInputs(GameManager.current_game.progression.character);

		Recordable.moved = previousPosition != transform.position;
		previousPosition = transform.position;
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Immobile.
 * Objects that inherit from the immobile class are able to:
 * -Nothing yet
 * 
 * Note:
 * Later on maybe this class will contain be inherited by scripted events, like levers
 */

public class Immobile : ReadSpriteSheet
{
	
	protected List<float> actionTime = new List<float> ();
	
	protected int previous_operation_mode;
	protected float playbackStart;
	protected float initialTime;
	protected int rewindPosition;
	
	
	public override void Action()
	{
		if (operation_mode == 1)
			actionTime.Add (Time.time);
		
	}
	public virtual void UndoAction()
	{

	}
	public override void NormalUpdate()
	{
		base.NormalUpdate ();
		previous_operation_mode = operation_mode;
	}
	
	public override void Record()
	{
		//recordInfo();
		if (previous_operation_mode != 1) {
			actionTime.Clear();
			actionTime.Add (Time.time);		
		}
		previous_operation_mode = operation_mode;
	}
	
	public override void Rewind()
	{
		//readInfo();
		if (previous_operation_mode != 2) {
			playbackStart = Time.time;
			rewindPosition = actionTime.Count - 1;
		}

		if (rewindPosition >= 0) {
			if ((Time.time - playbackStart)* rewind_speed >= (( playbackStart - actionTime[rewindPosition]))) {
					UndoAction ();
				rewindPosition--;
			}
		}
		previous_operation_mode = operation_mode;
	}
	
	public override void Playback()
	{
		if (actionTime.Count < 1)
						return;
		if(previous_operation_mode != 3)
		{
			playbackStart = Time.time;
			initialTime = actionTime[0];
		}
		
		if ((Time.time - playbackStart) >= (actionTime [0]- initialTime)) {
			if(actionTime[0] != initialTime){	
				Action ();}
			actionTime.RemoveAt (0);
		}
		previous_operation_mode = operation_mode;
	}
	
}

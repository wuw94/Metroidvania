using UnityEngine;
using System.Collections;

public class Button : Immobile{

	public GameObject player;
	public DependantPlatform[] platform;

	private bool pressed;
	private bool bycollider;

	public override void Action()
	{
		base.Action ();
		if (!bycollider)
						return;
		if (this_info.eventState == 0)
		{
			
			this_info.eventState = 1;
			foreach(DependantPlatform p in platform){
				p.changeCollisions();}
		}
		else if (this_info.eventState == 1)
		{
			
			this_info.eventState = 0;
			foreach(DependantPlatform p in platform){
				p.changeCollisions();}
		}	
		
	}
	public override void UndoAction()
	{
		Action ();
	}

	void OnTriggerEnter2D(Collider2D other)
	{

		if (other.name == "Player" && !pressed) {
			bycollider = true;
			Action ();
			bycollider = false;
		    	pressed = true;
			}
	}
	void OnTriggerExit2D(Collider2D other)
	{
	
		 if (other.name == "Player" && pressed) {
			bycollider = true;
			Action ();
			bycollider = false;
			pressed = false;
				}
		}
	
	public override void NormalUpdate()
	{
		//base.NormalUpdate();
		previous_operation_mode = operation_mode;
	}
	
	public override void Record()
	{
		base.Record ();
		//base.Record();
	}
	
	public override void Rewind()
	{
		bycollider = true;
		base.Rewind();
		bycollider = false;
		//this_info.eventState = recorded_states[record_index].eventState;
	}
	
	public override void Playback()
	{
		bycollider = true;
		base.Playback ();
		bycollider = false;
		//this_info.eventState = recorded_states[record_index].eventState;
	}
	
	
	

}

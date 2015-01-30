﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Lever : Immobile
{
	public GameObject player;
	public DependantPlatform[] platform;

	public override void Action()
	{
		base.Action ();

		if (this_info.eventState == 0)
		{

			this_info.eventState = 1;
			this_info.animState = 1;
			foreach(DependantPlatform p in platform){
				p.changeCollisions();}
		}
		else if (this_info.eventState == 1)
		{

			this_info.eventState = 0;
			this_info.animState = 0;
			foreach(DependantPlatform p in platform){
				p.changeCollisions();}
		}	

	}
	public override void UndoAction()
	{
		Action ();
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
		base.Rewind();
		//this_info.eventState = recorded_states[record_index].eventState;
	}

	public override void Playback()
	{
		base.Playback ();
		//this_info.eventState = recorded_states[record_index].eventState;
	}




}

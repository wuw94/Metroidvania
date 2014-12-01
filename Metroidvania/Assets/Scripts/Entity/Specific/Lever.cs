using UnityEngine;
using System.Collections;

public class Lever : Immobile
{
	public GameObject player;

	public override void Action()
	{
		if (this_info.eventState == 0)
		{
			this_info.eventState = 1;
			this_info.animState = 1;
		}
		else if (this_info.eventState == 1)
		{
			this_info.eventState = 0;
			this_info.animState = 0;
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
		//this_info.eventState = recorded_states[record_index].eventState;
	}

	public override void Playback()
	{
		base.Playback();
		//this_info.eventState = recorded_states[record_index].eventState;
	}



}

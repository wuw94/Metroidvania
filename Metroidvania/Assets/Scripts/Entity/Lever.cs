using UnityEngine;
using System.Collections;

public class Lever : Immobile
{
	public GameObject player;
	// Use this for initialization
	void Start () {
	
	}




	public override void NormalUpdate()
	{
		base.NormalUpdate();
		if (Vector2.Distance(transform.position, player.transform.position) < 5)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				if (this_info.eventState == 0)
				{
					this_info.eventState = 1;
				}
				else if (this_info.eventState == 1)
				{
					this_info.eventState = 0;
				}
			}
		}
	}

	public override void Record()
	{
		base.Record();
		if (Vector2.Distance(transform.position, player.transform.position) < 5)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				if (this_info.eventState == 0)
				{
					this_info.eventState = 1;
				}
				else if (this_info.eventState == 1)
				{
					this_info.eventState = 0;
				}
			}
		}
	}

	public override void RecordAct()
	{
		base.RecordAct();
		if (Vector2.Distance(transform.position, player.transform.position) < 5)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				if (this_info.eventState == 0)
				{
					this_info.eventState = 1;
				}
				else if (this_info.eventState == 1)
				{
					this_info.eventState = 0;
				}
			}
		}
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

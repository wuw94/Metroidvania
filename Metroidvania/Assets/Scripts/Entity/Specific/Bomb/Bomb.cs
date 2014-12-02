using UnityEngine;
using System.Collections;

/* Bomb.
 * Bomb takes in a delay_time (in 1/50 second) delay before exploding. This timer is affected by recording and rewinding
 * 
 * 
 * Each phase is governed by the value of eventState relative to max
 * eS = max : inactive
 * 0 <= eS < max : countdown
 * eS < 0 : exploding
 */

public class Bomb : Immobile
{
	public int delay_time = 10;
	public int power = 20;

	void Start()
	{
		this_info.eventState = delay_time;
	}

	private void doExplosion()
	{
		this_info.eventState = 0;
		Instantiate(Resources.Load("Prefabs/explosion", typeof(GameObject)), transform.position, transform.rotation);
		Camera.main.GetComponent<CameraManager>().shake_time = 0.5f;
		Destroy(gameObject);
	}

	public override void Action()
	{
		if (this_info.eventState == delay_time)
		{
			this_info.eventState = delay_time - 1;
		}
	}

	public override void NormalUpdate()
	{
		base.NormalUpdate();
		if (this_info.eventState < delay_time)
		{
			this_info.eventState--;
		}
		if (this_info.eventState < 0)
		{
			doExplosion();
		}
	}

	public override void Record()
	{
		base.Record();
		if (this_info.eventState < delay_time)
		{
			this_info.eventState--;
		}
		if (this_info.eventState < 0)
		{
			doExplosion();
		}
	}

	public override void Rewind()
	{
		base.Rewind();
		if (this_info.eventState < 0)
		{
			doExplosion();
		}
	}

	public override void Playback()
	{
		base.Playback();
		if (this_info.eventState < 0)
		{
			doExplosion();
		}
	}
}

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
	public byte delay_time = 25;
	public int power = 20;

	public bool ticking;
	private Color new_color;

	void Start()
	{
		new_color = GetComponent<Renderer>().material.color;
		this_info.eventState = delay_time;
	}

	void Update()
	{
		new_color.a = (this_info.eventState == delay_time)?1:0.5f;
		GetComponent<Renderer>().material.color = new_color;
	}

	private void doExplosion()
	{
		this_info.eventState = 0;
		Instantiate(Resources.Load("Prefabs/Immobiles/explosion", typeof(GameObject)), transform.position, transform.rotation);
		Camera.main.GetComponent<CameraManager>().shake_time = 0.5f;
		Destroy(gameObject);
	}

	public override void Action()
	{
		base.Action ();
		if (!ticking) {
						ticking = true;
						Debug.Log (this_info.eventState);
		} else {
			this_info.eventState--;
			Debug.Log (this_info.eventState);
		}

	}
	public void Tick()
	{
		if(ticking)
			Action ();
	}
	public override void UndoAction ()
	{

		if (this_info.eventState < delay_time) {
						this_info.eventState++;
		}else{
			ticking = false;
		}
	}


	public override void NormalUpdate()
	{
		base.NormalUpdate();
		Tick ();
		if (this_info.eventState == 0)
		{
			doExplosion();
		}
	}

	public override void Record()
	{
		base.Record();
		Tick ();
		if (this_info.eventState == 0)
		{
			doExplosion();
		}
	}

	public override void Rewind()
	{
		base.Rewind();
		if (this_info.eventState == 0)
		{
			doExplosion();
		}
	}

	public override void Playback()
	{
		base.Playback();
		if (this_info.eventState == 0)
		{
			doExplosion();
		}
	}
}

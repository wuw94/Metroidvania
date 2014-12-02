using UnityEngine;
using System.Collections;

public class Bomb : Immobile
{
	public float ticking_time = 10;
	public int power = 10;

	void Start()
	{
		this_info.eventState = 0; // 0 is inactive, 1: countdown, 2: exploding
	}
	
	void FixedUpdate ()
	{
		if (this_info.eventState == 1)
		{
			ticking_time -= Time.deltaTime * 10f;
		}
		if (ticking_time <= 0)
		{
			this_info.eventState = 2;
			Instantiate(Resources.Load("Prefabs/explosion", typeof(GameObject)), transform.position, transform.rotation);
			Camera.main.GetComponent<LerpFollow>().uptime = 0.5f;
			Destroy(gameObject);
		}
	}

	public override void Action()
	{
		if (this_info.eventState == 0)
		{
			this_info.eventState = 1;
		}
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if (this_info.eventState == 2)
		{
			if (col.gameObject.name == "Player")
			{
				GameManager.current_game.progression.character.changeHealth(-power);
			}
		}
	}
}

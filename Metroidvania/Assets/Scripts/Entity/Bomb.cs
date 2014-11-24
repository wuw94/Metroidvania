using UnityEngine;
using System.Collections;

public class Bomb : Recordable
{
	public float ticking_time = 10;
	public float power = 10.0f;

	void Start()
	{
		this_info.eventState = 0; // 0 is inactive, 1: countdown, 2: exploding
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (this_info.eventState == 1)
		{
			ticking_time -= Time.deltaTime * 10f;
		}
		if (ticking_time <= 0)
		{
			this_info.eventState = 2;
		}
		//print (GameManager.current_game.progression.character.health);
	}
				

	void OnTriggerStay2D(Collider2D col)
	{
		if (this_info.eventState == 0 && Input.GetKeyDown(KeyCode.Q))
		{
			this_info.eventState = 1;
		}
		if (this_info.eventState == 2)
		{
			if (col.gameObject.name == "Player")
			{
				GameManager.current_game.progression.character.health -= power;
			}
		}
	}
}

using UnityEngine;
using System.Collections;

public class Spike : Immobile
{

	// Use this for initialization
	void Start()
	{
		this_info.eventState = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if (col.gameObject.name == "Player")
		{
			GameManager.current_game.progression.character.changeHealth(-GameManager.current_game.progression.character.health_max);
		}
	}
}

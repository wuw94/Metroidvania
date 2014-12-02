using UnityEngine;
using System.Collections;

public class Explosion : Immobile
{
	public int power = 3;

	void OnTriggerStay2D(Collider2D col)
	{
		if (col.gameObject.name == "Player")
		{
			GameManager.current_game.progression.character.changeHealth(-power);
		}
	}
}

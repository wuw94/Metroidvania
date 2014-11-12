using UnityEngine;
using System.Collections;

/* Player.
 * Always put this on the player prefab, but make sure there is only one on the scene at any given time.
 * 
 */

public class Player : Controllable
{
	void Start()
	{
		GameManager.SetGameAll(5, 5, 0.5f, 5);
	}
	
	void FixedUpdate()
	{
		checkMovementInputs(GameManager.current_game.progression.character);
	}
}

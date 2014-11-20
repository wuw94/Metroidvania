using UnityEngine;
using System.Collections;

/* Player.
 * Always put this on the player prefab, but make sure there is only one on the scene at any given time.
 * 
 * Script Functionalities
 * 
 * Functions:
 * Start() - calls functions when game is started
 * FixedUpdate() - used for adjusting rigidbody components in fixed consistency
 * 				   in this case, it maintains consistency on movement speed
 * GameManager.SetGameAll() - set player stats when game is started up
 * 
 */

public class Player : Controllable
{
	void Start()
	{
		GameManager.SetGameAll(10, 5, 0.5f, 5);
	}
	
	void FixedUpdate()
	{
		checkMovementInputs(GameManager.current_game.progression.character);
	}
}

using UnityEngine;
using System.Collections;

/* Game Manager.
 * Revolves around the static variable, current_game, which stores almost all the game's data
 * Access the data from anywhere with:
 * GameManager.current_game
 * 
 * For debugging purposes, a static function will OVERWRITE all current data (or create a game) with new parameters:
 * GameManager.SetGameAll(...)
 * 
 * auth Wesley Wu
 */

[System.Serializable]
public sealed class GameManager
{
	public static GameManager current_game; // Holds the game state, accessable from anywhere

	public ProgressionManager progression;
	public PreferenceManager preferences;

	public GameManager()
	{
		this.progression = new ProgressionManager();
		this.preferences = new PreferenceManager();
	}

	public static void SetGameAll(float msm, float msag, float msaa, float js)
	{
		// Parameters in this order:
		// 1. move speed maximum
		// 2. move speed acceleration (ground)
		// 3. move speed acceleration (air)
		// 4. jump speed
		current_game = new GameManager();
		current_game.progression.character.move_speed_max = msm;
		current_game.progression.character.move_speed_accel_ground = msag;
		current_game.progression.character.move_speed_accel_air = msaa;
		current_game.progression.character.jump_speed = js;
	}
}

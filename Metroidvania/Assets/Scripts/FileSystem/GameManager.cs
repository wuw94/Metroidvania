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
 * 
 * Script Functionalities
 * 
 * Method:
 * SetGameAll() - assigns each variable with the parameter arguments
 * GameManager() - creates a new object for progression and preferences
 * 
 * Variables:
 * current_game - GameManager variable
 * progression - ProgressionManager variable
 * character - CharacterManager variable
 * 
 * current_game.progression.character - uses variables from CharacterManager affected by ProgressionManager into GameManager
 * 
 * Type:
 * static - shared class variable across all three managers
 */

/// <summary>
/// Stores information about the game.
/// </summary>
[System.Serializable]
public sealed class GameManager
{
	/// <summary>
	/// The current game session, accessible from anywhere.
	/// </summary>
	public static GameManager current_game;

	/// <summary>
	/// Progression for the game.
	/// </summary>
	public ProgressionManager progression;

	/// <summary>
	/// Player's preferences for the game.
	/// </summary>
	public PreferenceManager preferences;

	/// <summary>
	/// Constructor.
	/// </summary>
	public GameManager()
	{
		this.progression = new ProgressionManager();
		this.preferences = new PreferenceManager();
	}

	/// <summary>
	/// Reminder, we need to change this.
	/// </summary>
	public static void SetGameAll(float msm, float msag, float msaa, float js)
	{
		// Parameters in this order:
		// 1. move speed maximum
		// 2. move speed acceleration (ground)
		// 3. move speed acceleration (air)
		// 4. jump speed
		current_game.progression.character.move_speed_max = msm;
		current_game.progression.character.move_speed_accel_ground = msag;
		current_game.progression.character.move_speed_accel_air = msaa;
		current_game.progression.character.jump_speed = js;
	}
}

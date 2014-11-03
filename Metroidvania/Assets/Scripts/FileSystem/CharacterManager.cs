using UnityEngine;
using System.Collections;

/* Character Manager.
 * Stores information about the player's character
 * 
 * -name- stores the character's name
 * -equipment- stores information about which equipment the player has
 * 
 * auth Wesley Wu
 */

[System.Serializable]
public sealed class CharacterManager
{
	public string name;
	public int equipment;
	
	public CharacterManager()
	{
		this.name = "";
	}
}

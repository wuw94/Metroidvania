using UnityEngine;
using System.Collections;

[System.Serializable]
public sealed class ProgressionManager
{
	public CharacterManager character;
	public Map map;

	public ProgressionManager()
	{
		this.character = new CharacterManager();
		this.map = new Map();
	}
}

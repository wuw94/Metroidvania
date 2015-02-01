using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
	public int WhatIndexAmI()
	{
		return GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[3].IndexOf(this);
	}
}

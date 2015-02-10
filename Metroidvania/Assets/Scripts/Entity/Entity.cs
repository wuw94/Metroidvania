using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{
	public List<Equipment> equipment = new List<Equipment>();

	public enum GooColor{Blue, Red, Purple};

	public int WhatIndexAmI()
	{
		return GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[ResourceDirectory.resource[this.GetType()].index].IndexOf(this);
	}
}

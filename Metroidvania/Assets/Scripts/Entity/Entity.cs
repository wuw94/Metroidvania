using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{


	public enum GooColor{Blue, Red, Purple};

	/// <summary>
	/// Returns an integer defining the index of this object inside ThisMap.entities[ThisObjectType].
	/// Used for objects that are referenced, such as DependantPlatforms.
	/// </summary>
	/// <returns>The index of this.</returns>
	public int WhatIndexAmI()
	{
		return GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[ResourceDirectory.resource[this.GetType()].index].IndexOf(this);
	}
}

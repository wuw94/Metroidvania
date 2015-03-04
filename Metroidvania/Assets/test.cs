using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;



public class test : MonoBehaviour
{
	void Start()
	{
		if (PlayerPrefs.GetString("Map Name") == "map2")
		{
			Debug.Log("destroy");
			Destroy(gameObject);
		}
	}
}
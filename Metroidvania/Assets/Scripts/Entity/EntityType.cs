using UnityEngine;
using System.Collections;

public static class EntityType
{
	public static string[] list_type = {
		"Camera",		// 0
		"Player",		// 1
		"Ground"		// 2
	};

	public static int[] ignore_type = {
		0
	};

	public static Hashtable hash_type = new Hashtable{
		{"Camera", 0},
		{"Player", 1},
		{"Ground", 2}
	};
}

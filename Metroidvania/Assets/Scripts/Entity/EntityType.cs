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

	public static int extractNameAsNumber(string name)
	{
		return (int)hash_type[name.Substring(0, name.IndexOfAny(new char[] {'1','2','3','4','5','6','7','8','9','0'}))];
	}

	public static int extractNumber(string name)
	{
		int i;
		int.TryParse(name.Substring(name.IndexOfAny(new char[] {'1','2','3','4','5','6','7','8','9','0'})), out i);
		return i;
	}

	public static string newName(string prefab_type)
	{
		int i = 0;
		while (GameObject.Find(prefab_type + i.ToString()) != null)
		{
			i++;
		}
		return prefab_type + i.ToString();
	}
}

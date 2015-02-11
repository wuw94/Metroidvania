using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DetectLeaks : MonoBehaviour
{
	bool show = false;

	private long total_memory_allocated;
	private long memory_allocated_low;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			show = !show;
		}
	}

	void OnGUI()
	{
		if (show)
		{
			Object[] objects = FindObjectsOfType(typeof (UnityEngine.Object));
			
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			
			foreach(Object obj in objects)
			{
				string key = obj.GetType().ToString();
				if(dictionary.ContainsKey(key))
				{
					dictionary[key]++;
				} 
				else
				{
					dictionary[key] = 1;
				}
			}
			
			List<KeyValuePair<string, int>> myList = new List<KeyValuePair<string, int>>(dictionary);
			myList.Sort(
				delegate(KeyValuePair<string, int> firstPair,
			         KeyValuePair<string, int> nextPair)
				{
				return nextPair.Value.CompareTo((firstPair.Value));
			}
			);
			
			foreach (KeyValuePair<string, int> entry in myList)
			{
				GUILayout.Label(entry.Key + ": " + entry.Value);
			}


			// GC Memory
			if (System.GC.GetTotalMemory(false) < total_memory_allocated)
			{
				memory_allocated_low = System.GC.GetTotalMemory(false);
			}
			GUI.Label(new Rect(10,Screen.height - 60, 500, 20), Application.dataPath);
			GUI.Label(new Rect(10,Screen.height - 40, 500, 20), "Memory Allocated (now): " + ((float)total_memory_allocated / 1000000).ToString("n2") + "MB");
			GUI.Label(new Rect(10,Screen.height - 20, 500, 20), "Memory Allocated (low): " + ((memory_allocated_low == 0) ? "Uncollected" : ((float)memory_allocated_low / 1000000).ToString("n2") + "MB"));
			total_memory_allocated = System.GC.GetTotalMemory(false);
		}
		
	}
}
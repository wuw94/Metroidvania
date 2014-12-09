using UnityEngine;
using System.Collections;

using UnityEditor;

public class TileEditorManager : EditorWindow
{
	[MenuItem("TileEditor/Save")]
	static void Save()
	{
		if (EditorApplication.isPlaying)
		{
			string[] path = EditorApplication.currentScene.Split(char.Parse("/"));
			path[path.Length-1] = "AutoSave_" + path[path.Length-1];
			if (EditorApplication.SaveScene(string.Join("/", path)))
			{
				Debug.Log("scene saved at: " + string.Join("/", path));
			}
			else
			{
				Debug.LogWarning("scene failed to save properly");
			}
		}
	}

}

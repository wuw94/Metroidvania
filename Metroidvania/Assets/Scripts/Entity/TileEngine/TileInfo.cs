using UnityEngine;
using System.Collections;

[System.Serializable]
public struct TileInfo
{
	public bool active;
	public int type;
	
	public TileInfo(bool a, int t)
	{
		active = a;
		type = t;
	}
}

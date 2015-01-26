using UnityEngine;
using System.Collections;

[System.Serializable]
public struct TileInfo
{
	public bool active;
	public byte type;

	public TileInfo(bool a, byte t)
	{
		active = a;
		type = t;
	}
}

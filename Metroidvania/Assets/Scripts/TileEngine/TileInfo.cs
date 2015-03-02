using UnityEngine;
using System.Collections;

[System.Serializable]
public struct TileInfo
{
	public bool active;
	public byte type;
	public bool outdoor;

	public TileInfo(bool a, byte t, bool o)
	{
		active = a;
		type = t;
		outdoor = o;
	}
}

using UnityEngine;
using System.Collections;

[System.Serializable]
public class PseudoVector3
{
	public float x;
	public float y;
	public float z;

	public PseudoVector3()
	{
		this.x = 0;
		this.y = 0;
		this.z = 0;
	}

	public PseudoVector3(Vector2 v)
	{
		this.x = v.x;
		this.y = v.y;
		this.z = 0;
	}

	public PseudoVector3(Vector3 v)
	{
		this.x = v.x;
		this.y = v.y;
		this.z = v.z;
	}
}

using UnityEngine;
using System.Collections;

/// <summary>
/// PseudoVector3 is [Serializable] and implicitly castable from Vector3.
/// </summary>
[System.Serializable]
public sealed class PseudoVector3
{
	/// <summary>
	/// x data
	/// </summary>
	public float x;
	
	/// <summary>
	/// y data
	/// </summary>
	public float y;

	/// <summary>
	/// z data
	/// </summary>
	public float z;
	
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	public PseudoVector3(float rx, float ry, float rz)
	{
		x = rx;
		y = ry;
		z = rz;
	}
	
	/// <summary>
	/// Returns a string representation of the object
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		return string.Format("PseudoVector3({0}, {1}, {2})", x, y, z);
	}
	
	/// <summary>
	/// Automatic conversion from PseudoVector3 to Vector3
	/// </summary>
	/// <param name="rValue"></param>
	/// <returns></returns>
	public static implicit operator Vector3(PseudoVector3 rValue)
	{
		return new Vector3(rValue.x, rValue.y, rValue.z);
	}
	
	/// <summary>
	/// Automatic conversion from Vector3 to PseudoVector3
	/// </summary>
	/// <param name="rValue"></param>
	/// <returns></returns>
	public static implicit operator PseudoVector3(Vector3 rValue)
	{
		return new PseudoVector3(rValue.x, rValue.y, rValue.z);
	}
	
}

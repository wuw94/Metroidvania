using UnityEngine;
using System.Collections;

/// <summary>
/// PseudoVector2 is [Serializable] and implicitly castable from Vector2.
/// </summary>
[System.Serializable]
public sealed class PseudoVector2
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
	/// Constructor
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	public PseudoVector2(float rx, float ry)
	{
		x = rx;
		y = ry;
	}

	/// <summary>
	/// Returns a string representation of the object
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		return string.Format("PseudoVector2({0}, {1})", x, y);
	}

	/// <summary>
	/// Automatic conversion from PseudoVector2 to Vector2
	/// </summary>
	/// <param name="rValue"></param>
	/// <returns></returns>
	public static implicit operator Vector2(PseudoVector2 rValue)
	{
		return new Vector2(rValue.x, rValue.y);
	}

	/// <summary>
	/// Automatic conversion from Vector2 to PseudoVector2
	/// </summary>
	/// <param name="rValue"></param>
	/// <returns></returns>
	public static implicit operator PseudoVector2(Vector2 rValue)
	{
		return new PseudoVector2(rValue.x, rValue.y);
	}

}

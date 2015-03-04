using UnityEngine;
using System.Collections;

/// <summary>
/// PseudoColor is [Serializable] and implicitly castable from Color.
/// </summary>
[System.Serializable]
public sealed class PseudoColor
{

	/// <summary>
	/// r data
	/// </summary>
	public float r;
	
	/// <summary>
	/// g data
	/// </summary>
	public float g;

	/// <summary>
	/// b data
	/// </summary>
	public float b;

	/// <summary>
	/// a data
	/// </summary>
	public float a;
	
	/// <summary>
	/// Constructor
	/// </summary>
	public PseudoColor(float rr, float rg, float rb, float ra)
	{
		r = rr;
		g = rg;
		b = rb;
		a = ra;
	}
	
	/// <summary>
	/// Returns a string representation of the object
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		return string.Format("PseudoColor({0}, {1}, {2}, {3})", r, g, b, a);
	}
	
	/// <summary>
	/// Automatic conversion from PseudoColor to Color
	/// </summary>
	/// <param name="rValue"></param>
	/// <returns></returns>
	public static implicit operator Color(PseudoColor rValue)
	{
		return new Color(rValue.r, rValue.g, rValue.b, rValue.a);
	}
	
	/// <summary>
	/// Automatic conversion from Color to PseudoColor
	/// </summary>
	/// <param name="rValue"></param>
	/// <returns></returns>
	public static implicit operator PseudoColor(Color rValue)
	{
		return new PseudoColor(rValue.r, rValue.g, rValue.b, rValue.a);
	}

}
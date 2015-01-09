using UnityEngine;
using System.Collections;

public sealed class CustomCircle
{
	/// <summary>
	/// The CustomCircle's center.
	/// </summary>
	public Vector2 center;

	/// <summary>
	/// The CustomCircle's radius.
	/// </summary>
	public float radius;

	/// <summary>
	/// Constructor.
	/// </summary>
	public CustomCircle(Vector2 c, float r)
	{
		center = c;
		radius = r;
	}

	/// <summary>
	/// Returns a string representation of the object
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		return string.Format("CustomCircle(center:({0},{1}), radius:{2})", center.x, center.y, radius);
	}
}

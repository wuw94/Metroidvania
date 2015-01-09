using UnityEngine;
using System.Collections;

public sealed class CustomRect
{
	public Vector2 center;
	public float width;
	public float height;
	
	/// <summary>
	/// Constructor
	/// </summary>
	public CustomRect(Vector2 c, float w, float h)
	{
		center = c;
		width = w;
		height = h;
	}

	/// <summary>
	/// x position of left side
	/// </summary>
	public float left()
	{
		return center.x - width/2;
	}

	/// <summary>
	/// x position of right side
	/// </summary>
	public float right()
	{
		return center.x + width/2;
	}

	/// <summary>
	/// y position of top side
	/// </summary>
	public float top()
	{
		return center.y + height/2;
	}

	/// <summary>
	/// y position of bottom side
	/// </summary>
	public float bottom()
	{
		return center.y - height/2;
	}

	/// <summary>
	/// Returns a string representation of the object
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		return string.Format("CustomRect(center:({0},{1}), width:{2}, height:{3})", center.x, center.y, width, height);
	}

	/// <summary>
	/// Automatic conversion from CustomRect to Rect
	/// </summary>
	/// <param name="rValue"></param>
	/// <returns></returns>
	public static implicit operator Rect(CustomRect rValue)
	{
		return new Rect(rValue.left(), rValue.top(), rValue.width, rValue.height);
	}
	
	/// <summary>
	/// Automatic conversion from Rect to CustomRect
	/// </summary>
	/// <param name="rValue"></param>
	/// <returns></returns>
	public static implicit operator CustomRect(Rect rValue)
	{
		return new CustomRect(rValue.center, rValue.width, rValue.height);
	}
}

using UnityEngine;
using System.Collections;

public sealed class CollisionManager
{
	/// <summary>
	/// Based on two specified objects, will return a bool of whether a collision has occurred.
	/// Finds mobile's correction parameter to determine whether that object should correct their location.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="other">Other.</param>
	public static bool ManageCollision(Mobile obj, Mobile other)
	{
		// How collision is managed.
		// 1. lower velocity if its higher than collider size (meaning could pass through wall)
		// 2. use velocities of current objects to determine future position
		// 3. check if collision happens there. if so, repositions objects based on mass and contact point
		// 4. reposition involves taking the collision rectangle, and moving backwards in velocity until the area of collision rectangle is 0
		//
		// We check: if (other.y) - (obj.y) / y > 0, this means the collision must be a horizontal collision
		// Thus we can shift the object backwards based on x, and scale upward based on y

		Vector2 obj_velocity = obj.rigidbody2D.velocity;
		Vector2 other_velocity = other.rigidbody2D.velocity;
		if (obj_velocity.x > obj.collider_r.width)
		{
			obj_velocity /= obj.collider_r.width;
			other_velocity /= other.collider_r.width;
		}
		if (obj_velocity.y > obj.collider_r.height)
		{
			obj_velocity /= obj.collider_r.height;
			other_velocity /= obj.collider_r.height;
		}
		if (other_velocity.x > other.collider_r.width)
		{
			obj_velocity /= other.collider_r.width;
		}
		if (other_velocity.y > other.collider_r.height)
		{
			obj_velocity /= other.collider_r.height;
		}
		
		if (obj.correction && other.correction)
		{
			if (true)
			{
				//horizontal movement collision
			}
			else
			{
				//vertical movement collision
			}
		}

		Vector2 obj_predicted_position = (Vector2)obj.transform.position + (Vector2)obj_velocity;
		Vector2 other_predicted_position = (Vector2)other.transform.position + (Vector2)other_velocity;

		if (intersects(new CustomRect(obj.transform.position, obj.collider_r.width, obj.collider_r.height), new CustomRect(other.transform.position, other.collider_r.width, other.collider_r.height)) ||
		    intersects(new CustomRect(obj.transform.position, obj.collider_r.width, obj.collider_r.height), new CustomCircle(other.transform.position, other.collider_c.radius)) ||
		    intersects(new CustomCircle(obj.transform.position, obj.collider_c.radius), new CustomRect(other.transform.position, other.collider_r.width, other.collider_r.height)) ||
		    intersects(new CustomCircle(obj.transform.position, obj.collider_c.radius), new CustomCircle(other.transform.position, other.collider_c.radius)))
		{
			return true;
		}

		return false;

	}

	/// <summary>
	/// Checks for intersection between two CustomRectangle
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="other">Other.</param>
	public static bool intersects(CustomRect obj, CustomRect other)
	{
		if (obj == null || other == null) {return false;}
		bool cond1 = obj.right() < other.left();
		bool cond2 = obj.left() > other.right();
		bool cond3 = obj.top() < other.bottom();
		bool cond4 = obj.bottom() > other.top();
		return cond1 && cond2 && cond3 && cond4;
	}

	/// <summary>
	/// Checks for intersection between two CustomCircle objects
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="other">Other.</param>
	public static bool intersects(CustomCircle obj, CustomCircle other)
	{
		if (obj == null || other == null) {return false;}
		float dx = other.center.x - obj.center.x;
		float dy = other.center.y - obj.center.y;
		float r = obj.radius + other.radius;
		return (dx * dx + dy * dy) <= r * r;
	}

	/// <summary>
	/// Checks for intersection between a CustomRect and a CustomCircle
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="other">Other.</param>
	public static bool intersects(CustomRect obj, CustomCircle other)
	{
		if (obj == null || other == null) {return false;}
		float closest_x = (other.center.x < obj.left() ? obj.left() : (other.center.x > obj.right() ? obj.right() : other.center.x));
		float closest_y = (other.center.y < obj.top() ? obj.top() : (other.center.y > obj.bottom() ? obj.bottom() : other.center.y));
		float dx = closest_x - other.center.x;
		float dy = closest_y - other.center.y;
		return (dx * dx + dy * dy) <= other.radius * other.radius;
	}

	/// <summary>
	/// Checks for intersection between a CustomCircle and a CustomRect
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="other">Other.</param>
	public static bool intersects(CustomCircle obj, CustomRect other)
	{
		if (obj == null || other == null) {return false;}
		float closest_x = (obj.center.x < other.left() ? other.left() : (obj.center.x > other.right() ? other.right() : obj.center.x));
		float closest_y = (obj.center.y < other.top() ? other.top() : (obj.center.y > other.bottom() ? other.bottom() : obj.center.y));
		float dx = closest_x - obj.center.x;
		float dy = closest_y - obj.center.y;
		return (dx * dx + dy * dy) <= obj.radius * obj.radius;
	}
}

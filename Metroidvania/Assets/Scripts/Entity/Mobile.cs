using UnityEngine;
using System.Collections;

/* Mobile.
 * Objects that inherit from the Mobile class are able to:
 * 1. Detect collisions with ground or walls
 * 2. Move left and right correctly, grounded or ungrounded, based on given parameters
 * 3. Jump correctly based on given parameters
 * 
 * Note:
 * Later on we'll implement multiple jumps
 */

public class Mobile : Recordable
{
	Vector3 savedVelocity;
	bool grounded = false;
	string impassabletype = "ImpassableTopBottom";

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == impassabletype)
		{
			grounded = true;
		}
	}

	public void MobileNormalUpdate()
	{
		rigidbody2D.isKinematic = false;
	}

	public void MobileRecord()
	{
	}

	public void MobileRewind()
	{
		rigidbody2D.isKinematic = true;
	}

	public void MobilePlayback()
	{
	}
	
	void OnCollisionExit2D(Collision2D col)
	{
		if (col.gameObject.tag == impassabletype)
		{
			grounded = false;
		}
	}

	public void moveLeft(float max, float accel_g, float accel_a)
	{
		if (rigidbody2D.velocity.x > -max)
		{
			if (grounded)
			{
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x - accel_g, rigidbody2D.velocity.y);
			}
			else
			{
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x - accel_a, rigidbody2D.velocity.y);
			}
		}
		else
		{
			rigidbody2D.velocity = new Vector2(-max, rigidbody2D.velocity.y);
		}
	}

	public void moveRight(float max, float accel_g, float accel_a)
	{
		if (rigidbody2D.velocity.x < max)
		{
			if (grounded)
			{
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x + accel_g, rigidbody2D.velocity.y);
			}
			else
			{
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x + accel_a, rigidbody2D.velocity.y);
			}
		}
		else
		{
			rigidbody2D.velocity = new Vector2(max, rigidbody2D.velocity.y);
		}
	}

	public void jump(float jumpspeed)
	{
		if (grounded)
		{
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y + jumpspeed);
		}
	}


}

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
	bool grounded = false;
	string tagtype = "Ground";

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == tagtype)
		{
			grounded = true;
		}
	}

	void OnCollisionStay2D(Collision2D col)
	{
		if (col.gameObject.tag == tagtype)
		{
			grounded = true;
		}
	}

	void OnCollisionExit2D(Collision2D col)
	{
		if (col.gameObject.tag == tagtype)
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
		if (rigidbody2D.isKinematic == true)
		{
			rigidbody2D.isKinematic = false;
		}
		if (grounded)
		{
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y + jumpspeed);
			savedVelocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y + jumpspeed);
		}

	}



	
	public override void NormalUpdate()
	{
		if (rigidbody2D.isKinematic == true)
		{
			rigidbody2D.isKinematic = false;
			transform.rigidbody2D.velocity = savedVelocity;
		}
	}
	
	public override void Record()
	{
		if (rigidbody2D.isKinematic == false)
		{
			savedVelocity = transform.rigidbody2D.velocity;
			rigidbody2D.isKinematic = true;
		}
		recordInfo();
	}

	public override void RecordAct()
	{
		if (rigidbody2D.isKinematic == true)
		{
			rigidbody2D.isKinematic = false;
			transform.rigidbody2D.velocity = savedVelocity;
		}
		recordInfo();
	}
	
	public override void Rewind()
	{
		if (rigidbody2D.isKinematic == false)
		{
			savedVelocity = transform.rigidbody2D.velocity;
			rigidbody2D.isKinematic = true;
		}
		readInfo();
	}
	
	public override void Playback()
	{
		readInfo();
	}
}

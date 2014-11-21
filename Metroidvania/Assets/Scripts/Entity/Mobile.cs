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
 * 
 * Script Functionalities
 * 
 * Variables:
 * grounded - boolean variable depicting if you're on the ground (y = 0) or in the air (y > 0)(jumping)
 * 		- if True,  player movement speed of accel_g
 * 		- if False, player movement speed of accel_a
 * impassabletype - prevent passing through gameObjects when colliding
 * rigidbody2D.velocity.x - moves the player left/right and collides with gameObjects
 * rigidbody2D.velocity.y - moves the player up/down and collides with gameObjects
 * 
 * In Parameter Arguments:
 * col - variable of Collision2D to indicate its gameObject
 * max - maintain a constant movement speed if x exceeds max or else move at max's speed
 * accel_g - movement speed while on ground
 * accel_a - movement speed while in the air (slightly slower than accel_g)
 * jumpspeed - when player jumps (grounded == true), increase y based on jumpspeed
 * 
 * Functions:
 * OnCollisionEnter2D - sent when an incoming collider makes contact with this object's collider
 * 						in this case, when the Player touches Ground
 * OnCollisionExit2D  - sent when a collider on another object stops touching this object's collider
 * 						in this case, when the Player is not touching Ground
 * moveLeft  - -x (left) movement when player hits left key
 * moveRight - x (right) movement when player hits right key
 * jump - y (jumping) movement when player hits jump key
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
		if (Mathf.Abs(rigidbody2D.velocity.x) < max)
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
		if (Mathf.Abs (rigidbody2D.velocity.x) < max)
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
		if (rigidbody2D.isKinematic)
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
		if (rigidbody2D.isKinematic)
		{
			rigidbody2D.isKinematic = false;
			transform.rigidbody2D.velocity = savedVelocity;
		}
	}
	
	public override void Record()
	{
		if (!rigidbody2D.isKinematic)
		{
			savedVelocity = transform.rigidbody2D.velocity;
			rigidbody2D.isKinematic = true;
		}
		recordInfo();
	}

	public override void RecordAct()
	{
		if (rigidbody2D.isKinematic)
		{
			rigidbody2D.isKinematic = false;
			transform.rigidbody2D.velocity = savedVelocity;
		}
		recordInfo();
	}
	
	public override void Rewind()
	{
		if (!rigidbody2D.isKinematic)
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

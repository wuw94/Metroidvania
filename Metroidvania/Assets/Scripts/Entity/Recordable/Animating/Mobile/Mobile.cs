using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

public class Mobile : Animating
{
	public float grav_scale;
	protected bool is_attacking = false;
	protected bool control_enabled = true;
	bool velocity_assigned = false;
	protected bool isPlayer;

	// for collision type checking
	public bool accurate_check;
	private float check_radius = 0.1f;
	public LayerMask foreground;

	public Transform ground_check_left;
	public Transform ground_check_right;
	protected bool grounded = false;

	public Transform wall_check_top;
	public Transform wall_check_bottom;
	public Transform wall_check_back;
	protected bool front_contact = false;
	protected bool back_contact = false;

	public bool updraft_contact = false;



	public void Start()
	{
		base.Start();
		if (Application.loadedLevelName == "TileEditor")
		{
			rigidbody2D.isKinematic = true;
		}
	}

	public void noInput()
	{
		if (updraft_contact)
		{
			rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y + ((rigidbody2D.velocity.y < 1)?1:0));
		}
		else
		{
			if (front_contact)
			{
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, (rigidbody2D.velocity.y > 0 ? rigidbody2D.velocity.y : rigidbody2D.velocity.y));
			}
			if (grounded)
			{
				ChangeLoop(still_sprites);
				rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
			}
			else
			{
				ChangeLoop(jump_sprites);
			}
		}
	}

	public void moveLeft(float max, float accel_g, float accel_a)
	{
		this_info.facingRight = false;

		if (control_enabled)
		{
			if (updraft_contact)
			{
				rigidbody2D.velocity = new Vector2(-max, rigidbody2D.velocity.y + ((rigidbody2D.velocity.y < 1)?1:0));
			}
			else
			{
				if (grounded)
				{
					ChangeLoop(move_sprites);
					rigidbody2D.velocity = new Vector2(-max, rigidbody2D.velocity.y);
				}
				else
				{
					ChangeLoop(jump_sprites);
					if (rigidbody2D.velocity.x > -max)
					{
						rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x - accel_a, rigidbody2D.velocity.y);
					}
					if (front_contact)
					{
						rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, (rigidbody2D.velocity.y > 0 ? rigidbody2D.velocity.y : 0));
					}
				}
			}
		}
	}

	public void moveRight(float max, float accel_g, float accel_a)
	{
		this_info.facingRight = true;

		if (control_enabled)
		{
			if (updraft_contact)
			{
				rigidbody2D.velocity = new Vector2(max, rigidbody2D.velocity.y + ((rigidbody2D.velocity.y < 1)?1:0));
			}
			else
			{
				if (grounded)
				{
					ChangeLoop(move_sprites);
					rigidbody2D.velocity = new Vector2(max, rigidbody2D.velocity.y);
				}
				else
				{
					ChangeLoop(jump_sprites);
					if (rigidbody2D.velocity.x < max)
					{
						rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x + accel_a, rigidbody2D.velocity.y);
					}
					if (front_contact)
					{
						rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, (rigidbody2D.velocity.y > 0 ? rigidbody2D.velocity.y : 0));
					}
				}
			}
		}
	}

	public void moveUp(float max, float accel_g, float accel_a)
	{
		if (control_enabled)
		{
			if (updraft_contact)
			{
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y + ((rigidbody2D.velocity.y < 2)?2:0));
			}
			else
			{
				if (front_contact)
				{
					rigidbody2D.velocity = new Vector2(this_info.facingRight ? 1 : -1, max);
				}
			}
		}
	}

	public void moveDown(float max, float accel_g, float accel_a)
	{
		if (control_enabled)
		{
			if (updraft_contact)
			{
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y + 0.3f);
			}
			else
			{
				if (front_contact)
				{
					rigidbody2D.velocity = new Vector2(this_info.facingRight ? 1 : -1, -max);
				}
			}
		}
	}

	public void Attack(float max, float accel_g, float accel_a)
	{
		//animate(attack_frames, attack_frame_delay);

	}

	public void jump(float jumpspeed)
	{
		if (updraft_contact)
		{
		}
		else
		{
			if (grounded)
			{
				if (rigidbody2D.velocity.y < jumpspeed)
				{
					rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpspeed);
				}
			}
			else
			{
				if (front_contact && !this_info.facingRight)
				{
					StartCoroutine(DisableControl(0.2f, GameManager.current_game.preferences.IN_RIGHT));
					rigidbody2D.velocity = new Vector2(jumpspeed, 0);
				}
				else if (front_contact && this_info.facingRight)
				{
					StartCoroutine(DisableControl(0.2f, GameManager.current_game.preferences.IN_LEFT));
					rigidbody2D.velocity = new Vector2(-jumpspeed, 0);
				}
				else if (back_contact && this_info.facingRight)
				{
					StartCoroutine(DisableControl(0.2f, GameManager.current_game.preferences.IN_RIGHT));
					rigidbody2D.velocity = new Vector2(jumpspeed, 0);
				}
				else if (back_contact && !this_info.facingRight)
				{
					StartCoroutine(DisableControl(0.2f, GameManager.current_game.preferences.IN_LEFT));
					rigidbody2D.velocity = new Vector2(-jumpspeed, 0);
				}
			}
		}
	}

	private void manageGravityScale()
	{
		if (updraft_contact)
		{
			rigidbody2D.gravityScale = 0;
		}
		else
		{
			rigidbody2D.gravityScale = grav_scale;
		}
	}

	private void manageVelocity()
	{
		if (isPlayer)
		{
			assignVelocity(0,3);
		}
		else
		{
			assignVelocity(1,0);
		}
	}

	private void assignVelocity(int save_mode, int load_mode)
	{
		if (operation_mode == save_mode || operation_mode == load_mode)
		{
			if (!velocity_assigned)
			{
				transform.rigidbody2D.velocity = savedVelocity;
				velocity_assigned = true;
			}
			else
			{
				savedVelocity = transform.rigidbody2D.velocity;
			}
		}
		else
		{
			velocity_assigned = false;
		}
	}

	private void checkCollisions()
	{
		if (accurate_check)
		{
			Collider2D gL = Physics2D.OverlapCircle(ground_check_left.position, check_radius, foreground);
			Collider2D gR = Physics2D.OverlapCircle(ground_check_right.position, check_radius, foreground);
			grounded = 	(gL != null && gL.tag == "Ground") || (gR != null && gR.tag == "Ground");
			Collider2D wT = Physics2D.OverlapCircle(wall_check_top.position, check_radius, foreground);
			Collider2D wB = Physics2D.OverlapCircle(wall_check_bottom.position, check_radius, foreground);
			front_contact = (wT != null && wT.tag == "Ground") || (wB != null && wB.tag == "Ground");
			Collider2D wBa = Physics2D.OverlapCircle(wall_check_back.position, check_radius, foreground);
			back_contact = wBa != null && wBa.tag == "Ground";
		}
		else
		{
		}
		if (GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].tiles[(int)transform.position.x][(int)(collider2D.bounds.min.y)].active)
		{
			rigidbody2D.gravityScale = 0;
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
		}

	}

	public IEnumerator DisableControl(float time, KeyCode k)
	{
		control_enabled = false;
		for (int i = 0; i < time * 50; i++)
		{
			if (Input.GetKey(k))
			{
				control_enabled = true;
			}
			yield return new WaitForFixedUpdate();
			//yield return new WaitForSeconds(time);
		}
		control_enabled = true;
	}

	public override void NormalUpdate()
	{
		base.NormalUpdate();
		manageGravityScale();
		checkCollisions();
		manageVelocity();
	}
	
	public override void Record()
	{
		base.Record();
		manageGravityScale();
		manageVelocity();
		recordInfo();
	}

	public override void Rewind()
	{
		base.Rewind();
		manageGravityScale();
		manageVelocity();
		readInfo();
	}
	
	public override void Playback()
	{
		base.Playback();
		manageGravityScale();
		manageVelocity();
		readInfo();
	}
}

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
 * jump_speed - when player jumps (grounded == true), increase y based on jump_speed
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

public class Mobile : Control
{
	public EquipmentManager equipment;

	public float grav_scale;
	protected bool is_attacking = false;
	protected bool control_enabled = true;
	bool velocity_assigned = false;
	
	// for collision type checking
	public bool accurate_check;
	private float check_radius = 0.05f;
	public LayerMask foreground;

	public Transform ground_check_left;
	public Transform ground_check_right;
	public bool grounded = false;

	public Transform wall_check_top;
	public Transform wall_check_bottom;
	public Transform wall_check_back;
	protected bool front_contact = false;
	protected bool back_contact = false;

	public bool updraft_contact = false;
	public bool parachute_use = false;
	public bool on_ladder = false;
	public bool time_zone_contact = false;

	public float move_speed_base;
	public float jump_speed_base;
	public float move_speed_mut;
	public float jump_speed_mut;


	
	public List<Collider2D> current_collisions = new List<Collider2D>();

	void OnTriggerEnter2D(Collider2D col)
	{
		if (Application.loadedLevelName == "TileEditor"){return;}
		if (!current_collisions.Contains(col))
		{
			current_collisions.Add(col);
		}
	}
	
	void OnTriggerExit2D(Collider2D col)
	{
		if (Application.loadedLevelName == "TileEditor"){return;}
		if (current_collisions.Contains(col))
		{
			current_collisions.Remove(col);
		}
	}

	public void Start()
	{
		base.Start();
		equipment = new EquipmentManager(gameObject);
		if (Application.loadedLevelName == "TileEditor")
		{
			GetComponent<Rigidbody2D>().isKinematic = true;
		}
		move_speed_mut = move_speed_base;
		jump_speed_mut = jump_speed_base;
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

	private void checkCollisions()
	{
		if (accurate_check)
		{
			grounded = false;
			Collider2D[] gL = Physics2D.OverlapCircleAll(ground_check_left.position, check_radius, foreground);
			foreach (Collider2D c in gL)
			{
				if (c.tag == "Ground" || c.tag == "Ground")
				{
					grounded = true;
				}
			}
			Collider2D[] gR = Physics2D.OverlapCircleAll(ground_check_right.position, check_radius, foreground);
			foreach (Collider2D c in gR)
			{
				if (c.tag == "Ground" || c.tag == "Ground")
				{
					grounded = true;
				}
			}

			Collider2D wT = Physics2D.OverlapCircle(wall_check_top.position, check_radius, foreground);
			Collider2D wB = Physics2D.OverlapCircle(wall_check_bottom.position, check_radius, foreground);
			front_contact = (wT != null && wT.tag == "Ground") || (wB != null && wB.tag == "Ground");
			Collider2D wBa = Physics2D.OverlapCircle(wall_check_back.position, check_radius, foreground);
			back_contact = wBa != null && wBa.tag == "Ground";
		}
		else
		{
		}
		if (GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].tiles[(int)transform.position.x][(int)(GetComponent<Collider2D>().bounds.min.y)].active)
		{
			GetComponent<Rigidbody2D>().gravityScale = 0;
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
		}

	}
	
	private void manageGravity()
	{
		if (on_ladder)
		{
			GetComponent<Rigidbody2D>().gravityScale = 0;
		}
		else if (time_zone_contact)
		{

		}
		else
		{
			GetComponent<Rigidbody2D>().gravityScale = grav_scale;
		}
	}

	// Normal Movement
	private void Movement()
	{
		if (updraft_contact || parachute_use || on_ladder){return;}

		if (IN_JUMP)
		{
			IN_JUMP = false;
			if (grounded)
			{
				GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jump_speed_mut);
			}
		}
		if (IN_LEFT)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(-move_speed_mut, GetComponent<Rigidbody2D>().velocity.y);
		}
		if (IN_RIGHT)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(move_speed_mut, GetComponent<Rigidbody2D>().velocity.y);
		}

		if (!IN_JUMP && !IN_LEFT && !IN_RIGHT && !IN_UP && !IN_DOWN && !IN_ATTACK)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
		}
	}

	public void Equip(GameObject equipment)
	{
		equipment.transform.parent = transform.FindChild("Equipment");
	}

	public override void NormalUpdate()
	{
		base.NormalUpdate();
		checkCollisions();
		manageGravity();
		Movement();
	}

}

using UnityEngine;
using System.Collections;

//Function will be moved to Controllable
public class Ladder : MonoBehaviour
{
	public bool top;

	void OnTriggerStay2D(Collider2D col)
	{
		if (col.GetComponent<Mobile>() != null)
		{
			if (top)
			{
				GetOffLadder(col.GetComponent<Mobile>());
			}
			else
			{
				GetOnLadder(col.GetComponent<Mobile>());
				Movement(col.GetComponent<Mobile>());
			}
		}
	}

	private void GetOnLadder(Mobile mob)
	{
		if (!mob.on_ladder)
		{
			if (mob.IN_UP && !mob.parachute_use)
			{
				mob.on_ladder = true;
				mob.rigidbody2D.gravityScale = 0;
			}
		}
	}

	private void GetOffLadder(Mobile mob)
	{
		mob.on_ladder = false;
		mob.rigidbody2D.gravityScale = mob.grav_scale;
	}

	private void Movement(Mobile mob)
	{
		if (!mob.on_ladder){return;}

		mob.transform.position = new Vector3(transform.position.x + 0.5f, mob.transform.position.y, mob.transform.position.z);
		if (mob.IN_UP)
		{
			mob.rigidbody2D.velocity = new Vector2(0,mob.move_speed/1.5f);
		}
		else if (mob.IN_DOWN)
		{
			mob.rigidbody2D.velocity = new Vector2(0,-mob.move_speed/1.5f);
		}
		else if (mob.IN_JUMP && mob.IN_LEFT)
		{
			mob.IN_JUMP = false;
			mob.on_ladder = false;
			mob.rigidbody2D.gravityScale = mob.grav_scale;
			mob.rigidbody2D.velocity = new Vector2(-mob.jump_speed,mob.jump_speed/2);
		}
		else if (mob.IN_JUMP && mob.IN_RIGHT)
		{
			mob.IN_JUMP = false;
			mob.on_ladder = false;
			mob.rigidbody2D.gravityScale = mob.grav_scale;
			mob.rigidbody2D.velocity = new Vector2(mob.jump_speed,mob.jump_speed/2);
		}
		else
		{
			mob.rigidbody2D.velocity = new Vector2(0, 0);
		}
	}
}


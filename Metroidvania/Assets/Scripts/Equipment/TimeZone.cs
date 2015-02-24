using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeZone : MonoBehaviour
{
	List<Collider2D> collisions = new List<Collider2D>();

	void Start()
	{
		Invoke("DestroyThis", 3);
	}
	

	void OnTriggerStay2D(Collider2D col)
	{
		if (col.GetComponent<Mobile>() != null)
		{
			col.GetComponent<Mobile>().time_zone_contact = true;
			Movement(col.GetComponent<Mobile>());
			if (!collisions.Contains(col))
			{
				col.GetComponent<Mobile>().rigidbody2D.velocity =
					new Vector2(col.GetComponent<Mobile>().rigidbody2D.velocity.x, col.GetComponent<Mobile>().rigidbody2D.velocity.y/2);
				collisions.Add(col);
			}
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.GetComponent<Mobile>() != null)
		{
			col.GetComponent<Mobile>().time_zone_contact = false;
			if (collisions.Contains(col))
			{
				collisions.Remove(col);
			}
		}
	}

	void DestroyThis()
	{
		foreach (Collider2D c in collisions)
		{
			c.GetComponent<Mobile>().time_zone_contact = false;
		}
		Destroy(gameObject);
	}

	void Movement(Mobile mob)
	{
		if (mob.IN_JUMP)
		{
			mob.IN_JUMP = false;
			if (mob.grounded)
			{
				if (mob.rigidbody2D.velocity.y < mob.jump_speed/5)
				{
					mob.rigidbody2D.velocity = new Vector2(mob.rigidbody2D.velocity.x, mob.jump_speed/5);
				}
			}
		}
		if (mob.IN_LEFT)
		{
			mob.rigidbody2D.velocity = new Vector2(-mob.move_speed/5, mob.rigidbody2D.velocity.y);
		}
		if (mob.IN_RIGHT)
		{
			mob.rigidbody2D.velocity = new Vector2(mob.move_speed/5, mob.rigidbody2D.velocity.y);
		}
		
		if (!mob.IN_JUMP && !mob.IN_LEFT && !mob.IN_RIGHT && !mob.IN_UP && !mob.IN_DOWN && !mob.IN_ATTACK)
		{
			mob.rigidbody2D.velocity = new Vector2(0, mob.rigidbody2D.velocity.y);
		}
	}
}

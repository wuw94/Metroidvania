using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeZone : MonoBehaviour
{
	public List<GameObject> collisions = new List<GameObject>();
	float ratio = 0.5f;

	void Start()
	{
		Invoke("DestroyThis", 20);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.GetComponent<Mobile>() != null)
		{
			if (!collisions.Contains(col.gameObject))
			{
				Enter(col.GetComponent<Mobile>());
				collisions.Add(col.gameObject);
			}
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.GetComponent<Mobile>() != null)
		{
			if (collisions.Contains(col.gameObject))
			{
				Leave(col.GetComponent<Mobile>());
				collisions.Remove(col.gameObject);
			}
		}
	}

	void DestroyThis()
	{
		foreach (GameObject c in collisions)
		{
			Leave(c.GetComponent<Mobile>());
		}
		Destroy(gameObject);
	}

	void Enter(Mobile mob)
	{
		mob.time_zone_contact = true;
		mob.GetComponent<Rigidbody2D>().velocity = new Vector2(mob.GetComponent<Rigidbody2D>().velocity.x * ratio, mob.GetComponent<Rigidbody2D>().velocity.y * ratio);
		mob.GetComponent<Rigidbody2D>().gravityScale = mob.grav_scale * Mathf.Pow(ratio, 2);
		mob.move_speed_mut = mob.move_speed_base * ratio;
		mob.jump_speed_mut = mob.jump_speed_base * ratio;
	}

	void Leave(Mobile mob)
	{
		mob.time_zone_contact = false;
		mob.GetComponent<Rigidbody2D>().velocity = new Vector2(mob.GetComponent<Rigidbody2D>().velocity.x / ratio, mob.GetComponent<Rigidbody2D>().velocity.y / ratio);
		mob.GetComponent<Rigidbody2D>().gravityScale = mob.grav_scale;
		mob.move_speed_mut = mob.move_speed_base;
		mob.jump_speed_mut = mob.jump_speed_base;
	}


}

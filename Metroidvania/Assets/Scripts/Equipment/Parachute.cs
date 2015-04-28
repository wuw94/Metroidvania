using UnityEngine;
using System.Collections;

public class Parachute : Equipment
{
	public Sprite open;
	public Sprite closed;

	void Start()
	{
		transform.localPosition = new Vector3(0,0,0);
	}
	
	void Update()
	{
		GetComponent<SpriteRenderer>().sprite = GetHolder().parachute_use ? open : closed;
		Movement(GetHolder());
	}

	void Movement(Mobile mob)
	{
		mob.parachute_use = Input.GetKey(KeyCode.A) && !mob.grounded && !mob.on_ladder && mob.equipment.Contains(EquipmentType.Parachute);
		if (!mob.parachute_use){return;}

		if (mob.IN_LEFT)
		{
			mob.GetComponent<Rigidbody2D>().velocity = new Vector2(-mob.move_speed_mut, mob.GetComponent<Rigidbody2D>().velocity.y + ((mob.GetComponent<Rigidbody2D>().velocity.y < -1)?1:0));
		}
		else if (mob.IN_RIGHT)
		{
			mob.GetComponent<Rigidbody2D>().velocity = new Vector2(mob.move_speed_mut, mob.GetComponent<Rigidbody2D>().velocity.y + ((mob.GetComponent<Rigidbody2D>().velocity.y < -1)?1:0));
		}
		else
		{
			mob.GetComponent<Rigidbody2D>().velocity = new Vector2(0, mob.GetComponent<Rigidbody2D>().velocity.y + ((mob.GetComponent<Rigidbody2D>().velocity.y < -1)?1:0));
		}
	}
}

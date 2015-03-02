using UnityEngine;
using System.Collections;

//Function will be moved to Controllable
<<<<<<< HEAD
public class Ladder : MonoBehaviour //Controllable
{/*
	public bool on_ladder;
	public GameObject Player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Player near ladder orr Player on ladder)
		{
			Touch_the_ladder();
			Ladder_controls();
		}
		if(Player Attribute the endd offff Ladder)
		{
			At_the_end_of_ladder();
=======
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
>>>>>>> d9133f67934f507950d535b8c1f0bdc2893eb4ed
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
				mob.transform.position = new Vector2(transform.position.x + 0.5f, mob.rigidbody2D.position.y);
				
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
			

		if (mob.IN_UP)
		{
			mob.rigidbody2D.velocity = new Vector2(0,mob.move_speed_mut/1.5f);
		}
		else if (mob.IN_DOWN)
		{
			mob.rigidbody2D.velocity = new Vector2(0,-mob.move_speed_mut/1.5f);
		}
		else if (mob.IN_JUMP && mob.IN_LEFT)
		{
			mob.IN_JUMP = false;
			GetOffLadder(mob);
			mob.rigidbody2D.velocity = new Vector2(mob.jump_speed_mut,mob.jump_speed_mut/2);
		}
		else if (mob.IN_JUMP && mob.IN_RIGHT)
		{
			mob.IN_JUMP = false;
			GetOffLadder(mob);
			mob.rigidbody2D.velocity = new Vector2(mob.jump_speed_mut,mob.jump_speed_mut/2);
		}
		else
		{
			mob.rigidbody2D.velocity = new Vector2(0, 0);
		}
<<<<<<< HEAD
	}
*/}
=======

		if (!mob.IN_UP && mob.grounded)
		{
			GetOffLadder(mob);
		}
	}
}
>>>>>>> d9133f67934f507950d535b8c1f0bdc2893eb4ed

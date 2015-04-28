using UnityEngine;
using System.Collections;

/*
 *
 * Recordable ->  Mobile -> Controllable -> Player
 * 			  \		   	 -> "AI"		 
 * 			   -> Immobile 
 * 
 */

public class AI : Mobile
{
	public Player player;
	public bool playerIsSighted = false;	// True if player gets too close to enemy
	private float enemyDistance;	// Get the x distance between the enemy and the player
	private float initialPosition;	// Get the enemy's position to return to when losing sight of the player

	// sightStart - where we want the alien to begin looking
	// sightEnd - where its sight ends
	public Transform sightStart, sightEnd;
	
	// collision - if colliding with block occurs
	private bool collision = false;

	void Start()
	{
		// Get this gameObject's initial rigidbody position when starting up game
		initialPosition = GetComponent<Rigidbody2D>().position.x;
	}

	// Update is called once per frame
	void Update () 
	{
		// Once player rewinds, enemy returns to its original position
		// and stops chasing the player
		if (playerIsSighted && (operation_mode == 2))
		{
			playerIsSighted = false;
		}

		collision = Physics2D.Linecast (sightStart.position, sightEnd.position, 1 << LayerMask.NameToLayer ("Block"));
		Debug.DrawLine (sightStart.position, sightEnd.position, Color.green);

		if (collision)
			IN_JUMP = true;
		if (GetComponent<Rigidbody2D>().velocity.x < 0)
			transform.localScale = new Vector3 (-1, 1, 1);
		else
			transform.localScale = new Vector3 (1, 1, 1);
		
		if (operation_mode == 1)
		{
			// Calculate the x distance between the enemy and the player
			enemyDistance = Mathf.Abs(player.GetComponent<Rigidbody2D>().position.x) - Mathf.Abs(this.gameObject.GetComponent<Rigidbody2D>().position.x);
			enemyDistance = Mathf.Abs(enemyDistance);

			if (enemyDistance > 30f) 
			{
				playerIsSighted = false;
			}

			if (playerIsSighted) 
			{
				ChasePlayer();
			}
			else 
			{
				if (GetComponent<Rigidbody2D>().position.x <= initialPosition + .1f && GetComponent<Rigidbody2D>().position.x >= initialPosition - .1f)
				{
					StopMoving();
				}
				else if (GetComponent<Rigidbody2D>().position.x <= initialPosition)
				{
					IN_RIGHT = true;
				}
				else if (GetComponent<Rigidbody2D>().position.x > initialPosition)
				{
					IN_LEFT = true;
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		if (operation_mode == 1)
		{
			if (c.gameObject.tag == "Player") 
			{
				playerIsSighted = true;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D c)
	{
		if (operation_mode == 1)
		{
			if (c.gameObject.tag == "Player") 
			{
				BreakRecordingMode();
			}
		}
	}

	void ChasePlayer()
	{
		if (GetComponent<Rigidbody2D>().position.x <= player.GetComponent<Rigidbody2D>().position.x) 
		{
			IN_RIGHT = true;
		}
		else //if (this.gameObject.rigidbody2D.position.x > player.rigidbody2D.position.x)
		{
			IN_LEFT = true;
		}
	}

	// Hugger breaks recording mode if it grabs the player
	void BreakRecordingMode()
	{
		operation_mode = 0;
	}

	void StopMoving()
	{
		GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
	}
}

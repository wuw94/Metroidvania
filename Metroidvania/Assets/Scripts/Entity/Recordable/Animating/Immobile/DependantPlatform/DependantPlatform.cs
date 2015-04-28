using UnityEngine;
using System.Collections;

/* DependantPlatform.
 * Give this DependantPlatform a lever whose event_state will affect this platform's behavior
 * 
 * 
 * Here are the behaviors based on given int:
 * 0	Collisions off when lever's event_state is 0
 * 		Collisions on when lever's event_state is 1
 * 1	Collisions on when lever's event_state is 1
 * 		Collisions off when lever's event_state is 0
 * 
 */

public class DependantPlatform : Immobile
{
	public bool start;

	void Start()
	{
		this_info.facingRight = true;
		transform.GetComponent<Collider2D>().enabled = start;
		GetComponent<Renderer>().enabled = start;
	}
	
	void Update()
	{
		if (Application.loadedLevelName == "TileEditor")
		{
			transform.GetComponent<Collider2D>().enabled = true;
		}
	}

	public void changeCollisions()
	{
		bool c = transform.GetComponent<Collider2D>().enabled;
		transform.GetComponent<Collider2D>().enabled = !c;
		GetComponent<Renderer>().enabled = !c;
	}

}

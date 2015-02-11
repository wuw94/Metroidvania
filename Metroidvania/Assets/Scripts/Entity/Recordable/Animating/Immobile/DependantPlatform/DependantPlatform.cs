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
		transform.collider2D.enabled = start;
		renderer.enabled = start;
	}
	
	void Update()
	{
		if (Application.loadedLevelName == "TileEditor")
		{
			transform.collider2D.enabled = true;
		}
	}

	public void changeCollisions()
	{
		bool c = transform.collider2D.enabled;
		transform.collider2D.enabled = !c;
		renderer.enabled = !c;
	}

}

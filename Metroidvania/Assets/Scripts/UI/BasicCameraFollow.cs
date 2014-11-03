using UnityEngine;
using System.Collections;

/* Basic Camera Follow. So smooth
 * Makes the camera move quickly toward the linked variable 'follow', but slowing near the edge
 * -camera.fieldOfView- to control how much the camera shows
 * -speed- to control speed of camera movement
 * 
 * auth Wesley Wu
 */

public class BasicCameraFollow : MonoBehaviour {
	public GameObject follow;
	private int speed = 1;
	private float distanceX;
	private float distanceY;

	// Use this for initialization
	void Start ()
	{
		camera.fieldOfView = 110;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		//Calculate distance between camera and object XY
		distanceX = (follow.transform.position.x - transform.position.x) / speed;
		distanceY = (follow.transform.position.y - transform.position.y) / speed;

		//Smooth moves
		if (Mathf.Abs(distanceX) > 0.001f || Mathf.Abs(distanceY) > 0.001f)
		{
			transform.position = new Vector3(transform.position.x + distanceX / 10, transform.position.y + distanceY / 10, -10);
		}
	}
}

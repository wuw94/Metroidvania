using UnityEngine;
using System.Collections;

/* Lerp Follow.
 * Makes the camera move quickly toward the linked variable 'follow', but slowing near the edge
 * 
 * Change what the camera is following from outside, using:
 * LerpFollow.follow
 * 
 * auth Wesley Wu
 */

public class LerpFollow : MonoBehaviour {
	public static GameObject follow;
	public GameObject player;
	private int speed = 5;

	public void returnToPlayer()
	{
		follow = player;
	}

	void Start()
	{
		returnToPlayer();
		camera.fieldOfView = 110;
	}

	void FixedUpdate()
	{
		transform.position = new Vector3(Mathf.Lerp(transform.position.x, follow.transform.position.x, speed * Time.deltaTime),
		                                 Mathf.Lerp(transform.position.y, follow.transform.position.y, speed * Time.deltaTime),
		                                 transform.position.z);
	}
}

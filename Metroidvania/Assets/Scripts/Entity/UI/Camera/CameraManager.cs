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

public class CameraManager : MonoBehaviour
{
	// Lerp variables
	public GameObject follow;
	public Player player;
	private float speed = 5;

	// Shake variables
	public float shake_time = 0;


	public void returnToPlayer()
	{
		follow = player.gameObject;
	}

	void Start()
	{
		returnToPlayer();
		camera.fieldOfView = 110;
	}

	void FixedUpdate()
	{
		detectPlayer();
		manageLerp();
		manageShake();
	}

	private void detectPlayer()
	{
		if (player == null)
		{
			player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		}
		returnToPlayer();
	}

	private void manageLerp()
	{
		transform.position = new Vector3(Mathf.Lerp(transform.position.x, follow.transform.position.x, speed),
		                                 Mathf.Lerp(transform.position.y, follow.transform.position.y, speed),
		                                 transform.position.z);
	}

	private void manageShake()
	{
		if (shake_time > 0)
		{
			shake_time -= Time.deltaTime;
			Vector2 shake = Random.insideUnitCircle * shake_time;
			Camera.main.transform.position += new Vector3(shake.x, shake.y, 0);
		}
		else if (shake_time < 0)
		{
			shake_time = 0;
		}
	}
}

using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
	public float Character_Health;
	public float ticking_time;
	public float power = 20.0f;
	public float radius = 5.0f;
	public Collider physics_detection;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
				Vector2 explosion_position = transform.position;
				ticking_time -= Time.deltaTime * 10f;
				//Collider[] colliding_items = Physics.CheckSphere(explosionPos, radius);
				if (ticking_time == 0)
					physics_detection.rigidbody.AddExplosionForce (power, explosion_position, radius, 1.0F);
					Character_Health = Character_Health - power;

						}
		}

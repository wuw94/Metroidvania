using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
	/// <summary>
	/// Define what kind of data will be consumed when this Item is obtained.
	/// </summary>
	public Equipment info;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			other.GetComponent<Player>().equipment.Add(info);
			other.GetComponent<Player>().current_collisions.Remove(gameObject.collider2D);
			Destroy(gameObject);
		}
	}
}

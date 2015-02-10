using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
	public Equipment info;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			other.GetComponent<Player>().equipment.Add(info);
			Destroy(gameObject);
		}
	}
}

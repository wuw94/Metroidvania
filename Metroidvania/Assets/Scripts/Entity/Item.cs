using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
	/// <summary>
	/// Define what kind of data will be consumed when this Item is obtained.
	/// </summary>
	public string source;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			other.GetComponent<Player>().current_collisions.Remove(gameObject.GetComponent<Collider2D>());
			GameObject eq = (GameObject)Instantiate(Resources.Load(source));
			other.GetComponent<Player>().Equip(eq);
			Destroy(gameObject);
		}
	}
}

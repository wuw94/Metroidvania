using UnityEngine;
using System.Collections;

public class Explosion : Immobile
{
	public int power = 50;

	void OnTriggerStay2D(Collider2D col)
	{
		if (col.gameObject.GetComponent<Recordable>() != null)
		{
			col.gameObject.GetComponent<Recordable>().Damage(power * (GetComponent<CircleCollider2D>().radius - Vector2.Distance(col.gameObject.transform.position, transform.position)));
		}
	}
}

using UnityEngine;
using System.Collections;

public class Parachute : Equipment
{
	public Sprite open;
	public Sprite closed;
	
	void Start()
	{
		transform.localPosition = new Vector3(0,0,0);
	}
	
	void Update()
	{
		GetComponent<SpriteRenderer>().sprite = GetHolder().parachute_use ? open : closed;
	}
}

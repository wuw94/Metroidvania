using UnityEngine;
using System.Collections;

public class Integrator : Equipment
{
	public GameObject time_zone;

	void Start()
	{
		transform.localPosition = new Vector3(0,0,0);
	}
	

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.S))
		{
			Instantiate(time_zone,transform.position, transform.rotation);
		}
	}
}

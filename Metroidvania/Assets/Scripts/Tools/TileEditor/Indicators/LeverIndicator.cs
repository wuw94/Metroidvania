using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeverIndicator : Indicator
{
	public List<GameObject> affecting = new List<GameObject>();


	void Start()
	{
	}
	
	void Update ()
	{
		base.Update();
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		if (displaying)
		{
			foreach (GameObject g in affecting)
			{
				Gizmos.DrawLine(transform.position + new Vector3(0.5f,0.5f,0), g.transform.position + new Vector3(0.5f,0.5f,0));
			}
		}
	}
}

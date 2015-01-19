using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeverIndicator : Indicator
{
	public List<GameObject> affecting = new List<GameObject>();
	public bool displaying = false;

	void Start()
	{
	}
	
	void Update ()
	{
		if (Camera.main.GetComponent<TileEditor>().selection != null && Camera.main.GetComponent<TileEditor>().selection.Equals(gameObject))
		{
			displaying = true;
		}
		else
		{
			displaying = false;
		}
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

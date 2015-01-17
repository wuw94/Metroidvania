using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeverIndicator : Indicator
{
	public LeverInfo info = new LeverInfo();
	public TileEditor te;

	void Start()
	{
		te = Camera.main.GetComponent<TileEditor>();
	}
	
	void Update () {
	}

	void OnMouseDown()
	{
		display_info = true;
	}
	
	void OnDrawGizmos()
	{
		for (int i = 0; i < info.affecting.Count; i++)
		{
			Gizmos.DrawLine(((List<GameObject>)te.indicators[EntityTypes.Interactive])[info.self].transform.position,
			                ((List<GameObject>)te.indicators[EntityTypes.Interactive])[info.affecting[i]].transform.position);
		}
	}
}

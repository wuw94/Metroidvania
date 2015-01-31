using UnityEngine;
using System.Collections;

public class Indicator : MonoBehaviour
{
	public bool display_info = false;
	public bool displaying = false;

	public void Update()
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
}

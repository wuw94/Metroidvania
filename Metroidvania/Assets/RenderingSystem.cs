using UnityEngine;
using System.Collections;

public class RenderingSystem : GlobalVariables {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A))
		{
			camera.cullingMask = 1<<0 | 1<<1 | 1<<2 | 1<<4 | 1<<5 | 1<<9;
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			camera.cullingMask = 1<<0 | 1<<1 | 1<<2 | 1<<4 | 1<<5 | 1<<8;
		}
	}
}

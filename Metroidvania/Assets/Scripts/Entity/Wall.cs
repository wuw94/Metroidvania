using UnityEngine;
using System.Collections;

public class Wall : Immobile {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Action ();
	}

	public override void Action()
	{
		Destroy(gameObject);
	}
}

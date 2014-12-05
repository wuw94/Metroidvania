using UnityEngine;
using System.Collections;

public class Wall : Immobile {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public override void Damage(float amount)
	{
		Destroy(gameObject);
	}
}

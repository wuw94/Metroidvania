using UnityEngine;
using System.Collections;

public class test : Immobile
{

	public override void NormalUpdate()
	{
		base.NormalUpdate();

		transform.position = new Vector3(transform.position.x + 0.04f,
		                                 transform.position.y,
		                                 transform.position.z);

	}

}

using UnityEngine;
using System.Collections;

public class test : Immobile
{

	public override void NormalUpdate()
	{
		base.NormalUpdate();

		transform.position = new Vector3(transform.position.x + 0.02f,
		                                 transform.position.y,
		                                 transform.position.z);

	}

	public override void RecordAct()
	{
		base.RecordAct();

		transform.position = new Vector3(transform.position.x + 0.02f,
		                                 transform.position.y,
		                                 transform.position.z);

	}
}

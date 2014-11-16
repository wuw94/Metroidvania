using UnityEngine;
using System.Collections;

public class test : Controllable
{

	public override void NormalUpdate()
	{
		transform.position = new Vector3(transform.position.x + 0.02f,
		                                 transform.position.y,
		                                 transform.position.z);
	}

	public override void RecordAct()
	{
		transform.position = new Vector3(transform.position.x + 0.02f,
		                                  transform.position.y,
		                                  transform.position.z);
	}
}

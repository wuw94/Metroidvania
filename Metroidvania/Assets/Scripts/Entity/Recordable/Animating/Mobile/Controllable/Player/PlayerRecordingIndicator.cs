using UnityEngine;
using System.Collections;

public class PlayerRecordingIndicator : Recordable
{
	void Start()
	{
		GetComponent<Renderer>().material.color = new Vector4(GetComponent<Renderer>().material.color.r,
		                                      GetComponent<Renderer>().material.color.g,
		                                      GetComponent<Renderer>().material.color.b,
		                                      0.5f);
	}

	public override void NormalUpdate()
	{
	}

	public override void Record()
	{
	}

	public override void Rewind()
	{
	}

	public override void Playback()
	{
		Destroy(gameObject);
	}


}

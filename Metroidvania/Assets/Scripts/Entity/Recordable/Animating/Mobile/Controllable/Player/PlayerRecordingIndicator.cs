using UnityEngine;
using System.Collections;

public class PlayerRecordingIndicator : Recordable
{
	void Start()
	{
		renderer.material.color = new Vector4(renderer.material.color.r,
		                                      renderer.material.color.g,
		                                      renderer.material.color.b,
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

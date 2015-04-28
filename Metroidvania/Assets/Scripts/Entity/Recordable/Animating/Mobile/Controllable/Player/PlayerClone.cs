using UnityEngine;
using System.Collections;

/* Player.
 * Always put this on the player prefab, but make sure there is only one on the scene at any given time.
 * Not to be put on the clone of player. There's another script for that
 * 
 */

public class PlayerClone : Recordable
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
		Destroy(gameObject);
	}
	
	public override void Record()
	{
	}

	public override void Rewind()
	{
	}
	
	public override void Playback()
	{
		//base.Playback();
	}
	
	
	
	
}

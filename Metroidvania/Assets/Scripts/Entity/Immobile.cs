using UnityEngine;
using System.Collections;

/* Immobile.
 * Objects that inherit from the immobile class are able to:
 * -Nothing yet
 * 
 * Note:
 * Later on maybe this class will contain be inherited by scripted events, like levers
 */

public class Immobile : Recordable
{
	public override void NormalUpdate()
	{
	}
	
	public override void Record()
	{
		recordInfo();
	}

	public override void RecordAct()
	{
		recordInfo();
	}
	
	public override void Rewind()
	{
		readInfo();
	}
	
	public override void Playback()
	{
		readInfo();
	}
}

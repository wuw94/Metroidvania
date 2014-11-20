using UnityEngine;
using System.Collections;

public struct RecordInfo
{
	public float posX;
	public float posY;
	public int animState;
	public int eventState;

	public RecordInfo(float pX, float pY, int aS, int eS)
	{
		posX = pX;
		posY = pY;
		animState = aS;
		eventState = eS;
	}
}

using UnityEngine;
using System.Collections;

public struct RecordInfo
{
	public float posX;
	public float posY;
	public int animState;
	public int eventState;
	public bool facingRight;

	public RecordInfo(float pX, float pY, int aS, int eS, bool fR)
	{
		posX = pX;
		posY = pY;
		animState = aS;
		eventState = eS;
		facingRight = fR;
	}
}

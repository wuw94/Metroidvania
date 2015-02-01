using UnityEngine;
using System.Collections;

public struct RecordInfo
{
	public float posX;
	public float posY;
	public byte animState;
	public byte eventState;
	public bool facingRight;

	public RecordInfo(float pX, float pY, byte aS, byte eS, bool fR)
	{
		posX = pX;
		posY = pY;
		animState = aS;
		eventState = eS;
		facingRight = fR;
	}
}

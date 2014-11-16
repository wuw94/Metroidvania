using UnityEngine;
using System.Collections;

public struct RecordInfo
{
	public int prefabN;
	public int prefabI;
	public float posX;
	public float posY;
	public int animState;
	public int eventState;

	public RecordInfo(int pN, int pI, float pX, float pY, int aS, int eS)
	{
		prefabN = pN;
		prefabI = pI;
		posX = pX;
		posY = pY;
		animState = aS;
		eventState = eS;
	}
}

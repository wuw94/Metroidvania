using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct LeverInfo
{
	public PseudoVector2 self;
	public List<int> affecting;

	public LeverInfo(PseudoVector2 s, List<int> a)
	{
		self = s;
		affecting = a;
	}
}

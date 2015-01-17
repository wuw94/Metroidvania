using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct LeverInfo
{
	public int self;
	public List<int> affecting;

	public LeverInfo(int s, List<int> a)
	{
		self = s;
		affecting = a;
	}
}

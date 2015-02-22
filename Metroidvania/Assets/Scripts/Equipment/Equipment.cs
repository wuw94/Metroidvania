using UnityEngine;
using System.Collections;

public abstract class Equipment : MonoBehaviour
{
	public Mobile GetHolder()
	{
		return transform.parent.parent.GetComponent<Mobile>();
	}
}

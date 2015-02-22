using UnityEngine;
using System.Collections;

public class EquipmentManager
{
	GameObject go_reference;

	public EquipmentManager(GameObject go)
	{
		go_reference = go;
	}
	public bool Contains(EquipmentType eq)
	{
		return go_reference.transform.FindChild("Equipment").FindChild(eq.ToString() + "(Clone)") != null;
	}
}

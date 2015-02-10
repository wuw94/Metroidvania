using UnityEngine;
using System;
using System.Collections;

[System.Serializable]
public struct FieldInformation
{
	public string declaring_type;
	public string field_name;
	public object field_value;
	public FieldInformation(string dt, string fn, object fv)
	{
		declaring_type = dt;
		field_name = fn;
		field_value = fv;
	}
}
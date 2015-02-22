using UnityEngine;
using System.Collections;

public class PointLight : MonoBehaviour
{
	public float range;
	public float intensity;
	public PseudoColor color = Color.white;

	void Start()
	{
		GetComponent<Light>().range = range;
		GetComponent<Light>().intensity = intensity;
		GetComponent<Light>().color = color;
	}

	void Update()
	{
		range = GetComponent<Light>().range;
		intensity = GetComponent<Light>().intensity;
		color = GetComponent<Light>().color;
	}
}

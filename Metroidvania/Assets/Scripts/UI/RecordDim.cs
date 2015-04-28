using UnityEngine;
using System.Collections;

public class RecordDim : MonoBehaviour {

	void Update()
	{
		if (Recordable.dim)
		{
			GetComponent<Renderer>().material.color = new Color(0.2f,0.2f,0.2f,GetComponent<Renderer>().material.color.a);
		}
		else if (Mathf.Abs(GetComponent<Renderer>().material.color.r - (Time.timeScale)) > 0.01f)
		{
			GetComponent<Renderer>().material.color = new Color(Mathf.Lerp(GetComponent<Renderer>().material.color.r, Time.timeScale, Time.deltaTime * 4),
			                                    Mathf.Lerp(GetComponent<Renderer>().material.color.g, Time.timeScale, Time.deltaTime * 8),
			                                    Mathf.Lerp(GetComponent<Renderer>().material.color.b, Time.timeScale, Time.deltaTime * 4),
			                                    GetComponent<Renderer>().material.color.a);

			/*
			renderer.material.color = new Color(Mathf.Lerp(renderer.material.color.r, Recordable.dim?0.3f:1, Time.deltaTime * 4),
			                                    Mathf.Lerp(renderer.material.color.g, Recordable.dim?0.3f:1, Time.deltaTime * 8),
			                                    Mathf.Lerp(renderer.material.color.b, Recordable.dim?0.3f:1, Time.deltaTime * 4),
			                                    renderer.material.color.a);
            */
		}
	}
}

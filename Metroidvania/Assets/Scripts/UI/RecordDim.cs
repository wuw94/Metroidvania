using UnityEngine;
using System.Collections;

public class RecordDim : MonoBehaviour {

	void Update()
	{
		if (Mathf.Abs(renderer.material.color.r - (Time.timeScale)) > 0.01f)
		{
			renderer.material.color = new Color(Mathf.Lerp(renderer.material.color.r, Time.timeScale, Time.deltaTime * 4),
			                                    Mathf.Lerp(renderer.material.color.g, Time.timeScale, Time.deltaTime * 8),
			                                    Mathf.Lerp(renderer.material.color.b, Time.timeScale, Time.deltaTime * 4),
			                                    renderer.material.color.a);

			/*
			renderer.material.color = new Color(Mathf.Lerp(renderer.material.color.r, Recordable.dim?0.3f:1, Time.deltaTime * 4),
			                                    Mathf.Lerp(renderer.material.color.g, Recordable.dim?0.3f:1, Time.deltaTime * 8),
			                                    Mathf.Lerp(renderer.material.color.b, Recordable.dim?0.3f:1, Time.deltaTime * 4),
			                                    renderer.material.color.a);
            */
		}
	}
}

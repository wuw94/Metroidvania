using UnityEngine;
using System.Collections;

public class Atmosphere : MonoBehaviour
{
	public Color color;
	[Range(0,1)]
	public float time;

	public bool down;

	void Update ()
	{
		transform.FindChild("Sky").renderer.material.color = new Color(0.6f + time/10 - time/1.2f, 0.7f - time/10 - time/1.2f, 0.9f - time/1.2f);

		transform.FindChild("Sun").localPosition = new Vector3(-1,5-15*time,transform.FindChild("Sun").localPosition.z);
		//transform.FindChild("Sun").GetComponent<Light>().intensity = 8-8*time;
		transform.FindChild("Sun").GetComponent<Light>().color = new Color(1,1-time,0.2f, 1-time);

		transform.FindChild("Lighting").GetComponent<Light>().intensity = 0.7f-0.7f*time;
		transform.FindChild("Lighting").GetComponent<Light>().color = new Color(1,1-time,0.2f, 1-time);

		if (time > 0.5f)
		{
			time += down ? (1-time)/100 + 0.0001f : -(1-time)/100 - 0.0001f;
		}
		else
		{
			time += down ? time/100 + 0.0001f : -time/100 - 0.0001f;
		}
		if (time <= 0.01f && !down)
		{
			down = true;
		}
		if (time >= 0.99f && down)
		{
			down = false;
		}
	}


}

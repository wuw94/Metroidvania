using UnityEngine;
using System.Collections;

public class test : MonoBehaviour
{
	void Update()
	{
		transform.localScale = new Vector3(Camera.main.GetComponent<AudioManager>().rmsValue*5,
		                                   Camera.main.GetComponent<AudioManager>().rmsValue*5,
		                                   transform.localScale.z);
	}
}

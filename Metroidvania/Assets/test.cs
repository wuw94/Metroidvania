using UnityEngine;
using System.Collections;

public class test : MonoBehaviour
{
	void Update()
	{
		transform.localScale = new Vector3(Camera.main.GetComponent<AudioManager>().rmsValue*100,
		                                   Camera.main.GetComponent<AudioManager>().rmsValue*100,
		                                   transform.localScale.z);
	}
}

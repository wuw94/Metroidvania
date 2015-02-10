using UnityEngine;
using System.Collections;

public class TextGeneratorEntity : MonoBehaviour {
	
	void Start () {
		Debug.Log(transform.position);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			Camera.main.GetComponent<RenderingSystem>().text_generator.AddText("Hello");
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			Camera.main.GetComponent<RenderingSystem>().text_generator.Next();
		}
	}
}

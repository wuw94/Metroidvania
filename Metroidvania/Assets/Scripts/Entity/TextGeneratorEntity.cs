using UnityEngine;
using System.Collections;

public class TextGeneratorEntity : MonoBehaviour {

	public string text;

	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			Camera.main.GetComponent<RenderingSystem>().text_generator.AddText(text);
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

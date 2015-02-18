using UnityEngine;
using System.Collections;

public class TextGeneratorEntity : MonoBehaviour {

	public string text;

	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			Camera.main.GetComponent<TextGenerator>().AddText(text);
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			Camera.main.GetComponent<TextGenerator>().Next();
		}
	}
}

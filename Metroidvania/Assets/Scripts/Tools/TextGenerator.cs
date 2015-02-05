using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Text Generator just makes single textboxes once, cannot change it
//Goal for tomorrow, figure out why must print out from OnGUI and how to manipulate that
public class TextGenerator : RenderingSystem {
	public static List<string> message_buffer = new List<string>();

	void Start()
	{
		AddText ("one");
		AddText ("asjdoajiogaioegjoaijgioeagjoaejgoieajgo");
		AddText ("ajasjdoajiogaioegjoaijgioeagjoaejgoieajgoasjdoajiogaioegjoaijgioeagjoaejgoieajgoasjdoajiogaioegjoaijgioeagjoaejgoieajgoasjdoajiogaioegjoaijgioeagjoaejgoieajgoasjdoajiogaioegjoaijgioeagjoaejgoieajgo");
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Next();
		}
	}

	void OnGUI () {
		if (message_buffer.Count > 0)
		{
			GUI.Label(new Rect(20,Screen.height - 100,Screen.width-20,100), message_buffer[0]);
			GUI.Label(new Rect(message_buffer[0].Length * 9, 20, 20, 20), ".");
		}
	}

	public void AddText(string text)
	{
		message_buffer.Add(text);
	}

	public void Next()
	{
		message_buffer.RemoveAt(0);
	}
}

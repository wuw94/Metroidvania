using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Text Generator just makes single textboxes once, cannot change it
//Goal for tomorrow, figure out why must print out from OnGUI and how to manipulate that
public class TextGenerator : RenderingSystem
{
	public static List<string> message_buffer = new List<string>();

	private Rect text_window = new Rect(20,Screen.height - 100,Screen.width-40,100);

	void Start()
	{
	}

	void Update()
	{
	}

	void OnGUI () {
		if (message_buffer.Count > 0)
		{
			text_window = GUI.Window(0, text_window, TextWindow, "READ THIS!!!!");
		}
	}

	void TextWindow(int windowID)
	{
		GUI.Label(new Rect(20,20,Screen.width-40,100), message_buffer[0]);
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

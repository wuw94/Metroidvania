using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
	/// <summary>
	/// The map this portal takes you to
	/// </summary>
	public string map;

	/// <summary>
	/// X Y position where you'll appear in the next map
	/// </summary>
	public PseudoVector2 arrival;

	void OnTriggerStay2D(Collider2D col)
	{
		if (col.tag == "Player" && Input.GetKey(KeyCode.UpArrow))
		{
			PlayerPrefs.SetString("Map Name", map);
			PlayerPrefs.SetFloat("Arrive X", arrival.x);
			PlayerPrefs.SetFloat("Arrive Y", arrival.y);
			Application.LoadLevel("LevelTester");
		}
	}
}

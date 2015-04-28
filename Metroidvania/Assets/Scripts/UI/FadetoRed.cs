using UnityEngine;
using System.Collections;

public class FadetoRed : MonoBehaviour
{
	void Update()
	{
		HealthCheck();
	}

	void HealthCheck()
	{
		GetComponent<GUITexture>().color = new Color(GetComponent<GUITexture>().color.r,GetComponent<GUITexture>().color.g,GetComponent<GUITexture>().color.b, (1 - (GameManager.current_game.progression.player.health/100))/2.5f);
	}
}
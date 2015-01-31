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
		guiTexture.color = new Color(guiTexture.color.r,guiTexture.color.g,guiTexture.color.b, (1 - (GameManager.current_game.progression.characters[MobileTypes.Player].health/100))/2.5f);
	}
}
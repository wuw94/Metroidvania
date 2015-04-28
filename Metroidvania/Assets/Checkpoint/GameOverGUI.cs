using UnityEngine;
using System.Collections;

public class GameOverGUI : MonoBehaviour {
	
	private GameObject gameOver;

	private RespawnSystem player;

	void Start()
	{
		gameOver = GameObject.Find("GameOverScreen");
		gameOver.SetActive (false);

		player = GameObject.Find("Player").GetComponent<RespawnSystem>();
	}

	void Update()
	{
		if (RespawnSystem.currentCheckpoint.GetComponent<Checkpoint>().playerLives == 0)
			gameOver.SetActive(true);
	}

	public void RestartLevel()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	public void ReturnToHUB()
	{
		//Application.LoadLevel("");
	}

	public void ReturnToMenu()
	{
		//Application.LoadLevel("TitleScreen");
	}
}

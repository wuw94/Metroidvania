using UnityEngine;
using System.Collections;

public class Luminary : Equipment
{
	void Start()
	{
		transform.localPosition = new Vector3(0,-0.2f,-0.5f);
	}

	void Update()
	{
		GetComponent<Light>().intensity = Mathf.Lerp(GetComponent<Light>().intensity, Random.Range(0,8),Time.deltaTime);
		if (Input.GetKey(GameManager.current_game.preferences.IN_RIGHT))
		{
			transform.rotation = new Quaternion(0,0.7f,0,0.7f);
		}
		else if (Input.GetKey(GameManager.current_game.preferences.IN_LEFT))
		{
			transform.rotation = new Quaternion(0.7f,0,-0.7f,0);
		}
		else if (Input.GetKey(GameManager.current_game.preferences.IN_UP))
		{
			transform.rotation = new Quaternion(0.5f,0.5f,0.5f,-0.5f);
		}
		else if (Input.GetKey(GameManager.current_game.preferences.IN_DOWN))
		{
			transform.rotation = new Quaternion(0.5f,-0.5f,0.5f,0.5f);
		}
	}
}

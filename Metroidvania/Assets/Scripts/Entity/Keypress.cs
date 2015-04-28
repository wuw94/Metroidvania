using UnityEngine;
using System.Collections;

public class Keypress : MonoBehaviour
{
	public string path;
	public Vector3 scale = new Vector3(1,1,1);
	public int layerbits;
	public float depth;

	public float lerp_to = 0.1f;
	
	void Start()
	{
		transform.localScale = scale;
		transform.position = new Vector3(transform.position.x, transform.position.y, depth);
		gameObject.layer = layerbits;
		try
		{
			Texture2D tex = (Texture2D)Resources.Load(path);
			GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0,0,tex.width,tex.height), new Vector2(0,0));
			transform.localScale /= 3.2f;
		}
		catch (System.NullReferenceException)
		{
			Debug.LogWarning("Invalid Path on Keypress");
		}
	}
	
	void Update()
	{
		layerbits = gameObject.layer;
		GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r,
		                                    GetComponent<Renderer>().material.color.g,
		                                    GetComponent<Renderer>().material.color.b,
		                                    Mathf.Lerp(GetComponent<Renderer>().material.color.a,lerp_to,Time.deltaTime * 10));
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Player")
		{
			lerp_to = 1;
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.tag == "Player")
		{
			lerp_to = 0.1f;
		}
	}
}
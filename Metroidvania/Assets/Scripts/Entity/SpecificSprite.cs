using UnityEngine;
using System.Collections;

public class SpecificSprite : MonoBehaviour
{
	public string path;
	public Vector3 scale = new Vector3(1,1,1);
	public int layerbits;
	public float depth;

	void Start()
	{
		transform.localScale = scale;
		transform.position = new Vector3(transform.position.x, transform.position.y, depth);
		gameObject.layer = layerbits;
		try
		{
			Texture2D tex = (Texture2D)Resources.Load(path);
			GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0,0,tex.width,tex.height), new Vector2(0,0));
		}
		catch (System.NullReferenceException)
		{
			Debug.LogWarning("Invalid Path on SpecificSprite");
		}
	}

	void Update()
	{
		layerbits = gameObject.layer;
	}
}
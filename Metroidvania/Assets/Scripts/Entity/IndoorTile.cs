using UnityEngine;
using System.Collections;

public class IndoorTile : MonoBehaviour {

	public string path;
	public Vector3 scale = new Vector3(1.0f/3.2f,1.0f/3.2f,1.0f/3.2f);
	public float OrderInLayer;
	
	void Start()
	{
		transform.localScale = scale;
		transform.position = new Vector3(transform.position.x, transform.position.y, -1);
		try
		{
			Texture2D tex = (Texture2D)Resources.Load(path);
			GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0,0,tex.width,tex.height), new Vector2(0,0));
			transform.localScale = new Vector3(1.0f/3.2f,1.0f/3.2f,1.0f/3.2f);
		}
		catch (System.NullReferenceException)
		{
			Debug.LogWarning("Invalid Path on IndoorTile");
		}
	}
	
	void Update()
	{
		OrderInLayer = renderer.sortingOrder;
	}
}

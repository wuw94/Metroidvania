using UnityEngine;
using System.Collections;

public class SpecificSprite : MonoBehaviour
{
	public string path;
	public Vector3 scale = new Vector3(1,1,1);

	void Start()
	{
		transform.localScale = scale;
		Texture2D tex = (Texture2D)Resources.Load(path);
		GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0,0,tex.width,tex.height), new Vector2(0,0));
	}
}
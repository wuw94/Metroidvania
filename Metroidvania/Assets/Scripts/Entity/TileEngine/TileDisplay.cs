using UnityEngine;
using System.Collections;

public class TileDisplay : MonoBehaviour
{
	private Texture2D tile_image;
	public int rows;
	public int columns;
	private Rect display_rect = new Rect();
	public int rotation;


	void Start()
	{
		transform.localScale = new Vector3(2/(renderer.bounds.max.x - renderer.bounds.min.x),
		                                   2/(renderer.bounds.max.y - renderer.bounds.min.y),
		                                   2);
	}
	
	public void setImage(Texture2D img, int rot)
	{
		rotation = rot;
		GetComponent<SpriteRenderer>().sprite = Sprite.Create(img,
		                                                      new Rect(0, 0, img.width,img.height),
		                                                      new Vector2(0.5f, 0.5f));
		Quaternion quat = Quaternion.identity;
		quat.eulerAngles = new Vector3(0,0,rotation);
		transform.rotation = quat;
	}


	/*
	public override void NormalUpdate()
	{
		base.NormalUpdate();

	}
	
	public override void Record()
	{
		base.Record();
	}
	
	public override void Rewind()
	{
		base.Rewind();
	}
	
	public override void Playback()
	{
		base.Playback();
	}
	*/
}

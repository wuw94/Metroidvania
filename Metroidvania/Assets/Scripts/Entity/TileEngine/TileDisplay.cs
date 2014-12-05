using UnityEngine;
using System.Collections;

public class TileDisplay : MonoBehaviour
{
	public Texture2D tile_sheet;
	public int rows;
	public int columns;
	private Rect display_rect = new Rect();
	public Vector2 frame;


	void Start()
	{
		display_rect.width = (float)tile_sheet.width / columns;
		display_rect.height = (float)tile_sheet.height / rows;
		transform.localScale = new Vector3(2/(renderer.bounds.max.x - renderer.bounds.min.x),
		                                   2/(renderer.bounds.max.y - renderer.bounds.min.y),
		                                   2);
	}

	void Update()
	{
		display_rect.x = (float)tile_sheet.width / columns * frame.x;
		display_rect.y = (float)tile_sheet.height / rows * frame.y;
		GetComponent<SpriteRenderer>().sprite = Sprite.Create(tile_sheet, display_rect, Vector2.zero);

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

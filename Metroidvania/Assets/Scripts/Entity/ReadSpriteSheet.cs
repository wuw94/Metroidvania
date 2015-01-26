using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReadSpriteSheet : Recordable
{	
	public Texture2D spriteSheet;
	public Vector2 xy;

	private bool testDebug; // Sprite sheet testing
	private ReadSpriteSheet spritesheet; /// Sprite Sheet testing
	private SpriteRenderer sprite; // Sprite Sheet testing
	private int frame; // Sprite Sheet testing

	private Sprite[] sprites = new Sprite[0];
	
	void Start()
	{
		frame = 0; // Sprite Sheet test
		spritesheet = GetComponent<ReadSpriteSheet> ();// Sprite Sheet testing
		sprite = GetComponent<SpriteRenderer> ();// Sprite Sheet testing

		//StartCoroutine (SpriteSheetTest ()); // Sprite Sheet Testing;

		MakeSprites();
	}

	private void MakeSprites()
	{
		sprites = new Sprite[(int)(xy.x * xy.y)];
		for (int i = 0; i < xy.x * xy.y; i++)
		{
			sprites[i] = MakeFrame(i);
		}
	}

	public Sprite Frame(int frame)
	{
		if (sprites.Length == 0)
		{
			MakeSprites();
		}
		int frame_number = (frame >= 0 && frame <= ((xy.x * xy.y) - 1)) ? frame : 0;
		return sprites[frame_number];
	}

	public Sprite MakeFrame(int frame)
	{
		int frame_number = (frame >= 0 && frame <= ((xy.x * xy.y) - 1)) ? frame : 0; // returns first frame if frame number to big or small;
		int row = (frame_number / (int)xy.x);
		Rect spriteRect = new Rect ();
		spriteRect.width = (spriteSheet.width / xy.x); 
		spriteRect.height = (spriteSheet.height / xy.y); 
		spriteRect.x = ((frame_number % xy.x)/xy.x) * spriteSheet.width; 
		spriteRect.y = ( ((xy.y - 1) / (xy.y)) -(row / xy.y) ) * spriteSheet.height;
		return Sprite.Create(spriteSheet,spriteRect,new Vector2(0.5f,0.5f));
	}

	void Update()
	{
		if (spriteSheet != null)
		{
			if(this_info.facingRight)
			{
				transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
			}
			else
			{
				transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
			}
			GetComponent<SpriteRenderer>().sprite = Frame(this_info.animState);
		}
	}


	IEnumerator SpriteSheetTest(){ // only for testing
		for (int x = 0; x < 20; x++) {
			
			sprite.sprite = spritesheet.Frame (frame);
			yield return new WaitForSeconds (1f);
			frame += 1;//test texture is too  small so it doesnt fill the collider. Sprite should be correct size;
		}
	}

}
using UnityEngine;
using System.Collections;

public class ReadSpriteSheet : Recordable {
	
	public Texture2D spriteSheet;
	public Vector2 xy;

	private bool testDebug; // Sprite sheet testing
	private ReadSpriteSheet spritesheet; /// Sprite Sheet testing
	private SpriteRenderer sprite; // Sprite Sheet testing
	private int frame; // Sprite Sheet testing
	
	void Start()
	{
		frame = 0; // Sprite Sheet test
		spritesheet = GetComponent<ReadSpriteSheet> ();// Sprite Sheet testing
		sprite = GetComponent<SpriteRenderer> ();// Sprite Sheet testing

		//StartCoroutine (SpriteSheetTest ()); // Sprite Sheet Testing;
		

	}
	public Sprite Frame(int frame)
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


	IEnumerator SpriteSheetTest(){ // only for testing
		for (int x = 0; x < 20; x++) {
			
			sprite.sprite = spritesheet.Frame (frame);
			yield return new WaitForSeconds (1f);
			frame += 1;//test texture is too  small so it doesnt fill the collider. Sprite should be correct size;
		}
	}

}
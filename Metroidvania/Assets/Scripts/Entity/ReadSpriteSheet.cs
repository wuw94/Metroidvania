using UnityEngine;
using System.Collections;

public class ReadSpriteSheet : MonoBehaviour {

	public Texture2D spriteSheet;
	public Vector2 xy;

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

	}


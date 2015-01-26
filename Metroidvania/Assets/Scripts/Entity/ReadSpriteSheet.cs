using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReadSpriteSheet : Recordable
{
	public Sprite[] still_sprites;
	public Sprite[] jump_sprites;
	public Sprite[] move_sprites;
	private Sprite[] current_loop;
	
	protected void ChangeLoop(Sprite[] new_loop)
	{
		current_loop = new_loop;
	}

	protected IEnumerator Animate()
	{
		while (true)
		{
			for (byte i = 0; i < current_loop.Length; i++)
			{
				GetComponent<SpriteRenderer>().sprite = current_loop[i];
				yield return new WaitForSeconds(0.1f);
			}
		}
	}

	private void UpdateFacing()
	{
		transform.localScale = new Vector3(this_info.facingRight ? Mathf.Abs(transform.localScale.x) : -Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
	}

	public override void NormalUpdate()
	{
		UpdateFacing();
	}
	
	public override void Record()
	{
		UpdateFacing();
	}
	
	public override void Rewind()
	{
		UpdateFacing();
	}
	
	public override void Playback()
	{
		UpdateFacing();
	}
}
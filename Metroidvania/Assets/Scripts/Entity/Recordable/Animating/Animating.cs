using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Animating : Recordable
{
	private Sprite[] current_loop = new Sprite[0];
	public byte loop_index = 0;

	public void Start()
	{
		if (Application.loadedLevelName != "TileEditor")
		{
			StartCoroutine(Animate());
		}
	}
	
	protected void ChangeLoop(Sprite[] new_loop)
	{
		current_loop = new_loop;
	}

	protected IEnumerator Animate()
	{
		while (true)
		{
			if (current_loop.Length > 0)
			{
				for (loop_index = 0; loop_index < current_loop.Length; loop_index++)
				{
					GetComponent<SpriteRenderer>().sprite = current_loop[loop_index];
					yield return new WaitForSeconds(0.08f);
				}
			}
			else
			{
				yield return null;
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
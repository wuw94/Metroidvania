using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Lever : Immobile
{
	public List<int> platforms = new List<int>();


	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Player")
		{
			List<Object> objects = new List<Object>(UnityEditor.Selection.objects);
			objects.Add(gameObject);
			for (int i = 0; i < platforms.Count; i++)
			{
				objects.Add(((DependantPlatform)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[3][platforms[i]]).gameObject);
			}
			UnityEditor.Selection.objects = objects.ToArray();
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.tag == "Player")
		{
			List<Object> objects = new List<Object>(UnityEditor.Selection.objects);
			objects.Remove(gameObject);
			for (int i = 0; i < platforms.Count; i++)
			{
				objects.Remove(((DependantPlatform)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[3][platforms[i]]).gameObject);
			}
			UnityEditor.Selection.objects = objects.ToArray();
		}
	}

	void Start()
	{
		ChangeLoop (move_sprites);
	}

	void Update()
	{
		if (GetComponent<SpriteRenderer>().sprite != still_sprites[this_info.eventState])
		{
			GetComponent<SpriteRenderer>().sprite = still_sprites[this_info.eventState];
		}
	}

	public override void Action()
	{
		base.Action ();

		if (this_info.eventState == 0)
		{

			this_info.eventState = 1;

			for (int i = 0; i < platforms.Count; i++)
			{
				((DependantPlatform)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[3][platforms[i]]).changeCollisions();
			}
		}
		else if (this_info.eventState == 1)
		{

			this_info.eventState = 0;
			for (int i = 0; i < platforms.Count; i++)
			{
				((DependantPlatform)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[3][platforms[i]]).changeCollisions();
			}
		}	

	}

	public override void UndoAction()
	{
		Action();
	}

	public override void NormalUpdate()
	{
		//base.NormalUpdate();
		previous_operation_mode = operation_mode;
	}

	public override void Record()
	{
		base.Record ();
		//base.Record();
	}
	
	public override void Rewind()
	{
		base.Rewind();
		//this_info.eventState = recorded_states[record_index].eventState;
	}

	public override void Playback()
	{
		base.Playback ();
		//this_info.eventState = recorded_states[record_index].eventState;
	}



	void OnDrawGizmosSelected()
	{
		try
		{
			for (int i = 0; i < platforms.Count; i++)
			{
				Gizmos.color = Color.magenta;
				Vector3 pos = ((DependantPlatform)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[3][platforms[i]]).transform.position;
				Gizmos.DrawLine(transform.position, new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z));
			}
		}
		catch
		{}
	}


}

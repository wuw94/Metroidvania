using UnityEngine;
using System.Collections;
using System.Linq;

/* Recordable.
 * All interactive objects in game world must inherit from Recordable, allows Time Shift to manipulate its values.
 * Attach this script to the main camera and nothing else for every map.
 * 
 * Call functions inside this class using:
 * Recordable.FunctionName();
 * 
 * 
 * IMPORTANT!
 * Recordable.recordInfo() and Recordable.readInfo() should only be called by the player
 * 
 * 
 * Overridden Functions (at the end):
 * - Use these functions in ALL objects instead of Update()/FixedUpdate()
 * - By using these functions, we can control what each object does at each point of a time shift
 * 
 * public override void NormalUpdate():
 * - Called on a normal game state, i.e. no time mechanic is running
 * 
 * public override void Record():
 * - Called during first phase of time shift, stores location of object into the list
 * 
 * public override void Rewind():
 * - Called during the second phase of time shift, loads the object locations in reverse order
 * 
 * public override void Playback():
 * - Called during the last phase of time shift, loads object locations in normal order
 * 
 */

public class Recordable : MonoBehaviour
{
	// For the recording data structure
	public static readonly int recorded_states_max = 1000;
	public static RecordInfo[][] recorded_states = new RecordInfo[recorded_states_max][];
	public static int recorded_states_filled = 0;
	public static int record_index = 0;
	public static int operation_mode = 0;
	public static readonly int change_mode_cd_max = 100;
	public static int change_mode_cd = 0;

	// Controls preferences
	public readonly int rewind_speed = 5;


	public static bool moved;


	void FixedUpdate()
	{
		// Change Mode CD
		if (change_mode_cd > 0)
		{
			change_mode_cd--;
		}
		else
		{
			change_mode_cd = 0;
		}

		// Operations
		if (operation_mode == 0)
		{
			NormalUpdate();
		}
		else if (operation_mode == 1)
		{
			Record();
			if (moved)
			{
				RecordAct();
			}
		}
		else if (operation_mode == 2)
		{
			Rewind();
		}
		else if (operation_mode == 3)
		{
			Playback();
		}
	}



	public static void ChangeOperationMode()
	{
		change_mode_cd = change_mode_cd_max;
		if (operation_mode < 3)
		{
			operation_mode++;
		}
		else
		{
			operation_mode = 0;
		}

		if (operation_mode == 0)
		{
			startNormalUpdate();
		}
		else if (operation_mode == 1)
		{
			startRecord();
		}
		else if (operation_mode == 2)
		{
			startRewind();
		}
		else if (operation_mode == 3)
		{
			startPlayback();
		}
		print("operation_mode changed, is now " + operation_mode);
	}


	private static void startNormalUpdate()
	{
		recorded_states = new RecordInfo[recorded_states_max][];
	}
	
	private static void startRecord()
	{
		record_index = 0;
		for (int i = 0; i < recorded_states.Length; i++)
		{
			recorded_states[i] = new RecordInfo[1];
		}
	}
	
	private static void startRewind()
	{
		recorded_states_filled = record_index;
	}
	
	private static void startPlayback()
	{
	}




	

	// Records each frame, to be accessed with Time Shift
	public static void recordInfo()
	{
		if (operation_mode == 1 && moved && record_index < recorded_states_max - 1)
		{
			GameObject[] current_frame_objects = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
			recorded_states[record_index] = new RecordInfo[current_frame_objects.Length];
			for (int i  = 0; i < current_frame_objects.Length; i++)
			{
				recorded_states[record_index][i] = new RecordInfo((int)EntityType.hash_type[current_frame_objects[i].name],0,
				                                                             current_frame_objects[i].transform.position.x,
				                                                             current_frame_objects[i].transform.position.y,
				                                                             0,0);
			}
			record_index++;
			print(record_index);
		}
	}

	// Reads each frame and edits the state of all of the world's objects to match that of which was recorded
	public static void readInfo()
	{
		if (record_index >= 0)
		{
			for (int i = 0; i < recorded_states[record_index].Length; i++)
			{
				if (!EntityType.ignore_type.Contains(recorded_states[record_index][i].prefabN))
				{
					RecordInfo info = recorded_states[record_index][i];
					GameObject obj = GameObject.Find(EntityType.list_type[info.prefabN]);
					obj.transform.Translate(info.posX - obj.transform.position.x,
					                        info.posY - obj.transform.position.y,
					                        0);
				}
			}
		}
	}






	//------------------------------ OVERRIDE FUNCTIONS ------------------------------

	public virtual void NormalUpdate()
	{
	}

	public virtual void Record()
	{
	}
	
	public virtual void RecordAct()
	{
	}

	public virtual void Rewind()
	{
		if (record_index > 0)
		{
			record_index -= rewind_speed;
		}
		else
		{
			record_index = 0;
			ChangeOperationMode();
		}
	}

	public virtual void Playback()
	{
		if (record_index < recorded_states_filled-1)
		{
			record_index++;
		}
		else
		{
			ChangeOperationMode();
		}
	}
}

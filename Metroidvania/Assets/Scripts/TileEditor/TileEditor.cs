using UnityEngine;
using System.Collections;

/* TileEditor
 * 
 * IMPORTANT! Do not put this on any object other than main camera
 * Manages the basics of the TileEditor scene
 */
public class TileEditor : MonoBehaviour
{
	public UnityEngine.UI.InputField tile_type_input;
	public UnityEngine.UI.InputField save_path_input;

	public float camera_speed = 0.5f;

	public string tile_type = "";
	string save_path = "TestSceneTileEditor";

	void Update()
	{
		if (Input.GetKey(KeyCode.RightArrow))
		{
			transform.position += new Vector3(camera_speed,0,0);
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			transform.position += new Vector3(-camera_speed,0,0);
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			transform.position += new Vector3(0,camera_speed,0);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			transform.position += new Vector3(0,-camera_speed,0);
		}
		manageZoom();
	}

	void manageZoom()
	{
		GetComponent<Camera>().orthographicSize += Input.GetAxis("Mouse ScrollWheel");
		if (GetComponent<Camera>().orthographicSize < 0)
		{
			GetComponent<Camera>().orthographicSize = 0.01f;
		}
	}
	

	public void setTileType()
	{
		//tile_type = tile_type_input.value;
	}

	public void reposition()
	{
		transform.position = new Vector3(0, 0, transform.position.z);
	}

	public void doSave()
	{
		print(Application.dataPath + "/Scenes/TileEditorScenes/" + save_path + ".unity");
		//TileEditorManager.Save(Application.dataPath + "/Scenes/TileEditorScenes/" + save_path);
	}
}

using UnityEngine;
using System.Collections;

/* TileDisplay
 * 
 * 0.5f unit width, 0.5f unit height
 * One of the 4 corners of a tile. It adjusts its scaling automatically on Start()
 * Given a setImage(Texture2D img, int rot) call, will replace its old image and rotation
 * 
 */

public sealed class TileDisplay : MonoBehaviour
{
	public short rotation;
	private Vector3 euler = new Vector3(0,0,0);


	void Start()
	{
		transform.localScale = new Vector3(0.5f/(renderer.bounds.max.x - renderer.bounds.min.x),
		                                   0.5f/(renderer.bounds.max.y - renderer.bounds.min.y),
		                                   1);
	}

	public void setImage(Sprite spr, int rot)
	{
		rotation = (short)rot;
		GetComponent<SpriteRenderer>().sprite = spr;
		Quaternion quat = Quaternion.identity;
		euler.z = rotation;
		quat.eulerAngles = euler;
		transform.rotation = quat;
	}

	public bool IsVisible()
	{
		return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), renderer.bounds);
	}
}

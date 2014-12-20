using UnityEngine;
using System.Collections;

/* TileDisplay
 * 
 * 0.5f unit width, 0.5f unit height
 * One of the 4 corners of a tile. It adjusts its scaling automatically on Start()
 * Given a setImage(Texture2D img, int rot) call, will replace its old image and rotation
 * 
 */

public class TileDisplay : MonoBehaviour
{
	private Texture2D tile_image;
	public int rows;
	public int columns;
	private Rect display_rect = new Rect();
	private Vector2 pivot = new Vector2(0.5f, 0.5f);
	public int rotation;


	void Start()
	{
		transform.localScale = new Vector3(0.5f/(renderer.bounds.max.x - renderer.bounds.min.x),
		                                   0.5f/(renderer.bounds.max.y - renderer.bounds.min.y),
		                                   1);
	}
	
	public void setImage(Texture2D img, int rot)
	{
		rotation = rot;
		display_rect.width = img.width;
		display_rect.height = img.height;
		GetComponent<SpriteRenderer>().sprite = Sprite.Create(img, display_rect, pivot);
		Quaternion quat = Quaternion.identity;
		quat.eulerAngles = new Vector3(0,0,rotation);
		transform.rotation = quat;
	}

	public bool IsVisible()
	{
		return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), renderer.bounds);
	}
}

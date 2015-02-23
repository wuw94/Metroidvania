using UnityEngine;
using System.Collections;

public class CollisionLight : MonoBehaviour
{
	
	void Start()
	{
		Invoke ("DestroyNow", 0.2f);
	}
	

	void Update()
	{
		transform.localPosition = new Vector3(transform.localPosition.x + 0.3f, transform.localPosition.y, transform.localPosition.z);
	}

	void DestroyNow()
	{
		Destroy(gameObject);
	}
}

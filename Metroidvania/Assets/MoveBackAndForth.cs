using UnityEngine;
using System.Collections;

public class MoveBackAndForth : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position = new Vector2(transform.position.x + 1, transform.position.y);
	}
}

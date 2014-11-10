using UnityEngine;
using System.Collections;

public class BasicPlayerMovement : GlobalVariables {
	bool grounded = true;
	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == "ImpassableTopBottom")
		{
			grounded = true;
		}
	}

	// Use this for initialization
	void Start () {
		TestStuff();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float x = rigidbody2D.velocity.x;
		float y = rigidbody2D.velocity.y;
		if(Input.GetKey(IN_RIGHT))
		{
			rigidbody2D.velocity = new Vector2(x+0.3f,y);
		}
		if(Input.GetKey(IN_LEFT))
		{
			rigidbody2D.velocity = new Vector2(x-0.3f,y);
		}
		if(Input.GetKey(IN_JUMP) && grounded)
		{
			rigidbody2D.velocity = new Vector2(x,y+10);
			grounded = false;
		}
	}
}

//<Jessica Parhusip>
//<Jude Collins> <Cjude>
//<Juston Lin> <Azirly>
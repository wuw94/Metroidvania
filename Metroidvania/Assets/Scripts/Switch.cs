using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour {
	public GroundTrigger[] groundTriggers;
	private Animator animator;
	
	// Use this for initialization
	void Start () 
	{
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
	
	void OnTriggerEnter2D(Collider2D target)
	{
		if (target.gameObject.tag == "Player") 
		{
			animator.SetInteger ("AnimState", 1);

			foreach(GroundTrigger trigger in groundTriggers)
				if (trigger != null)
					trigger.Toggle(true);
		}
	}
	
	void OnTriggerExit2D(Collider2D target)
	{
		animator.SetInteger("AnimState", 2);

		foreach(GroundTrigger trigger in groundTriggers)
			if (trigger != null)
				trigger.Toggle(false);
	}
}

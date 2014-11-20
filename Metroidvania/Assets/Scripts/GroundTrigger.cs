using UnityEngine;
using System.Collections;

public class GroundTrigger : MonoBehaviour 
{
	public Ground[] grounds;
	private Animator animator;

	// Use this for initialization
	void Start () 
	{
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void Toggle(bool value)
	{
		foreach (Ground ground in grounds) 
		{
			if (value)
				ground.Raise ();
			else
				ground.Lower ();
		}
	}
}

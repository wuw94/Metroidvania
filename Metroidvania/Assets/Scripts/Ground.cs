using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {

	private Animator animator;
	public float raiseDelay = .2f;
	
	// Use this for initialization
	void Start () 
	{
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public void Raise()
	{
		animator.SetInteger ("AnimState", 1);
	}

	public void Lower()
	{
		StartCoroutine (LowerNow ());
	}

	private IEnumerator LowerNow()
	{
		yield return new WaitForSeconds(raiseDelay);
		animator.SetInteger("AnimState", 2);
	}
}

using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	public int Character_Health;
	// Use this for initialization
	void Start () {
		Character_Health = 100;
	}
	
	// Update is called once per frame
	void Update () {
		print (Character_Health);
	
	}
}

using UnityEngine;
using System.Collections;

/*
 * What this basically does is: Check for an existing instance, 
 * if there already is an instances, destroy self - if not, 
 * store this is instance (so anyone who's coming later will destroy themselves). 
 * This also provides a public accessor so you can access the single instance 
 * from anywhere via MyUnitySingleton.Instance. 
 * Of course, you'd have your sound-specific methods also in that same class/script.
 */

public class MyUnitySingleton : MonoBehaviour {

	private AudioSource audioSource;
	private static MyUnitySingleton instance = null;

	public static MyUnitySingleton Instance 
	{
		get { return instance; }
	}
		
	void Awake() 
	{
		if (instance != null && instance != this) {
				Destroy (this.gameObject);
				return;
		} else 
				instance = this;

		DontDestroyOnLoad (this.gameObject);			
	}
}

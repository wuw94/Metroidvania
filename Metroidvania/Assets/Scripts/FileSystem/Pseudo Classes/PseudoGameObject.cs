using UnityEngine;
using System.Collections;

/// <summary>
/// PseudoGameObject is [Serializable] and implicitly castable from GameObject.
/// </summary>
[System.Serializable]
public sealed class PseudoGameObject<T> where T : MonoBehaviour
{
	public PseudoVector3 position;

	/// <summary>
	/// Constructor. This is private because we don't want to be able to construct a PseudoGameObject.
	/// We only want to be able to typecast it from an already made GameObject.
	/// </summary>
	/// <param name="go">Go.</param>
	private PseudoGameObject(T go)
	{
		position = go.transform.position;
	}


	/// <summary>
	/// Returns a string representation of the object
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		return string.Format("PseudoGameObject<{0}>()", typeof(T).ToString());
	}
	
	/// <summary>
	/// Automatic conversion from PseudoGameObject to Monobehavior Object
	/// </summary>
	/// <param name="rValue"></param>
	/// <returns></returns>
	public static implicit operator T(PseudoGameObject<T> rValue)
	{
		if (typeof(T) == typeof(Player))
		{
			return ((GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/Mobiles/Player/Player", typeof(GameObject)), rValue.position, Quaternion.identity)).GetComponent<T>();
		}
		else
		{
			return new GameObject().AddComponent<T>();
		}
	}
	
	/// <summary>
	/// Automatic conversion from Monobehavior Object to PseudoGameObject. When converting, will destroy the GameObject,
	/// leaving you only the PseudoGameObject as its representation.
	/// </summary>
	/// <param name="rValue"></param>
	/// <returns></returns>
	public static implicit operator PseudoGameObject<T>(T rValue)
	{
		PseudoGameObject<T> pgo = new PseudoGameObject<T>(rValue);
		MonoBehaviour.Destroy(rValue.gameObject);
		return pgo;
	}
	
}

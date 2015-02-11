using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// PseudoGameObject is [Serializable] and implicitly castable from GameObject.
/// </summary>
[System.Serializable]
public sealed class PseudoGameObject<T> where T : MonoBehaviour
{
	public PseudoVector3 position;
	
	public List<FieldInformation> information;



	/// <summary>
	/// Constructor. This is private because we don't want to be able to construct a PseudoGameObject.
	/// We only want to be able to typecast it from an already made GameObject.
	/// </summary>
	/// <param name="go">Go.</param>
	public PseudoGameObject(T go)
	{
		this.position = go.transform.position;
		this.information = new List<FieldInformation>();
		foreach (FieldInfo field in go.GetType().GetFields())
		{
			if (((field.FieldType.IsSerializable && field.FieldType.GetElementType() == null) || (field.FieldType.GetElementType() != null && field.FieldType.GetElementType().IsSerializable)))
			{
				this.information.Add(new FieldInformation(field.DeclaringType.ToString(), field.Name, field.GetValue(go)));
			}
		}
		MonoBehaviour.Destroy(go.gameObject);
	}




	public static T CreateGameObject<T>(PseudoGameObject<T> pgo) where T : MonoBehaviour
	{
		return (T)pgo;
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
		T obj = ((GameObject)MonoBehaviour.Instantiate(Resources.Load(ResourceDirectory.resource[typeof(T)].path, typeof(GameObject)), rValue.position, Quaternion.identity)).GetComponent<T>();

		foreach (FieldInfo field in typeof(T).GetFields())
		{
			if (((field.FieldType.IsSerializable && field.FieldType.GetElementType() == null) || (field.FieldType.GetElementType() != null && field.FieldType.GetElementType().IsSerializable)))
			{
				foreach (FieldInformation info in rValue.information)
				{
					if (field.DeclaringType.ToString().Equals(info.declaring_type) && field.Name.Equals(info.field_name) && field.FieldType.Equals(info.field_value.GetType()))
					{
						field.SetValue(obj, info.field_value);
					}
				}
			}
		}
		return obj;
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


		return pgo;
	}
	
}

using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;



public class test : MonoBehaviour
{
	void Start()
	{
		/*
		System.Type genericType = typeof(Repository<>);
		System.Type[] typeArgs = {System.Type.GetType("TypeRepository")};
		System.Type repositoryType = genericType.MakeGenericType(typeArgs);
		System.Object repository = System.Activator.CreateInstance(repositoryType);

		System.Reflection.MethodInfo genericMethod = repositoryType.GetMethod("GetMeSomething");
		System.Reflection.MethodInfo closedMethod = genericMethod.MakeGenericMethod(typeof(Something));
		closedMethod.Invoke(repository, new[] {"Query String"});
		*/

		Player player = ((GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/Mobiles/Player/Player", typeof(GameObject)), new Vector3(0,0,0), Quaternion.identity)).GetComponent<Player>();

		Type generic = typeof(PseudoGameObject<>);
		Type[] typeArgs = {typeof(Player)};
		Type constructed = generic.MakeGenericType(typeArgs);


		object ps_object = Activator.CreateInstance(constructed, new object[]{player});


		//ps_object = player;
		Debug.Log(ps_object);
		Debug.Log(ps_object.GetType());

		//object ps_player = (object)Convert.ChangeType(player, constructed);

		//PseudoGameObject<Player> ps_player = player;
		//Debug.Log(ps_player);
	}

}

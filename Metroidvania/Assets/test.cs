using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;



public class test : MonoBehaviour
{
	void Start()
	{
		Child c = new Child();
		Debug.Log(c.a);
	}
}


public class Parent
{
	public int a = 1;
}

public class Child : Parent
{
	public int a = 0;
	public Child()
	{
	}
}
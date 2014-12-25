using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Security.Permissions;


[System.Serializable]
public class test : MonoBehaviour
{
	public SerializeTest t;

	[System.Serializable]
	public class SerializeTest : ISerializable
	{
		public string a;
		public string b;
		public string c;

		public SerializeTest()
		{
			a = "az";
			b = "bz";
			c = "cz";
		}


		protected SerializeTest(SerializationInfo info, StreamingContext context) // Called when deserializing data
		{
			a = (string)info.GetValue("a", typeof(string));
			b = (string)info.GetValue("b", typeof(string));
			c = (string)info.GetValue("c", typeof(string));
		}

		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
       	public virtual void GetObjectData(SerializationInfo info, StreamingContext context) // Called when serializing data
	    {
			info.AddValue("a", a);
			info.AddValue("b", b);
			info.AddValue("c", c);
		}

	}

	void Start()
	{
		//Deserialize();
		//print(t.a);
		//print(t.c);
	}


	void Serialize()
	{
		SerializeTest t = new SerializeTest();
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.dataPath + "/Maps/test.test");
		bf.Serialize(file, t);
		Debug.Log("Saved: " + Application.dataPath + "/Maps/test.test");
		file.Close();
	}

	void Deserialize()
	{
		FileStream file = null;
		try
		{
			Debug.Log("Converting File: " + Application.dataPath + "/Maps/test.test");
			BinaryFormatter bf = new BinaryFormatter();
			file = File.Open(Application.dataPath + "/Maps/test.test", FileMode.Open);
			t = (SerializeTest)bf.Deserialize(file);
			Debug.Log("Success: " + Application.dataPath + "/Maps/test.test");
		}
		catch (System.Runtime.Serialization.SerializationException)
		{
			Debug.LogWarning("Wrong Version");
		}
		finally
		{
			if (file != null)
			{
				file.Close();
			}
		}
	}
}

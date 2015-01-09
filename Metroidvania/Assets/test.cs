using UnityEngine;
using System.Collections;

using System.Threading;



public class test : MonoBehaviour
{
	float x = 1f;
	bool threadRunning = true;
	private Object thislock = new Object();

	void Start()
	{

		new Thread(Multiply).Start();

		threadRunning = false;
	}

	void Multiply()
	{
		while (threadRunning)
		{
			//Debug.Log("thread action");

			lock(thislock)
			{
				x++;
			}
		}
	}

}

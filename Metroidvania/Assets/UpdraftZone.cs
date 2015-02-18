using UnityEngine;
using System.Collections;

public class UpdraftZone : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.GetComponent<Mobile>() != null && col.gameObject.GetComponent<Mobile>().equipment.Contains(Equipment.Parachute))
		{
			Debug.Log("entered");
			col.gameObject.GetComponent<Mobile>().updraft_contact = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.GetComponent<Mobile>() != null)
		{
			col.gameObject.GetComponent<Mobile>().updraft_contact = false;
		}
	}
}

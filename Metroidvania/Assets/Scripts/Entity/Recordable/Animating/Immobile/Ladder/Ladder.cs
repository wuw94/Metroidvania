using UnityEngine;
using System.Collections;

//Function will be moved to Controllable
public class Ladder : Controllable
{
	public bool on_ladder;
	public GameObject Player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Player near ladder orr Player on ladder){
			Touch_the_ladder()
			Ladder_controls()
		}
		if(Player Attribute the endd offff Ladder){
			At_the_end_of_ladder()
		}
	}
	public void Touch_the_ladder()
	{
		//You lock into place and stay on the ladder
		if(){
			on_ladder = true;
		}
	}
	public void At_the_end_of_ladder()
	{
		if(){
			on_ladder = false;
		}
	}
	public void Ladder_controls(){
		if (on_ladder = true) {
			
			//controls will be for ladder
			if(button_for_up){
				moveUp()
			}
			if(button_for_down){
				moveDown()
			}
			if(button_for_left_and_right){
				on_ladder = false
			}
			
		} else {
			//controls will be for when on stationary ground
		}
	}
}


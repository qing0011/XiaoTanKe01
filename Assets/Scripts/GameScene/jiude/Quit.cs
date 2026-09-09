using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Quit : MonoBehaviour {

	void Start(){

		Input.backButtonLeavesApp = true;
	}
	// Use this for initialization

	public void doquit () {

		Application.Quit ();


		if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home) )

		{
			Application.Quit(); //exit when key back
		}
	}
}


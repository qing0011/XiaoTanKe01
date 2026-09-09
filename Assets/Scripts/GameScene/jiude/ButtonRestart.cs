using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using GoogleMobileAds.Api;


public class ButtonRestart : MonoBehaviour {

	// Restart Game level
	public void restartCurrentScene () 
	{
		//SceneManager.LoadScene ("HelixAxeL2");
		int scene = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(scene, LoadSceneMode.Single);
	}


}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class NexLevelButton : MonoBehaviour {

	public string levelName;
	// Use this for initialization
	public void nextLevel () 
	{
		//SceneManager.LoadScene ("HelixAxeL2");
		SceneManager.LoadScene (levelName);
		PlayerPrefs.DeleteAll ();

	}
}

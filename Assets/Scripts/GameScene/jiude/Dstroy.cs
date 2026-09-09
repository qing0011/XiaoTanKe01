using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dstroy : MonoBehaviour {
	public Button RestartButton;

	// Display Button whene Player lose
	void OncolisionEnter (Collision other) {

		RestartButton.gameObject.SetActive (true);
	}
}

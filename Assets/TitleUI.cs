using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour {


	public void StartGame() {
		Debug.Log ("start clicked!");
		SceneManager.LoadScene ("SpaceMan");
	}

	public void QuitGame() {
		Debug.Log ("quit clicked!");
		Application.Quit ();
	}
}

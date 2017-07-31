using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TitleUI : MonoBehaviour {


	public RectTransform instructionsWindow;
	public RectTransform mainTitleMenu;

	public void StartGame() {
		Debug.Log ("start clicked!");
		SceneManager.LoadScene ("SpaceMan");
	}

	public void QuitGame() {
		
		
		Debug.Log ("quit clicked!");
		Application.Quit ();
	}

	public void ShowInstructions() {
		mainTitleMenu.gameObject.SetActive(false);
		instructionsWindow.gameObject.SetActive (true);
	}

}

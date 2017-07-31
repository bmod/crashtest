using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TitleUI : MonoBehaviour {


	public RectTransform instructionsWindow;
	public RectTransform mainTitleMenu;

	public void StartGame() {
		Debug.Log ("start clicked!");
		MusicManager.instance.PlayConfirm ();
		SceneManager.LoadScene ("SpaceMan");
	}

	public void QuitGame() {
		
		
		Debug.Log ("quit clicked!");
		MusicManager.instance.PlayConfirm ();
		Application.Quit ();
	}

	public void ShowInstructions() {
		mainTitleMenu.gameObject.SetActive(false);
		instructionsWindow.gameObject.SetActive (true);
	}

}

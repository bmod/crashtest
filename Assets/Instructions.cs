using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour {


	public 	RectTransform mainTitleMenu;
	public RectTransform highScore;
	public void BackToTitleMenu() {
		gameObject.SetActive (false);

		mainTitleMenu.gameObject.SetActive (true);
		highScore.gameObject.SetActive (true);
	}
}

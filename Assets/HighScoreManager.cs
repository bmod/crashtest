using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScoreManager : MonoBehaviour {

	public static HighScoreManager instance;


	void Awake() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (instance);
			SceneManager.sceneLoaded += (Scene arg0, LoadSceneMode arg1) => {
				if(arg0.name == "Title") {
					Debug.Log("title loaded!");
					GameObject go = GameObject.Find("HighScorePanel");
					go.GetComponent<HighScoreTitleDisplayt>().scoreText.text = "HIGH SCORE:\n"+  HighScoreManager.instance.GetHighScore();
				}
			};
		} else {
			Debug.LogError ("instance had to be destroyed");
			Destroy (this);
		}
	}

	public void SetHighScore(int highScore) {
		PlayerPrefs.SetInt ("high_score", highScore);
	}

	public int GetHighScore() {
		return PlayerPrefs.GetInt ("high_score");
	}

}

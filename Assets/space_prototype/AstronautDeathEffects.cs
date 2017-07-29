using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AstronautDeathEffects : MonoBehaviour {


	public Damageable astronautDamageable;

	// Use this for initialization
	void Start () {
		astronautDamageable.OnKilled += GameOverOnKilled;
	}

	void GameOverOnKilled(Missile source) {
		Debug.Log ("Game over mon!");
		Destroy (astronautDamageable.gameObject);
		Debug.LogError ("We need to update this function with Game over UI");
		StartCoroutine (RestartScene ());
	}


	IEnumerator RestartScene() {
		yield return new WaitForSeconds (2f);
		SceneManager.LoadScene (0);
	}

	void OnDestroy() {
		astronautDamageable.OnKilled -= GameOverOnKilled;
	}

}

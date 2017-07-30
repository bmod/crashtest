using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AstronautDeathEffects : MonoBehaviour {


	public Damageable astronautDamageable;
	public GameObject deathPrefab;

	// Use this for initialization
	void Start () {
		astronautDamageable.OnKilled += GameOverOnKilled;
	}

	void GameOverOnKilled(DamageSource source) {
		Debug.Log ("Game over mon!");
		Destroy (astronautDamageable.gameObject);


		GameObject go = Instantiate (deathPrefab);
		go.transform.localRotation = astronautDamageable.transform.localRotation;
		go.transform.position = astronautDamageable.transform.position;

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

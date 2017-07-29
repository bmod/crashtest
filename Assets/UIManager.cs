using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	public Damageable playerDamageable;
	public Image healthBar;
	public Image powerBar;
	float ogHealthbarWidth;
	float ogPowerbarWidth;

	[SerializeField]
	float power;
	public float maxPower = 50f;

	// Use this for initialization
	void Start () {
		power = maxPower;

		GameObject go = GameObject.Find ("SpaceMan");
		if (go != null) {
			playerDamageable = go.GetComponent<Damageable> ();
			playerDamageable.OnDamageTaken += UpdateBarOnPlayerDamaged;
		} else {
			Debug.LogError ("Couldn't find the SpaceMan object in camera");
		}


		ogHealthbarWidth = healthBar.rectTransform.sizeDelta.x;
		ogPowerbarWidth = powerBar.rectTransform.sizeDelta.x;

		StartCoroutine (DecreasePower());
	}

	void UpdateBarOnPlayerDamaged(int amount, Missile source) {
		healthBar.rectTransform.sizeDelta = new Vector2 (playerDamageable.HealthRatio * ogHealthbarWidth, healthBar.rectTransform.sizeDelta.y);
	}

	IEnumerator DecreasePower() {
		yield return new WaitForSeconds (1f);
		power -= 1f;

		powerBar.rectTransform.sizeDelta = new Vector2 (PowerRatio * ogPowerbarWidth, powerBar.rectTransform.sizeDelta.y);
		if (power < 0f) {
			GameOver ();
		}
		StartCoroutine (DecreasePower ());
	}

	void GameOver() {
		StartCoroutine (RestartScene ());
	}
	IEnumerator RestartScene() {
		yield return new WaitForSeconds (2f);
		SceneManager.LoadScene (0);
	}


	public float PowerRatio {
		get{ 
			if (power < 0f) {
				return 0f;
			}
			return power / maxPower;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

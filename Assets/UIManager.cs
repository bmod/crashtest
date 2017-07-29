using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	public Damageable playerDamageable;
	public PowerRunningOut powerRunOut;
	public Image healthBar;
	public Image powerBar;
	float ogHealthbarWidth;
	float ogPowerbarWidth;



	// Use this for initialization
	void Start () {

		GameObject go = GameObject.Find ("SpaceMan");
		if (go != null) {
			playerDamageable = go.GetComponent<Damageable> ();
			playerDamageable.OnDamageTaken += UpdateBarOnPlayerDamaged;

			powerRunOut = go.GetComponent<PowerRunningOut> ();
			powerRunOut.OnLostPower += UpdateBarOnPlayerLostPower;

		} else {
			Debug.LogError ("Couldn't find the SpaceMan object in camera");
		}


		ogHealthbarWidth = healthBar.rectTransform.sizeDelta.x;
		ogPowerbarWidth = powerBar.rectTransform.sizeDelta.x;


	}

	void OnDestroy() {
		playerDamageable.OnDamageTaken -= UpdateBarOnPlayerDamaged;
		powerRunOut.OnLostPower -= UpdateBarOnPlayerLostPower;
	}

	void UpdateBarOnPlayerDamaged(int amount, Missile source) {
		healthBar.rectTransform.sizeDelta = new Vector2 (playerDamageable.HealthRatio * ogHealthbarWidth, healthBar.rectTransform.sizeDelta.y);
	}


	void UpdateBarOnPlayerLostPower(float amount) {
		powerBar.rectTransform.sizeDelta = new Vector2 (powerRunOut.PowerRatio * ogPowerbarWidth, powerBar.rectTransform.sizeDelta.y);
	}


	
	// Update is called once per frame
	void Update () {
		
	}
}

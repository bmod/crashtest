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

	public Text gameOverText;

	public RectTransform gameOverRoot;

	// Use this for initialization
	void Start () {

		GameObject go = GameObject.Find ("SpaceMan");
		if (go != null) {
			playerDamageable = go.GetComponent<Damageable> ();
			playerDamageable.OnDamageTaken += UpdateBarOnPlayerDamaged;
			playerDamageable.OnHealed += UpdateBarOnPlayerHealed;

			powerRunOut = go.GetComponent<PowerRunningOut> ();
			powerRunOut.OnLostPower += UpdateBarOnPlayerLostPower;
			powerRunOut.OnGainedPower += UpdateBarOnPlayerGainedPower;

		} else {
			Debug.LogError ("Couldn't find the SpaceMan object in camera");
		}


		gameOverRoot.gameObject.SetActive (false);

		ogHealthbarWidth = healthBar.rectTransform.sizeDelta.x;
		ogPowerbarWidth = powerBar.rectTransform.sizeDelta.x;


	}

	public static UIManager instance;

	void Awake() {
		if (instance == null) {
			instance = this;

		} else {
			Debug.LogError ("Had to destroy dupe: " + GetType());
			Destroy (this);
		}
	}

	void OnDestroy() {
		playerDamageable.OnDamageTaken -= UpdateBarOnPlayerDamaged;
		powerRunOut.OnLostPower -= UpdateBarOnPlayerLostPower;
	}

	void UpdateBarOnPlayerDamaged(int amount, DamageSource source) {
		healthBar.rectTransform.sizeDelta = new Vector2 (playerDamageable.HealthRatio * ogHealthbarWidth, healthBar.rectTransform.sizeDelta.y);
	}
	void UpdateBarOnPlayerHealed(int amount) {
		healthBar.rectTransform.sizeDelta = new Vector2 (playerDamageable.HealthRatio * ogHealthbarWidth, healthBar.rectTransform.sizeDelta.y);
	}


	void UpdateBarOnPlayerLostPower(float amount) {
		powerBar.rectTransform.sizeDelta = new Vector2 (powerRunOut.PowerRatio * ogPowerbarWidth, powerBar.rectTransform.sizeDelta.y);
	}

	void UpdateBarOnPlayerGainedPower(float amount) {
		powerBar.rectTransform.sizeDelta = new Vector2 (powerRunOut.PowerRatio * ogPowerbarWidth, powerBar.rectTransform.sizeDelta.y);
	}

	public void PresentGameOver() {


		StartCoroutine (DelayPresentGameOverMenu());
		MusicManager.instance.FadeMusicPitchDown ();
	}

	public IEnumerator DelayPresentGameOverMenu() {
		yield return new WaitForSeconds (3.3f);

		if (PickupDropChanceManager.instance.score > HighScoreManager.instance.GetHighScore ()) {
			gameOverText.text = "GAME OVER!\nNew High Score: " + PickupDropChanceManager.instance.score;
			HighScoreManager.instance.SetHighScore (PickupDropChanceManager.instance.score);
		} else {
			gameOverText.text = "GAME OVER!\nYour Score: "+PickupDropChanceManager.instance.score+"\nHigh Score: " + HighScoreManager.instance.GetHighScore ();
		}

		gameOverRoot.gameObject.SetActive (true);
		PlayerControls.instance.gameObject.SetActive (false);
	}

	public void BackToTitle() {
		SceneManager.LoadScene ("Title");	
		MusicManager.instance.PlayConfirm ();
		MusicManager.instance.FadeMusicPitchUp ();
	}

	public void Retry() {
		MusicManager.instance.PlayConfirm ();
		SceneManager.LoadScene ("SpaceMan");
		MusicManager.instance.FadeMusicPitchUp();
	}

}

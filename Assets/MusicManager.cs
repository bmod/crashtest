using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {


	public static MusicManager instance;

	AudioSource musicSource;
	public AudioSource menuConfirm;


	void Awake() {
		Screen.SetResolution (1600, 900, true);
		if (instance != null) {
			Destroy (this);
			Debug.LogError ("Can't have multiple " + GetType () + "!");
		} else {
			musicSource = GetComponent<AudioSource> ();
			musicSource.Play ();
			instance = this;
			DontDestroyOnLoad (this);
		}
	}

	public void PlayConfirm() {
		menuConfirm.Play ();
	}

	public float gameOverMusicPitch = .7f;
	Coroutine lastPitchChangeRoute;

	public void FadeMusicPitchDown() {
		if (lastPitchChangeRoute != null) {
			StopCoroutine (lastPitchChangeRoute);
		}
		StartCoroutine (DownPitchRoute ());
	}

	public void FadeMusicPitchUp() {
		if (lastPitchChangeRoute != null) {
			StopCoroutine (lastPitchChangeRoute);
		}
		StartCoroutine (UpPitchRoute ());
	}

	public float fadeSpeed = .4f;
	public IEnumerator DownPitchRoute() {
		float runningPitch = musicSource.pitch;

		while (runningPitch > gameOverMusicPitch) {
			runningPitch -= Time.deltaTime * fadeSpeed;
			musicSource.pitch = runningPitch;
			yield return null;
		}
		musicSource.pitch = gameOverMusicPitch;
	}

	public IEnumerator UpPitchRoute() {
		float runningPitch = musicSource.pitch;

		while (runningPitch < 1f) {
			runningPitch += Time.deltaTime * fadeSpeed * 5f;
			musicSource.pitch = runningPitch;
			yield return null;
		}
		musicSource.pitch = 1f;
	}

}

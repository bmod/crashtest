using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {


	public static MusicManager instance;

	void Awake() {
		if (instance != null) {
			Destroy (this);
			Debug.LogError ("Can't have multiple " + GetType () + "!");
		} else {
			instance = this;
			DontDestroyOnLoad (this);
		}
	}

}

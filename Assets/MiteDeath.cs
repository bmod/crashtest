using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiteDeath : MonoBehaviour {



	// Use this for initialization
	void Start () {
		GetComponent<Damageable> ().OnKilled += BlowUpOnKilled;
	}

	void OnDestroy() {
		GetComponent<Damageable> ().OnKilled -= BlowUpOnKilled;
	}

	void BlowUpOnKilled(Missile source) {
		Destroy (gameObject);
	}

}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MissileLauncher {
	
}

public class Missile : MonoBehaviour, DamageSource {

	public int damageInflicted = 5;

	public  MissileLauncher launcher;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("CameraEdge")) {
			Destroy (gameObject);
			return;
		}
		Damageable damageable = other.GetComponent<Damageable> ();
		if (damageable != null) {
			damageable.TakeDamage (damageInflicted, this);

			Rigidbody2D rgbdy = damageable.GetComponent<Rigidbody2D> ();
			if (!damageable.CompareTag ("Player")) {
				rgbdy.AddForceAtPosition (GetComponent<Rigidbody2D> ().velocity.normalized * 2f, transform.position);
			} else {
				rgbdy.AddForceAtPosition (GetComponent<Rigidbody2D> ().velocity.normalized * .1f, transform.position);
			}

			Destroy (gameObject);
		}


	}
}

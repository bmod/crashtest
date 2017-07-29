using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour, DamageSource {

	public int damageInflicted = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("collided with something: " + other.name);
		Damageable damageable = other.GetComponent<Damageable> ();
		if (damageable != null) {
			damageable.TakeDamage (damageInflicted, this);
		}


		Destroy (gameObject);
	}
}

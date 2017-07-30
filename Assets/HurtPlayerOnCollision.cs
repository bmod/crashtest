using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayerOnCollision : MonoBehaviour, DamageSource {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.collider.CompareTag ("Player")) {
			Damageable damageable = other.collider.GetComponent<Damageable> ();
			if (damageable != null) {
				damageable.TakeDamage (1, this);

				Damageable myDamageable = GetComponent<Damageable> ();
				if (myDamageable != null) {
					myDamageable.TakeDamage (1, this);
				}
			}




		}
	}
}

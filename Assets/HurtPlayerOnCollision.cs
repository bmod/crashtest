using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayerOnCollision : MonoBehaviour, DamageSource {


	public float velocityMagThreshold = 5f;

	void OnCollisionEnter2D(Collision2D other) {
		if (other.collider.CompareTag ("Player")) {
			
			Rigidbody2D rgbd = other.collider.GetComponent<Rigidbody2D> ();
			if (other.relativeVelocity.sqrMagnitude > velocityMagThreshold * velocityMagThreshold) {
				Damageable damageable = other.collider.GetComponent<Damageable> ();
				if (damageable != null) {
					damageable.TakeDamage (1, this);

					Damageable myDamageable = GetComponent<Damageable> ();
					if (myDamageable != null) {
						myDamageable.TakeDamage (1, this);
					}
				}
			} else {
				Debug.Log ("velocity not high enough: " + rgbd.velocity.magnitude);
			}




		}
	}
}

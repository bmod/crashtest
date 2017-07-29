using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Just so there's commonality between missiles/etc...
public interface DamageSource {
	
}

public class MiteSeeking : MonoBehaviour, DamageSource {
	

	public float distanceToChangeToAttacking = 2f;

	public float seekPower = 2f;
	// Update is called once per frame
	void Update () {
		Seek ();
		CheckForAnimChange ();

	}

	bool attackingSet = false;

	void CheckForAnimChange() {
		if (InRange () && !attackingSet) {
			attackingSet = true;
			GetComponent<Animator> ().SetTrigger ("Attack");
		} else if(attackingSet && !InRange()) {
			attackingSet = false;
			GetComponent<Animator> ().SetTrigger ("Seek");
		}
	}



	bool InRange() {
		bool inr = distanceToChangeToAttacking * distanceToChangeToAttacking >
		(PlayerControls.instance.astronaut.position - transform.position).sqrMagnitude;
		return inr;
	}

	public void CheckForOverlapAndDamagePlayer() {
		Collider2D player = PlayerControls.instance.astronaut.GetComponent<Collider2D> ();
		if (GetComponent<Collider2D> ().IsTouching (player)) {

			Debug.Log ("damag!");
			player.GetComponent<Damageable> ().TakeDamage (1, this);
		} else {
			Debug.Log ("Not overlaping!");
		}
	}

	void Seek() {
		Vector2 directions = PlayerControls.instance.astronaut.position - transform.position;

		directions = directions + PlayerControls.instance.astronaut.GetComponent<Rigidbody2D> ().velocity;

		directions = directions.normalized * seekPower;
		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (directions.x, directions.y));
	}

}

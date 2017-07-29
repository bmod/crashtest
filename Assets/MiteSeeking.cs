using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiteSeeking : MonoBehaviour {
	

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

	void Seek() {
		Vector2 directions = PlayerControls.instance.astronaut.position - transform.position;

		directions = directions + PlayerControls.instance.astronaut.GetComponent<Rigidbody2D> ().velocity;

		directions = directions.normalized * seekPower;
		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (directions.x, directions.y));
	}

}

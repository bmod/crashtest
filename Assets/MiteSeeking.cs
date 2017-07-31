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
		CheckForAnimChange ();
		if (PlayerControls.instance.astronaut == null) { //he died
			SeekLastPosition();
			return;
		}

		Seek ();

	}

	bool attackingSet = false;

	void CheckForAnimChange() {
		
		if (InAttackRange () && !attackingSet) {
			Debug.Log ("going to attacking !!!!");
			attackingSet = true;
			GetComponent<Animator> ().SetTrigger ("Attack");
		} else if(attackingSet && !InAttackRange()) {
			attackingSet = false;
			Debug.Log ("going to seeking !!!!");
			GetComponent<Animator> ().SetTrigger ("Seek");
		}
	}

	public float slowOnAttackFactor = .5f;


	bool InAttackRange() {
		if (PlayerControls.instance.astronaut == null) {
			return false;
		}
		bool inr = distanceToChangeToAttacking * distanceToChangeToAttacking >
			(PlayerControls.instance.astronaut.position - new Vector3(transform.position.x, transform.position.y, 0f)).sqrMagnitude;
		return inr;
	}

	public void CheckForOverlapAndDamagePlayer() {
		if (PlayerControls.instance.astronaut == null) //He died.
			return;
		Collider2D player = PlayerControls.instance.astronaut.GetComponent<Collider2D> ();
		if (GetComponent<Collider2D> ().IsTouching (player)) {

			player.GetComponent<Damageable> ().TakeDamage (1, this);
			Rigidbody2D playerRgbd = player.GetComponent<Rigidbody2D> ();
			playerRgbd.velocity = playerRgbd.velocity * (1f - slowOnAttackFactor);

		} else {
			Debug.Log ("Not overlaping!");
		}
	}

	public Vector3 lastPosition;
	public float maxVelocity = 5f;
	void Seek() {
		lastPosition = PlayerControls.instance.astronaut.position;
		Vector2 directions = PlayerControls.instance.astronaut.position - transform.position;

		if (!InAttackRange ()) {
			directions = directions + PlayerControls.instance.astronaut.GetComponent<Rigidbody2D> ().velocity;
		} 


		directions = directions.normalized * seekPower;
		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (directions.x, directions.y));


	}

	void SeekLastPosition() {
		
		Vector2 directions = lastPosition - transform.position;

	

		directions = directions.normalized * seekPower;
		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (directions.x, directions.y));
	}


}

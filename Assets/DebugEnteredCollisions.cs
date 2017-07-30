using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEnteredCollisions : MonoBehaviour {


	void OnCollisionEnter2D(Collision2D other) {
		Debug.Log ("other collided with me: " + other.collider.name);
	}
}

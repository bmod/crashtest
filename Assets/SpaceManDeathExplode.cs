using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceManDeathExplode : MonoBehaviour {


	public float maxSpeedOfParts = 1.5f;
	public float maxAngularSpeedOfParts =  360f;
	// Use this for initialization
	void Start () {
		
		foreach (Rigidbody2D rgbd in GetComponentsInChildren<Rigidbody2D> ()) {
			Vector2 randoVel = Random.insideUnitCircle * maxSpeedOfParts;	
			rgbd.velocity = randoVel;

			rgbd.angularVelocity = Random.Range (-maxAngularSpeedOfParts, maxAngularSpeedOfParts);
		}
	}

}

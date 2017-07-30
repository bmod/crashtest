using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericDeathEffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Damageable>().OnKilled += (DamageSource source) => {
			Destroy(gameObject);
		};
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

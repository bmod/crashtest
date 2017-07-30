using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchOhDamage : MonoBehaviour {

	public Animator animation;
	// Use this for initialization
	void Start () {
		GetComponent<Damageable>().OnDamageTaken += (amount, source) => {
			if(!animation.GetCurrentAnimatorStateInfo(0).IsName("Hit")) { //Should prevent the hit animation from "stacking
				
				animation.SetTrigger("Hit");
			}
		};
	}
	

}

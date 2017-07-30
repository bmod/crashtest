using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericDeathEffect : MonoBehaviour {


	public AudioSource deathAudio;

	// Use this for initialization
	void Start () {
		GetComponent<Damageable>().OnKilled += (DamageSource source) => {
			
			AppearAsDestroyed();


			StartCoroutine(DelayDestroy());	
		};
	}

	void AppearAsDestroyed() {

		DisableAllAssociated<Collider2D>();
		DisableAllAssociated<SpriteRenderer>();
	}

	void DisableAllAssociated<T>() where T : Component {
		T sr = GetComponent<T>();
		if(sr != null) {
			Destroy (sr);

		}

		foreach(T rend in GetComponentsInChildren<T>()) {
			Destroy (rend);

		}
	}



	IEnumerator DelayDestroy() {
		if(deathAudio != null)
			deathAudio.Play();
		yield return new WaitForSeconds (3f);
		Destroy (gameObject);
	}
}

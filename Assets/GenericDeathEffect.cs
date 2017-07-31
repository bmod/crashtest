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

		DisableAllAssociated<Collider2D>(gameObject);
		DisableAllAssociated<SpriteRenderer>(gameObject);
	}

	public static void DisableAllAssociated<T>(GameObject onGameObject) where T : Component {
		T sr = onGameObject.GetComponent<T>();
		if(sr != null) {
			Destroy (sr);

		}

		foreach(T rend in onGameObject.GetComponentsInChildren<T>()) {
			Destroy (rend);

		}
	}

	public GameObject spawnOnDeath;

	IEnumerator DelayDestroy() {
		
		if(deathAudio != null)
			deathAudio.Play();
		if (spawnOnDeath != null) {
			GameObject go = Instantiate (spawnOnDeath);
			go.transform.SetParent (transform.parent);
			go.transform.position = transform.position;
		}

		yield return new WaitForSeconds (3f);
		Destroy (gameObject);
	}
}

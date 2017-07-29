using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour {


	public Transform player;

	public Damageable playerDamageable;

	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find ("SpaceMan");
		if (go != null) {
		

			playerDamageable = go.GetComponent<Damageable> ();
			playerDamageable.OnDamageTaken += ShakeOnDamaged;
		} else {
			Debug.LogError ("Couldn't find the SpaceMan object in camera");
		}

	}

	void OnDestroy() {
		if (playerDamageable != null) {
			playerDamageable.OnDamageTaken -= ShakeOnDamaged;
		}
	}

	public float shakeMagnitude = .2f;
	void ShakeOnDamaged(int amount, Missile source) {
		DoShake (shakeMagnitude, .5f, null);
	}


	private Vector3 currentVelocity;
	// Update is called once per frame
	void Update () {
		var targetPos = new Vector3(player.position.x, player.position.y, transform.position.z) + shakingOffset;
		transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref currentVelocity, 1);
	}




	private Vector3 shakingOffset = Vector3.zero;

	Coroutine lastShakeRoutine;
	public void DoShake(float magnitude, float duration, AnimationCurve curve) {
		if (lastShakeRoutine != null) {
			StopCoroutine (lastShakeRoutine);
		}

		lastShakeRoutine = StartCoroutine (ShakeProcess(magnitude, duration, curve));

	}

	private IEnumerator ShakeProcess(float magnitude, float duration, AnimationCurve curve) {
		if (curve == null) {
			curve = AnimationCurve.EaseInOut (0f, 1f, 1f, 0f);
		}



		float ogOffsetPossible = magnitude / 2f;
		float offsetPossible = ogOffsetPossible;
		//		Debug.Log("doing a magnitude of " + magnitude + " and offset possible is " + offsetPossible);
		float timeRunning = 0f;
		while (timeRunning < duration) {
			offsetPossible = ogOffsetPossible * curve.Evaluate(timeRunning / duration);

			//			Debug.Log("offset possible evaled: " + offsetPossible);

			shakingOffset = new Vector3 (
				Random.Range(-offsetPossible, offsetPossible), 
				Random.Range(-offsetPossible, offsetPossible), 
				Random.Range(-offsetPossible, offsetPossible));

			timeRunning += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}

		shakingOffset = Vector3.zero;
	}

}

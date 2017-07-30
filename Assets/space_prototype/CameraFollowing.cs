using System.Collections;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{

	public float SmoothTime = 0.3f;
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
	void ShakeOnDamaged(int amount, DamageSource source) {
		if (isShaking) {
			return;
		}
		DoShake (shakeMagnitude, .5f, null);
	}


	private Vector3 currentVelocity;
	// Update is called once per frame
	void FixedUpdate () {
		var targetPos = new Vector3(player.position.x, player.position.y, transform.position.z);
		var smoothPos = Vector3.SmoothDamp(transform.position, targetPos, ref currentVelocity, SmoothTime);
		transform.position = smoothPos + shakingOffset;
	}




	private Vector3 shakingOffset = Vector3.zero;

	Coroutine lastShakeRoutine;
	public void DoShake(float magnitude, float duration, AnimationCurve curve) {
		if (lastShakeRoutine != null) {
			StopCoroutine (lastShakeRoutine);
		}

		lastShakeRoutine = StartCoroutine (ShakeProcess(magnitude, duration, curve));

	}

	bool isShaking;
	private IEnumerator ShakeProcess(float magnitude, float duration, AnimationCurve curve) {
		isShaking = true;
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
		isShaking = false;
		shakingOffset = Vector3.zero;
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {


	KeyCode rotateGunCounterClockWise = KeyCode.LeftArrow;
	KeyCode rotateGunClockwise = KeyCode.RightArrow;
	public float gunRotationSpeed = 180f;
	[SerializeField]
	Transform gunRotator;

	KeyCode rotateBoosterCounterClockwise = KeyCode.A;
	KeyCode rotateBoosterClockwise = KeyCode.D;
	public float boosterRotateSpeed = 180f;
	[SerializeField]
	Transform boosterRotator;


	KeyCode gunShotKey = KeyCode.UpArrow;
	public Transform barrelOpening;
	KeyCode boosterKey = KeyCode.W;

	// Update is called once per frame
	void Update () {
		CheckForBoosterRotation ();
		CheckForGunRotation ();
		CheckForShot ();
		CheckForBoost ();
	}

	public GameObject missilePrefab;

	void CheckForShot() {
		if (Input.GetKey (gunShotKey)) {
			Shoot ();
		}
	}

	bool cooling = false;
	float cooldown = .2f;
	public float bulletSpeed = 7f;
	void Shoot() {
		if (cooling)
			return;
		GameObject shot = Instantiate (missilePrefab);
		shot.transform.position = barrelOpening.position;
		Vector3 launchVel = UnitForward2DFromTransform (gunRotator) * bulletSpeed;
		Vector3 starting = boosterRigidbody.GetComponent<Rigidbody2D> ().velocity;
		shot.GetComponent<Rigidbody2D> ().velocity =  launchVel + starting;
		StartCoroutine (Cooldown ());
		StartCoroutine (KillBullet (shot));
	}

	IEnumerator Cooldown() {
		cooling = true;
		yield return new WaitForSeconds (cooldown);
		cooling = false;
	}
	IEnumerator KillBullet(GameObject bullet) {
		yield return new WaitForSeconds (2f);
		Destroy (bullet);
	}

	Vector3 UnitForward2DFromTransform(Transform t) {
		Vector3 fwd = t.localRotation * Vector3.up;
		return fwd;
	}


	public Rigidbody2D boosterRigidbody;
	public float boosterForceMagnitude = 8f;
	void CheckForBoost() {
		if (Input.GetKey (boosterKey)) {
			Vector3 force = UnitForward2DFromTransform (boosterRotator) * boosterForceMagnitude;
			boosterRigidbody.AddForce (new Vector2 (force.x, force.y));
		}
	}

	void CheckForBoosterRotation() {
		if (Input.GetKey (rotateBoosterClockwise)) {
			//			Debug.Log ("rotattng!");
			boosterRotator.Rotate (new Vector3 (0f, 0f, -(Time.deltaTime * boosterRotateSpeed)));

		}

		if (Input.GetKey (rotateBoosterCounterClockwise)) {
			//			Debug.Log ("rotattng!");
			boosterRotator.Rotate (new Vector3 (0f, 0f, Time.deltaTime * boosterRotateSpeed));
		}

	}



	void CheckForGunRotation() {
		if (Input.GetKey (rotateGunClockwise)) {
			//			Debug.Log ("rotattng!");
			gunRotator.Rotate (new Vector3 (0f, 0f, -(Time.deltaTime * gunRotationSpeed)));
		}
		if (Input.GetKey (rotateGunCounterClockWise)) {
			//			Debug.Log ("rotattng!");
			gunRotator.Rotate (new Vector3 (0f, 0f, Time.deltaTime * gunRotationSpeed));
		}
	}
}

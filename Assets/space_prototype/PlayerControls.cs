using System.Collections;
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

	public Transform astronaut;


	public ParticleSystem boosterParticles;

	KeyCode gunShotKey = KeyCode.UpArrow;
	public Transform barrelOpening;
	KeyCode boosterKey = KeyCode.W;

	// Update is called once per frame
	void Update () {
		CheckForBoosterRotation ();
		CheckForGunRotation ();
		CheckForShot ();
		CheckForBoost ();
		// Gamepad controls can be used at the same time.

//		CheckForBoosterRotationGamePad();
//		CheckForGunRotationGamePad();
//		CheckForShotGamePad();
	}

	public GameObject missilePrefab;

	void CheckForShot() {
		if (Input.GetKey (gunShotKey)) {
			Shoot ();
		}
	}

	void CheckForShotGamePad()
	{
		if (Input.GetAxis("Fire1") < -0.2)
			Shoot();
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

	void Start() {
		particleEmission = boosterParticles.emission;
	}


	public Rigidbody2D boosterRigidbody;
	ParticleSystem.EmissionModule particleEmission;
	public float boosterForceMagnitude = 8f;

	[SerializeField]
	bool boosting = false;
	void CheckForBoost() {
		if (Input.GetKey (boosterKey)) {
			boosting = true;
			Vector3 force = UnitForward2DFromTransform (boosterRotator) * boosterForceMagnitude;
			boosterRigidbody.AddForce (new Vector2 (force.x, force.y));
		} else {
			boosting = false;
		}

		if (boosting) {
			Debug.Log ("doing boosting!");
			particleEmission.enabled = true;
		} else {
			Debug.Log ("nNOT boosting");
			particleEmission.enabled = false;
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

	void CheckForBoosterRotationGamePad()
	{
		var leftSticKVec = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
		var inputMagnitude = leftSticKVec.magnitude;
		if (inputMagnitude > 0.2f)
		{
			var currentAngle = boosterRotator.eulerAngles.z;
			var targetAngle = Mathf.Atan2(leftSticKVec.y, leftSticKVec.x) * Mathf.Rad2Deg - 90;
			var newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, Time.deltaTime * boosterRotateSpeed);
			boosterRotator.rotation = Quaternion.Euler(0, 0, newAngle);

			var force = UnitForward2DFromTransform(boosterRotator) * boosterForceMagnitude * inputMagnitude;
			boosterRigidbody.AddForce(force);
			particleEmission.enabled = true;
		}
		else
		{
			particleEmission.enabled = false;
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

	void CheckForGunRotationGamePad()
	{
		var rightStickVec = new Vector3(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"), 0);
		var inputMagnitude = rightStickVec.magnitude;
		if (inputMagnitude > 0.2f)
		{
			var currentAngle = gunRotator.eulerAngles.z;
			var targetAngle = Mathf.Atan2(rightStickVec.y, rightStickVec.x)* Mathf.Rad2Deg - 90;
			var newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, Time.deltaTime * gunRotationSpeed);
			gunRotator.rotation = Quaternion.Euler(0, 0, newAngle );
		}
		
	}

	public static PlayerControls instance;
	void Awake() {
		if (instance != null) {
			Debug.LogError ("Warning: had to destroy an extraneous instance of player controls.");
			Destroy (this);
		} else {
			instance = this;
		}
	}
}

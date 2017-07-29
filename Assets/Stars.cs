using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour {

	public GameObject starPrefab;

	public int howMany = 100;
	public float separation = 1f;
	public float randomPlaceFuzz = .5f;



	// Use this for initialization
	void Start () {

		float howManySqrRoot = Mathf.Sqrt ((float)howMany);
		float halfSqrRoot = howManySqrRoot / 2f;
		Vector3 runningPos = transform.position - new Vector3 (
			halfSqrRoot * separation,
			halfSqrRoot * separation,
			transform.position.z
		);

		for (int i = 0; i < howManySqrRoot; i++) {
			for (int j = 0; j < howManySqrRoot; j++) {
				GameObject star = Instantiate (starPrefab);
				star.transform.position = runningPos + RandomOffset (randomPlaceFuzz);
//				float randScale = Random.Range (-.5f, .5f);
				star.transform.localScale = new Vector3(
					star.transform.localScale.x,
					star.transform.localScale.y,
					transform.localScale.z);
				
				runningPos = runningPos + new Vector3 (separation, 0f, 0f);
			}
			runningPos = runningPos + new Vector3 (-(separation * howManySqrRoot), 0f, 0f);
			runningPos = runningPos + new Vector3 (0f, separation, 0f);
		}
	}

	Vector3 RandomOffset(float fuzz) {
		Vector3 randomOff = new Vector3 (Random.Range(-fuzz, fuzz), Random.Range(-fuzz, fuzz), 0f);
		return randomOff;
	}

	bool CoinFlipIsHeads() {
		return Random.Range (0f, 1f) < .5f;
	}

	// Update is called once per frame
	void Update () {
		
	}
}

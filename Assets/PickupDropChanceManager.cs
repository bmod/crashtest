using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PickupDropChanceManager : MonoBehaviour {

	public static PickupDropChanceManager instance;
	public int score;


	public string[] exclusionsByTag;

	void Awake() {
		if (instance != null) {
			Destroy (this);

//			Debug.LogError ("Had to destroy dup " + GetType().ToString());
		} else {
			score = 0;
			SetNext ();
			instance = this;	
		}
	}

	int spawnpatternIndex = 0;
	public GameObject[] pickupSpawnPattern;


	//This could scale up with difficulty
	public int baseFrequency = 8;
	public int fudgeFactor = 2;
	[SerializeField]
	private int nextIn;

	void SetNext() {
		//set the next threshold
		nextIn = baseFrequency +  Random.Range (-fudgeFactor, fudgeFactor);

		//rotate index
		spawnpatternIndex = (spawnpatternIndex + 1) % pickupSpawnPattern.Length;
	}

	public void PlayerShotSomethingDown(Damageable thing) {
		score++;

		if(exclusionsByTag.ToList().Contains(thing.tag)) {
			return;
		}

		nextIn--;

		if (nextIn <= 0) {
			DoPickupSpawn (thing.transform.position);
		}
	}

	void DoPickupSpawn(Vector3 loc) {
		GameObject prefab = pickupSpawnPattern [spawnpatternIndex];
		GameObject newPickup =  Instantiate (prefab);
		newPickup.transform.position = loc;
		SetNext ();
	}

}

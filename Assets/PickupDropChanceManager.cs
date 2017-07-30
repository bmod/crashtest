using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupDropChanceManager : MonoBehaviour {

	public static PickupDropChanceManager instance;


	void Awake() {
		if (instance != null) {
			Destroy (this);
			Debug.LogError ("Had to destroy dup " + GetType().ToString());
		} else {
			
			SetNext ();
			instance = this;	
		}



	}

	int spawnpatternIndex = 0;
	public GameObject[] pickupSpawnPattern;


	//This could scale up with difficulty
	public int baseFrequency = 15;
	public int fudgeFactor = 3;
	[SerializeField]
	private int nextIn;

	void SetNext() {
		//set the next threshold
		nextIn = baseFrequency +  Random.Range (-fudgeFactor, fudgeFactor);

		//rotate index
		spawnpatternIndex = (spawnpatternIndex + 1) % pickupSpawnPattern.Length;
	}

	public void PlayerShotSomethingDown(Damageable thing) {
		Debug.Log ("Player shot down: " + thing.name);	
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

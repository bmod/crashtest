using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiteHatchery : MonoBehaviour {


	public  GameObject[] mitePrefabsPossible; 
	public Transform[] possibleSpawnLocations;

	public float spawnDelay = 15f;
	public int spawnsPerWave = 1;


	public bool playerInVicinity = false;
	// Use this for initialization
	void Start () {
		StartCoroutine (ConditionalSpawn());
	}



	IEnumerator ConditionalSpawn() {
		yield return new WaitForSeconds (spawnDelay);

		if (playerInVicinity) {
			for (int i = 0; i < spawnsPerWave; i++) {
				GameObject mite = Instantiate (mitePrefabsPossible [Random.Range (0, mitePrefabsPossible.Length)]); //random mite prefab
				mite.transform.position = possibleSpawnLocations [Random.Range (0, possibleSpawnLocations.Length)].position;
			}

		}


		StartCoroutine (ConditionalSpawn ());

	}




	

}

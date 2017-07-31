using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerRunningOut : MonoBehaviour {

	[SerializeField]
	float power;
	public float maxPower = 50f;

	// Use this for initialization
	void Start () {

		power = maxPower;
		StartCoroutine (DecreasePower());
	}

	public void AddPower(float amount)
	{
		power = Mathf.Min(power + amount, maxPower);

		if (OnGainedPower != null) {
			OnGainedPower (amount);
		}
	}

	IEnumerator DecreasePower() {
		yield return new WaitForSeconds (1f);
		power -= 1f;

		if (OnLostPower != null) {
			OnLostPower (1f);
		}


		if (power < 0f) {
			GameOver ();
		}
		StartCoroutine (DecreasePower ());
	}

	public delegate void LostPowerAction (float amount);
	public event LostPowerAction OnLostPower;


	public delegate void GainedPowerAction (float amount);
	public event GainedPowerAction OnGainedPower;

	void GameOver() {
		UIManager.instance.PresentGameOver ();

	}


	public float PowerRatio {
		get{ 
			if (power < 0f) {
				return 0f;
			}
			return power / maxPower;
		}
	}

}

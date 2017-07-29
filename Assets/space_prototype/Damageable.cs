using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {

	[SerializeField]
	int currentHealth; 
	public int maxHealth = 4;

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
	}

	public void TakeDamage(int amount, Missile source) {

		currentHealth -= amount;

		if (OnDamageTaken != null) {
			OnDamageTaken (amount, source);
		}

		if (currentHealth <= 0) {
			if (OnKilled != null) {
				OnKilled (source);
			}
		}
	}

	public void AddHealth(int h)
	{
		currentHealth = Mathf.Min(currentHealth+h, maxHealth);
		print("Omnom!");
	}

	public float HealthRatio {
		get {
			if (currentHealth < 0) {
				return 0f;

			}

			return currentHealth / (float) maxHealth;
		}
	}

	public delegate void DamageTakenAction(int amount, Missile source);
	public event DamageTakenAction OnDamageTaken;

	public delegate void KilledAction(Missile source);
	public event KilledAction OnKilled;

}

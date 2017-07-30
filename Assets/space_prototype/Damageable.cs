using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {

	[SerializeField]
	int currentHealth; 
	public int maxHealth = 4;
	public AudioSource damagedAudio;



	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
	}

	public void TakeDamage(int amount, DamageSource source) {

		currentHealth -= amount;

		if (OnDamageTaken != null) {
			OnDamageTaken (amount, source);
		}

		if (currentHealth <= 0) {
			if (OnKilled != null) {
				OnKilled (source);
			}

			if (source is Missile) {
				Missile missile = source as Missile;
				if (missile.launcher is PlayerControls) {
					PickupDropChanceManager.instance.PlayerShotSomethingDown (this);
				}
			}

		}


		if (damagedAudio != null) {
			damagedAudio.Play ();
		}
			

	}

	public void AddHealth(int h)
	{
		currentHealth = Mathf.Min(currentHealth+h, maxHealth);
		print("Omnom!");
		if (OnHealed != null) {
			OnHealed (h);
		}

	}

	public float HealthRatio {
		get {
			if (currentHealth < 0) {
				return 0f;

			}

			return currentHealth / (float) maxHealth;
		}
	}

	public delegate void DamageTakenAction(int amount, DamageSource source);
	public event DamageTakenAction OnDamageTaken;


	public delegate void HealedAction(int amount);
	public event HealedAction OnHealed;

	public delegate void KilledAction(DamageSource source);
	public event KilledAction OnKilled;

}

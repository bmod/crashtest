using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    public enum Type
    {
        Health,
        Oxygen,
        Battery
    }

	public AudioSource powerUpSound;

    public Type type = Type.Health;
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {

		if (!other.CompareTag ("Player")) {
			return;
		}

        switch (type)
        {
		case Type.Battery:
				var pow = PlayerControls.instance.GetComponentInChildren<PowerRunningOut> ();
                pow.AddPower(amount);
                break;
            case Type.Health:
				var damageable = PlayerControls.instance.GetComponentInChildren<Damageable> ();
                damageable.AddHealth(amount);
                break;
            case Type.Oxygen:
                break;
        }
		StartCoroutine (DelayDeath ());
    }

	IEnumerator DelayDeath() {
		GenericDeathEffect.DisableAllAssociated<SpriteRenderer> (gameObject);
		GenericDeathEffect.DisableAllAssociated<Collider2D> (gameObject);
		powerUpSound.Play ();
		yield return new WaitForSeconds (3f);
		Destroy (gameObject);
	}
}
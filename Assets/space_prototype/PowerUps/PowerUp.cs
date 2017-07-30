using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Type
    {
        Health,
        Oxygen,
        Battery
    }

    public Type type = Type.Health;
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (type)
        {
            case Type.Battery:
                if (other.gameObject.name != "Drone")
                    return;
                var pow = other.gameObject.transform.parent.GetComponentInChildren<PowerRunningOut>();
                pow.AddPower(amount);
                Destroy(gameObject);
                break;
            case Type.Health:
                var damageable = other.GetComponent<Damageable>();
                if (damageable == null)
                    return;
                damageable.AddHealth(amount);
                Destroy(gameObject);
                break;
            case Type.Oxygen:
                break;
        }
    }
}
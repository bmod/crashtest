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
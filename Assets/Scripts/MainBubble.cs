using UnityEngine;

public class MainBubble : MonoBehaviour, IDamageable
{
    [SerializeField] public int health;
    [SerializeField] private ScoreProgress _healthProgress;

    const int MaxDamage = 10000000;

    private bool _isDead = false;

    private void Start() 
    {
        _healthProgress.Change(health);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        _healthProgress.Change(health);
        if (health <= 0)
        {
            Destroy(gameObject);
            ActionBus.MainBubbleKilled?.Invoke();
            print("Died");
            _isDead = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IKillable component))
        {
            TakeDamage(component.GetDamage());

            if (!_isDead && other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(MaxDamage);
            }

        }
    }
}

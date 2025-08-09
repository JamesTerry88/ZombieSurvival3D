using UnityEngine;
using UnityEngine.Events;

public class ZombieStats : MonoBehaviour
{
    [Header("Runtime Stats (set on spawn)")]
    public int maxHealth;
    public int currentHealth;
    public int damage;

    [Header("Events")]
    public UnityEvent OnDeath = new UnityEvent();

    public void Initialize(int health, int dmg)
    {
        maxHealth = health;
        currentHealth = health;
        damage = dmg;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        OnDeath.Invoke();
        Destroy(gameObject);
    }
}

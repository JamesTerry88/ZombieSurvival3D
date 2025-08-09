using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    float startTime;

    void Start()
    {
        currentHealth = maxHealth;
        startTime = Time.time;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        float surv = Time.time - startTime;
        int minutes = (int)(surv / 60f);
        float seconds = surv % 60f;
        Debug.Log($"Player survived for {minutes}:{seconds:00.00}");

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

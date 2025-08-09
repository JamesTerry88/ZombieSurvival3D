using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [Header("Refs")]
    public ZombieSpawner spawner;

    [Header("Round State")]
    public int currentRound = 0;
    public bool autoStart = true;

    [Header("Count Scaling")]
    public int baseZombies = 5;
    public int zombiesPerRoundAdd = 3;

    [Header("Health Scaling")]
    public int baseHealth = 50;
    public float healthGrowth = 1.25f;

    [Header("Damage Scaling")]
    public int baseDamage = 5;
    public float damageGrowth = 1.15f;

    void Start()
    {
        if (autoStart) StartNextRound();
    }

    void Update()
    {
        if (spawner == null) return;
        if (currentRound > 0 && spawner.AliveCount == 0)
            StartNextRound();
    }

    public void StartNextRound()
    {
        if (spawner == null) return;

        currentRound++;

        int count = Mathf.Max(1, baseZombies + (currentRound - 1) * zombiesPerRoundAdd);
        int hp    = Mathf.RoundToInt(baseHealth * Mathf.Pow(healthGrowth, currentRound - 1));
        int dmg   = Mathf.RoundToInt(baseDamage * Mathf.Pow(damageGrowth, currentRound - 1));

        spawner.SpawnWave(count, hp, dmg);
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject zombiePrefab;
    public Transform[] spawnPoints;      // leave empty to auto-collect children
    public Transform player;

    readonly List<ZombieStats> alive = new List<ZombieStats>();
    public int AliveCount => alive.Count;

    void Awake()
    {
        // Auto-collect child spawn points if none assigned
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            var list = new List<Transform>();
            foreach (Transform t in transform) list.Add(t);
            spawnPoints = list.ToArray();
        }
    }

    public void SpawnWave(int count, int healthPerZombie, int damagePerZombie)
    {
        if (!zombiePrefab || !player || spawnPoints == null || spawnPoints.Length == 0) return;

        for (int i = 0; i < count; i++)
        {
            Transform p = spawnPoints[Random.Range(0, spawnPoints.Length)];
            if (!p) continue;

            GameObject z = Instantiate(zombiePrefab, p.position, p.rotation);
            if (!z) continue;

            // Ensure agent + snap to NavMesh
            var agent = z.GetComponent<NavMeshAgent>() ?? z.AddComponent<NavMeshAgent>();
            if (NavMesh.SamplePosition(z.transform.position, out var hit, 5f, NavMesh.AllAreas))
                agent.Warp(hit.position);

            // Stats
            var stats = z.GetComponent<ZombieStats>() ?? z.AddComponent<ZombieStats>();
            stats.Initialize(healthPerZombie, damagePerZombie);
            stats.OnDeath.AddListener(() => alive.Remove(stats));
            alive.Add(stats);

            // AI target
            var ai = z.GetComponent<ZombieAI>() ?? z.AddComponent<ZombieAI>();
            ai.player = player;
        }
    }

    public void ClearAll()
    {
        for (int i = alive.Count - 1; i >= 0; i--)
            if (alive[i]) Destroy(alive[i].gameObject);
        alive.Clear();
    }
}

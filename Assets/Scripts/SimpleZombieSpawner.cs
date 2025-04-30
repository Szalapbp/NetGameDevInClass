using System.Collections; // Required for IEnumerator
using UnityEngine;

public class SimpleZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab; // Assign your zombie prefab
    public Transform[] spawnPoints; // Spawn point transforms
    public float spawnInterval = 2f; // Interval between spawns

    private void Start()
    {
        // Start the spawning coroutine
        StartCoroutine(SpawnZombieCoroutine());
    }

    private IEnumerator SpawnZombieCoroutine()
    {
        while (true)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                // Spawn the zombie at the spawn point
                Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);
                Debug.Log($"Zombie spawned at {spawnPoint.position}");
            }

            // Wait for the next spawn interval
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
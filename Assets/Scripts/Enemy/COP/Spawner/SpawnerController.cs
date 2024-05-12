using System.Collections;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab of the enemy to spawn
    private GameObject spawnedEnemy; // Reference to the spawned enemy GameObject
    private bool shouldSpawn = true; // Flag to control spawning
    private bool enemySpawned = false; // Flag to track if an enemy has been spawned
    private float respawnTimer = 30f; // Maximum time before respawning a new enemy
    private float currentTimer = 0f; // Timer to track time since last enemy spawned

    public enum SpawnPoint { Spawn1, Spawn2 }; // Enum to define spawn points
    public SpawnPoint spawnPoint; // Selected spawn point

    void Start()
    {
        StartCoroutine(SpawnEnemyWithRandomDelay()); // Start spawning enemies with random delay
    }

    IEnumerator SpawnEnemyWithRandomDelay()
    {
        while (shouldSpawn)
        {
            // Only attempt to spawn an enemy if there isn't one already in the scene
            if (!enemySpawned)
            {
                SpawnEnemy(); // Spawn an enemy
            }

            // Wait for a random time before attempting to spawn another enemy
            float randomDelay = Random.Range(1.0f, 5.0f); // Random delay between 1 and 5 seconds
            yield return new WaitForSeconds(randomDelay);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("Enemy Prefab not assigned in SpawnerController.");
            return;
        }

        // Only spawn a new enemy if there isn't one already in the scene
        if (spawnedEnemy == null)
        {
            spawnedEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity); // Instantiate enemy
            CopController enemyMovement = spawnedEnemy.GetComponent<CopController>(); // Get enemy movement component

            switch (spawnPoint)
            {
                case SpawnPoint.Spawn1:
                    enemyMovement.FaceLeft(); // Face left if spawn point is Spawn1
                    break;
                case SpawnPoint.Spawn2:
                    enemyMovement.FaceRight(); // Face right if spawn point is Spawn2
                    break;
                default:
                    Debug.LogWarning("Invalid spawn point.");
                    break;
            }

            // Save reference to the current spawner in the spawned enemy
            spawnedEnemy.GetComponent<CopController>().spawner = this;
            enemySpawned = true; // Mark that an enemy has been spawned
            currentTimer = 0f; // Reset the timer when a new enemy is spawned
        }
    }

    void Update()
    {
        // Increment the timer if an enemy is in the scene
        if (enemySpawned)
        {
            currentTimer += Time.deltaTime;
            // If the timer exceeds the limit, respawn a new enemy
            if (currentTimer >= respawnTimer)
            {
                RespawnEnemy();
            }
        }
    }

    void RespawnEnemy()
    {
        spawnedEnemy = null; // Clear reference to the spawned enemy
        enemySpawned = false; // Mark that no enemy is currently spawned
        SpawnEnemy(); // Spawn a new enemy
    }

    // Method called by the enemy when destroyed to indicate that another can spawn
    public void EnemyDestroyed()
    {
        enemySpawned = false; // Mark that no enemy is currently spawned
    }

    // Method to stop spawning enemies
    public void StopSpawning()
    {
        shouldSpawn = false; // Disable spawning
    }
}

using System.Collections;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject enemyPrefab;
    private GameObject spawnedEnemy;
    private bool shouldSpawn = true;
    private bool enemySpawned = false;
    private float respawnTimer = 30f; // Tiempo máximo antes de respawnear un nuevo enemigo
    private float currentTimer = 0f;

    public enum SpawnPoint { Spawn1, Spawn2 };
    public SpawnPoint spawnPoint;

    void Start()
    {
        StartCoroutine(SpawnEnemyWithRandomDelay());
    }

    IEnumerator SpawnEnemyWithRandomDelay()
    {
        while (shouldSpawn)
        {
            // Solo intenta generar un enemigo si no hay uno ya en el escenario
            if (!enemySpawned)
            {
                SpawnEnemy();
            }

            // Espera un tiempo aleatorio antes de intentar spawnear otro enemigo
            float randomDelay = Random.Range(1.0f, 5.0f); // Tiempo de espera aleatorio entre 1 y 5 segundos
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

        // Solo spawnear un nuevo enemigo si no hay uno ya en el escenario
        if (spawnedEnemy == null)
        {
            spawnedEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            CopController enemyMovement = spawnedEnemy.GetComponent<CopController>();

            switch (spawnPoint)
            {
                case SpawnPoint.Spawn1:
                    enemyMovement.FaceLeft();
                    break;
                case SpawnPoint.Spawn2:
                    enemyMovement.FaceRight();
                    break;
                default:
                    Debug.LogWarning("Invalid spawn point.");
                    break;
            }

            // Guardar referencia al Spawner actual
            spawnedEnemy.GetComponent<CopController>().spawner = this;
            enemySpawned = true; // Marcar que se ha spawnado un enemigo
            currentTimer = 0f; // Reiniciar el temporizador cuando se spawnée un nuevo enemigo
        }
    }

    void Update()
    {
        // Incrementar el temporizador si hay un enemigo en el escenario
        if (enemySpawned)
        {
            currentTimer += Time.deltaTime;
            // Si el temporizador excede el límite, respawnear un nuevo enemigo
            if (currentTimer >= respawnTimer)
            {
                RespawnEnemy();
            }
        }
    }

    void RespawnEnemy()
    {
        spawnedEnemy = null;
        enemySpawned = false;
        SpawnEnemy(); // Spawnear un nuevo enemigo
    }

    // Método que el enemigo llama cuando es destruido para indicar que puede spawnear otro
    public void EnemyDestroyed()
    {
        enemySpawned = false;
    }

    // Método para detener el spawner
    public void StopSpawning()
    {
        shouldSpawn = false;
    }
}

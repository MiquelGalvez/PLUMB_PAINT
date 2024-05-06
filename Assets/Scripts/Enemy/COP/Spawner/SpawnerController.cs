using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo a spawnear
    private GameObject spawnedEnemy; // Referencia al enemigo actualmente en la escena

    // Start is called before the first frame update
    void Start()
    {
        // Spawnear el primer enemigo
        SpawnEnemy();
    }

    // M�todo para spawnear un enemigo
    void SpawnEnemy()
    {
        // Spawnear el enemigo en la posici�n del Spawner
        spawnedEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        // Obtener el script de movimiento del enemigo
        CopController enemyMovement = spawnedEnemy.GetComponent<CopController>();
        if (enemyMovement != null)
        {
            // Determinar la direcci�n mirando la etiqueta
            if (this.CompareTag("Spawn1"))
            {
                enemyMovement.FaceRight();
            }
            else if (this.CompareTag("Spawn2"))
            {
                enemyMovement.FaceLeft();
            }
            else
            {
                Debug.LogWarning("Prefab de enemigo sin etiqueta de direcci�n.");
            }
        }
    }

    // M�todo para eliminar el enemigo
    public void DestroyEnemy()
    {
        if (spawnedEnemy != null)
        {
            Destroy(spawnedEnemy);
        }
    }
}

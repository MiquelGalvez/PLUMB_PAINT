using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCar : MonoBehaviour
{
    [SerializeField] private GameObject prefab; // The prefab you want to instantiate
    private float minTime = 4f; // Minimum time between each instance
    private float maxTime = 10f; // Maximum time between each instance

    void Start()
    {
        // Initialize the time for the first instance
        Invoke("SpawnPrefab", Random.Range(minTime, maxTime));
    }

    // Function to spawn the prefab
    void SpawnPrefab()
    {
        // Instantiate the prefab at the position of the SpawnPoint
        GameObject car = Instantiate(prefab, transform.position, Quaternion.identity);
        // Get the car movement script
        CarMovement carMovement = car.GetComponent<CarMovement>();
        // Set the speed of the car
        carMovement.SetSpeed(7f);

        // Call the SpawnPrefab method again after a random time
        Invoke("SpawnPrefab", Random.Range(minTime, maxTime));
    }
}

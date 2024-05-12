using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtackBoss : MonoBehaviour
{
    [SerializeField] GameObject prefab; // Prefab to instantiate
    private float minTime = 5f; // Minimum time between instances
    private float maxTime = 10f; // Maximum time between instances
    private float movementSpeed = 3f; // Movement speed of the instantiated prefab to the left

    private float nextInstanceTime; // Time for the next instance

    void Start()
    {
        // Initialize the time for the first instance
        nextInstanceTime = Time.time + Random.Range(minTime, maxTime);
    }

    void Update()
    {
        // Check if it's time to generate a new instance
        if (Time.time >= nextInstanceTime)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, 180); // Rotation of the instantiated object (180 degrees)
            // Instantiate the prefab
            GameObject newInstance = Instantiate(prefab, transform.position, rotation);
            // Apply leftward movement
            newInstance.GetComponent<Rigidbody2D>().velocity = Vector2.left * movementSpeed;
            // Update the time for the next instance
            nextInstanceTime = Time.time + Random.Range(minTime, maxTime);
        }
    }
}

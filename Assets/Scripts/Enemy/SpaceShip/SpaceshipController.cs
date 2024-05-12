using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipController : MonoBehaviour
{
    [SerializeField] float speed = 5f; // Movement speed
    private bool isFacingRight = true;

    private int direction = 1; // Initial movement direction: 1 (to the right)
    [SerializeField] Image fillImage; // Reference to the fill image for displaying health
    private EnemyHealthController enemyHealthController; // Reference to the enemy health controller

    [SerializeField] GameObject bulletPrefab; // Prefab of the bullet
    [SerializeField] Transform spawnPoint; // Spawn point of the bullet
    [SerializeField] float minShootInterval = 1f; // Minimum interval between shots
    [SerializeField] float maxShootInterval = 2f; // Maximum interval between shots

    private float shootTimer = 0f; // Shot timer
    private float currentShootInterval; // Current interval between shots

    void Start()
    {
        enemyHealthController = GetComponent<EnemyHealthController>(); // Get the enemy health controller component
        currentShootInterval = Random.Range(minShootInterval, maxShootInterval); // Initialize shoot interval
    }

    // Update is called once per frame
    void Update()
    {
        // Automatic horizontal movement
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);

        // Get the camera's edge in the world
        float screenEdgeX = Camera.main.ViewportToWorldPoint(new Vector2(1, 0)).x;

        // If the spaceship reaches the camera's edge, change direction
        if (transform.position.x >= screenEdgeX)
        {
            direction = -1; // Change direction to the left
            if (isFacingRight)
            {
                Flip(); // Flip the spaceship
            }
        }
        else if (transform.position.x <= -screenEdgeX)
        {
            direction = 1; // Change direction to the right
            if (!isFacingRight)
            {
                Flip(); // Flip the spaceship
            }
        }

        // Update shoot timer
        shootTimer += Time.deltaTime;

        // If the timer exceeds the current shoot interval, shoot
        if (shootTimer >= currentShootInterval)
        {
            Shoot(); // Perform a shoot
            currentShootInterval = Random.Range(minShootInterval, maxShootInterval); // Update shoot interval
            shootTimer = 0f; // Reset shoot timer
        }
    }

    // Method to perform a shoot
    void Shoot()
    {
        // Instantiate the bullet at the spawn point
        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);

        // Move the bullet downwards
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.down * 1.5f; // Move downwards

        // Play any sound effects, animations, etc., here if necessary
    }

    // Method to flip the spaceship
    void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player Shoot"))
        {
            float fillAmount = 0.02f; // Amount to increase fill image by
            fillImage.fillAmount += fillAmount; // Increase fill image fill amount
            Destroy(other.gameObject); // Destroy the player shoot object
            enemyHealthController.TakeDamage(1); // Damage the spaceship
        }
        if (other.CompareTag("UltimateShoot"))
        {
            float fillAmount = 0.09f; // Amount to increase fill image by
            fillImage.fillAmount += fillAmount; // Increase fill image fill amount
            enemyHealthController.TakeDamage(15); // Damage the spaceship with ultimate shoot
        }
    }
}

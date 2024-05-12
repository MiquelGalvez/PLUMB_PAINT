using UnityEngine;

public class ShootingController : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab of the bullet that the ShootingController will shoot
    public Transform shootPoint; // Point from which bullets will be shot
    public float bulletSpeed = 10f; // Speed at which the bullet will move
    public float fireRate = 1f; // Firing rate in seconds
    public float bulletDestroyTime = 2f; // Time in seconds before destroying the bullet

    private Transform player; // Reference to the player's transform
    private float lastShootTime; // Time when the last shot was fired
    private Animator animator; // Reference to the Animator component

    void Start()
    {
        // Find the player GameObject at the start
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Get the Animator component
        animator = GetComponent<Animator>();
        // Initialize the last shoot time at the start of the game
        lastShootTime = -fireRate;
    }

    void Update()
    {
        // Check if the player is within the field of view and the PlayerDetect animation is active
        if (Vector2.Distance(transform.position, player.position) < 10f && animator.GetBool("PlayerDetect"))
        {
            // Check if enough time has passed since the last shot
            if (Time.time - lastShootTime > fireRate)
            {
                // Perform the shot
                Shoot();
                // Update the last shoot time
                lastShootTime = Time.time;
            }
        }
    }

    void Shoot()
    {
        // Determine the shooting direction based on the enemy's rotation
        Vector2 direction = Vector2.right; // By default, shoot to the right
        if (transform.rotation.eulerAngles.y == 180f) // If the rotation is -180 degrees
        {
            direction = Vector2.left; // Shoot to the left
        }

        // Instantiate a new bullet
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        // Get the Rigidbody2D component of the instantiated bullet and assign velocity
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;

        // Destroy the bullet after a certain time
        Destroy(bullet, bulletDestroyTime);
    }
}

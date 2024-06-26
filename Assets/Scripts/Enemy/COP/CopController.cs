using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CopController : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform spawnPoint2;
    [SerializeField] float minShootInterval = 1f;
    [SerializeField] float maxShootInterval = 2f;
    [SerializeField] float bulletSpeedRight = 10f;
    [SerializeField] float bulletSpeedDown = 5f;
    [SerializeField] float shootDuration = 0.5f;
    [SerializeField] float maxBulletDistance = 10f;
    private Image fillImage;
    private float movementSpeed = 2f;
    private EnemyHealthController enemyHealthController;
    public SpawnerController spawner; // Reference to the SpawnerController

    private SpriteRenderer copRenderer;
    [SerializeField] Transform playerTransform; // Reference to the player's transform
    private float stopDistanceFromPlayer = 3f; // Distance at which the cop should stop
    private float jumpForce = 5f; // Adjust as needed
    private float jumpMovementSpeed = 2f; // Adjust as needed
    bool hasJumped = false; // Variable to track if the character has jumped
    private AudioSource audioSource;

    Camera mainCamera; // Reference to the main camera

    [SerializeField] LayerMask groundLayerMask; // Layer representing the ground
    [SerializeField] float groundCheckDistance = 0.5f; // Distance to check for ground
    private bool isGrounded = false; // Variable to indicate if the cop is grounded
    private Rigidbody2D rb;
    private bool facingLeft = false;
    private Animator copAnimator;
    private GameObject player;
    private float strongerGravityForce = 10f;
    private Vector2 gravity = new Vector2(0, -5f); // Ajusta este valor seg�n sea necesario

    private SpawnerController spawnerController;
    private float edgeCheckDistance = 0.2f; // Distance to check for platform edge

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        copAnimator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("ToGo").transform;
        copRenderer = GetComponent<SpriteRenderer>();
        enemyHealthController = GetComponent<EnemyHealthController>();
        fillImage = GameObject.FindGameObjectWithTag("Ultimate").GetComponent<Image>();
        spawnerController = FindAnyObjectByType<SpawnerController>();

        // Obt�n la referencia al componente AudioSource
        audioSource = GetComponent<AudioSource>();

        mainCamera = Camera.main; // Assign the main camera reference
    }

    void Update()
    {
        // Check if the cop is outside the camera's view
        Vector2 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            Destroy(gameObject); // Destroy the cop if it's outside the camera's view
            spawnerController.EnemyDestroyed();
        }
    }

    void FixedUpdate()
    {
        copAnimator.SetBool("Idle", false);

        Vector2 rayDirection = new Vector2(1, -1).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, groundCheckDistance, groundLayerMask);
        bool isGrounded = hit.collider != null;
        Debug.DrawRay(transform.position, rayDirection * groundCheckDistance, Color.red);
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // Move the cop if it's grounded and not too close to the player
        if (isGrounded && distanceToPlayer > stopDistanceFromPlayer)
        {
            transform.Translate(Vector2.right * movementSpeed * Time.deltaTime);
            hasJumped = false;

            // Check if there's a platform edge to jump
            RaycastHit2D edgeHit = Physics2D.Raycast(transform.position + Vector3.right * edgeCheckDistance, Vector2.down, groundCheckDistance, groundLayerMask);
            if (!edgeHit)
            {
                // No platform edge detected, flip and start moving left
                Flip();
                FaceLeft();
            }
        }
        else if (!isGrounded)
        {
            // If not grounded, apply a jump force and move forward
            if (!hasJumped)
            {
                rb = GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                hasJumped = true;
            }

            // If the cop is within the stop distance from the player, apply stronger downward force
            if (distanceToPlayer <= stopDistanceFromPlayer)
            {
                // Apply stronger gravity
                rb.AddForce(Vector2.down * strongerGravityForce, ForceMode2D.Force);
            }
            else
            {
                // Apply normal gravity
                rb.velocity += gravity * Time.deltaTime;
            }

            // Smooth horizontal movement during jump
            float horizontalVelocity = Mathf.Lerp(rb.velocity.x, movementSpeed, 0.5f);
            rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);
        }

        if (distanceToPlayer <= stopDistanceFromPlayer)
        {
            rb.velocity = Vector2.zero;
            copAnimator.SetBool("Idle", true);
        }
    }

    // Function asignated to a fram of the animation
    void Shoot()
    {
        // Obtener la direcci�n hacia el jugador
        Vector2 direction = (player.transform.position - spawnPoint.position).normalized;
        // Shoot to the right
        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (direction.x < 0)
        {
            FaceLeft();
            rb.velocity = Vector2.left * bulletSpeedRight;
        }
        else if (direction.x > 0)
        {
            FaceRight();
            rb.velocity = Vector2.right * bulletSpeedRight;
        }

        audioSource.Play();
    }

    // Function asignated to a fram of the animation
    void ShootDown()
    {
        // Shoot to the right
        GameObject bullet = Instantiate(bulletPrefab, spawnPoint2.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.down * bulletSpeedDown;

        audioSource.Play();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player Shoot"))
        {
            float fillAmount = 0.02f;
            fillImage.fillAmount += fillAmount;
            Destroy(other.gameObject);
            enemyHealthController.TakeDamage(1);
        }
        if (other.CompareTag("UltimateShoot"))
        {
            float fillAmount = 0.09f;
            fillImage.fillAmount += fillAmount;
            enemyHealthController.TakeDamage(15);
        }
    }

    // Method to make the enemy face right
    public void FaceRight()
    {
        if (!facingLeft)
        {
            Flip();
        }
    }

    // Method to make the enemy face left
    public void FaceLeft()
    {
        if (facingLeft)
        {
            Flip();
        }
    }

    // Method to flip the enemy's direction
    private void Flip()
    {
        facingLeft = !facingLeft; // Change direction

        // Get the current scale of the enemy
        Vector3 scale = transform.localScale;
        // Flip horizontally
        scale.x *= -1;
        // Apply the new scale to the enemy
        transform.localScale = scale;
    }
}

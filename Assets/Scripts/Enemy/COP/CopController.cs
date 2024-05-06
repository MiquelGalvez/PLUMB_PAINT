using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopController : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float minShootInterval = 1f;
    [SerializeField] float maxShootInterval = 2f;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] float shootDuration = 0.5f;
    [SerializeField] float maxBulletDistance = 10f;
    [SerializeField] AudioClip takedamage;
    private Image fillImage;
    private float movementSpeed = 2f;
    private EnemyHealthController enemyHealthController;

    private float shootTimer = 0f;
    private float currentShootInterval;
    private bool isShooting = false;
    private int shotsReceived = 0;
    private SpriteRenderer copRenderer;

    Camera mainCamera; // Reference to the main camera

    void Start()
    {
        copRenderer = GetComponent<SpriteRenderer>();
        currentShootInterval = Random.Range(minShootInterval, maxShootInterval);
        enemyHealthController = GetComponent<EnemyHealthController>();
        fillImage = GameObject.FindGameObjectWithTag("Ultimate").GetComponent<Image>();

        mainCamera = Camera.main; // Assign the main camera reference
    }

    void Update()
    {
        shootTimer += Time.deltaTime;

        if (isShooting)
        {
            if (shootTimer >= shootDuration)
            {
                isShooting = false;
                shootTimer = 0f;
            }
        }
        else
        {
            if (shootTimer >= currentShootInterval)
            {
                Shoot();
                currentShootInterval = Random.Range(minShootInterval, maxShootInterval);
                shootTimer = 0f;
            }
        }

        // Obtener la posición actual del jugador
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

        // Calcular la distancia entre el policía y el jugador
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);

        // Si el jugador está a menos de 10 unidades de distancia, avanzar hacia él
        if (distanceToPlayer < 10f)
        {
            // Determinar la dirección en la que debe avanzar el policía
            Vector2 movementDirection = playerPosition - transform.position;
            movementDirection.Normalize();

            // Aplicar movimiento hacia la derecha si mira hacia la derecha y hacia la izquierda si mira hacia la izquierda
            if (facingRight)
            {
                transform.Translate(movementDirection * movementSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(-movementDirection * movementSpeed * Time.deltaTime);
            }
        }

        // Check if the cop is outside the camera's view
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            Destroy(gameObject); // Destroy the cop if it's outside the camera's view
        }
    }

    void Shoot()
    {
        isShooting = true;

        if (facingRight)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.left * bulletSpeed;

            StartCoroutine(DestroyBulletAfterDistance(bullet));
        }
        else if (!facingRight)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.right * bulletSpeed;

            StartCoroutine(DestroyBulletAfterDistance(bullet));
        }
    }

    IEnumerator DestroyBulletAfterDistance(GameObject bullet)
    {
        Vector3 initialPosition = bullet.transform.position;
        float distanceTraveled = 0f;

        while (true)
        {
            distanceTraveled = Vector3.Distance(initialPosition, bullet.transform.position);

            if (distanceTraveled >= maxBulletDistance)
            {
                if (bullet != null)
                {
                    Destroy(bullet);
                    break;
                }
                else
                {
                    break;
                }
            }

            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player Shoot"))
        {
            float fillAmount = 0.01f;
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

    private bool facingRight = true; // Variable para mantener la dirección del enemigo

    // Método para hacer que el enemigo mire hacia la derecha
    public void FaceRight()
    {
        if (!facingRight)
        {
            Flip();
        }
    }

    // Método para hacer que el enemigo mire hacia la izquierda
    public void FaceLeft()
    {
        if (facingRight)
        {
            Flip();
        }
    }

    // Método para voltear la dirección del enemigo
    private void Flip()
    {
        facingRight = !facingRight; // Cambiar la dirección

        // Obtener la escala actual del enemigo
        Vector3 scale = transform.localScale;
        // Voltear la escala horizontalmente
        scale.x *= -1;
        // Aplicar la nueva escala al enemigo
        transform.localScale = scale;
    }
}

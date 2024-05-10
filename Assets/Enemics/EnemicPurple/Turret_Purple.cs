using UnityEngine;
using UnityEngine.UI;

public class Turret_Purple : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] AudioSource audiosource;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float minShootInterval = 1f;
    [SerializeField] float maxShootInterval = 2f;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] float shootDuration = 0.5f; // Duración de la animación de disparo
    [SerializeField] float maxBulletDistance = 10f;
    [SerializeField] AudioClip takedamage;
    [SerializeField] Image fillImage;
    private EnemyHealthController enemyHealthController;
    private GameObject player; // Referencia al objeto del jugador
    private float shootTimer = 0f;
    private float currentShootInterval;
    private bool isShooting = false;
    private SpriteRenderer turretRenderer;

    void Start()
    {
        currentShootInterval = Random.Range(minShootInterval, maxShootInterval);
        enemyHealthController = GetComponent<EnemyHealthController>();
        audiosource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player"); // Buscar el objeto del jugador
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
            if (shootTimer >= currentShootInterval - shootDuration * 0.5f) // Se activa 0.5 segundos antes del disparo
            {
                Shoot();
            }

            if (shootTimer >= currentShootInterval)
            {
                currentShootInterval = Random.Range(minShootInterval, maxShootInterval);
                shootTimer = 0f;
            }
        }
    }

    void Shoot()
    {
        isShooting = true;

        // Calcula la dirección hacia el jugador desde la posición del proyectil
        Vector2 direction = (player.transform.position - spawnPoint.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // Desactiva la gravedad para que el proyectil no caiga
        rb.velocity = direction * bulletSpeed; // Establece la velocidad en la dirección calculada

        // Calcula el ángulo de rotación en Z y aplica la rotación al proyectil
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f); // Ajusta la rotación en Z
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
            audiosource.PlayOneShot(takedamage);
            enemyHealthController.TakeDamage(15);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipController : MonoBehaviour
{
    [SerializeField] float speed = 5f; // Velocidad de movimiento
    private bool isFacingRight = true;

    private int direction = 1; // Dirección inicial de movimiento: 1 (hacia la derecha)
    [SerializeField] Image fillImage;
    private EnemyHealthController enemyHealthController;

    [SerializeField] GameObject bulletPrefab; // Prefab de la bala
    [SerializeField] Transform spawnPoint; // Punto de generación de la bala
    [SerializeField] float minShootInterval = 1f; // Intervalo mínimo entre disparos
    [SerializeField] float maxShootInterval = 4f; // Intervalo máximo entre disparos

    private float shootTimer = 0f; // Temporizador de disparo
    private float currentShootInterval; // Intervalo actual entre disparos

    void Start()
    {
        enemyHealthController = GetComponent<EnemyHealthController>();
        currentShootInterval = Random.Range(minShootInterval, maxShootInterval); // Inicializar intervalo de disparo
    }

    // Update is called once per frame
    void Update()
    {
        // Movimiento automático horizontal
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);

        // Obtener el borde de la cámara en el mundo
        float screenEdgeX = Camera.main.ViewportToWorldPoint(new Vector2(1, 0)).x;

        // Si la nave alcanza el borde de la cámara, cambiar la dirección
        if (transform.position.x >= screenEdgeX)
        {
            direction = -1; // Cambiar dirección hacia la izquierda
            if (isFacingRight)
            {
                Flip();
            }
        }
        else if (transform.position.x <= -screenEdgeX)
        {
            direction = 1; // Cambiar dirección hacia la derecha
            if (!isFacingRight)
            {
                Flip();
            }
        }

        // Actualizar temporizador de disparo
        shootTimer += Time.deltaTime;

        // Si el temporizador supera el intervalo de disparo actual, disparar
        if (shootTimer >= currentShootInterval)
        {
            Shoot(); // Realizar disparo
            currentShootInterval = Random.Range(minShootInterval, maxShootInterval); // Actualizar intervalo de disparo
            shootTimer = 0f; // Reiniciar temporizador de disparo
        }
    }

    // Método para realizar un disparo
    void Shoot()
    {
        // Instanciar la bala en el punto de generación
        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);

        // Mover la bala hacia abajo
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.down * 1.5f; // Mover hacia abajo

        // Reproducir cualquier efecto de sonido, animación, etc., aquí si es necesario
    }

    // Método para voltear la nave
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
}

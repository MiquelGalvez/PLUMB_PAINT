using UnityEngine;
using UnityEngine.UI;

public class Enemi_Moves : MonoBehaviour
{
    public float speed;
    public bool esDerecha;
    public float contadorT;
    public float tiempoParaCambiar = 4f;
    public Transform player; // Referencia al objeto del jugador

    // Referencia al Animator
    private Animator animator;
    private bool playerDetected = false;
    private GameObject[] weapons;
    [SerializeField] Image fillImage;
    private EnemyHealthController enemyHealthController;

    private void Start()
    {
        contadorT = tiempoParaCambiar;
        // Obteniendo el componente Animator
        animator = GetComponent<Animator>();
        weapons = GameObject.FindGameObjectsWithTag("Weapon");
        enemyHealthController = GetComponent<EnemyHealthController>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // Si no hay un componente SpriteRenderer, agrégalo
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            // Asigna el sprite que desees al SpriteRenderer si es necesario
            // spriteRenderer.sprite = tuSprite;
        }
    }

    private void Update()
    {
        // Comprobando si el jugador está dentro del rango de detección
        if (player != null && Vector3.Distance(transform.position, player.position) < 5f)
        {
            playerDetected = true;
        }
        else
        {
            playerDetected = false;
        }

        // Controlar la dirección del movimiento y la animación
        // Dentro del método Update()

        // Controlar la dirección del movimiento y la animación
        if (esDerecha)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            if (transform.eulerAngles.y != 0) // Si no está mirando a la derecha
            {
                transform.rotation = Quaternion.Euler(0, 0, 0); // Rotar hacia la derecha
            }
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            if (transform.eulerAngles.y != 180) // Si no está mirando a la izquierda
            {
                transform.rotation = Quaternion.Euler(0, 180, 0); // Rotar hacia la izquierda
            }
        }


        // Si el jugador está detectado, activar animación y arma
        if (playerDetected)
        {
            animator.SetBool("PlayerDetect", true);

            // Activar objetos con el tag "Weapon"
            foreach (GameObject weapon in weapons)
            {
                weapon.SetActive(true);
            }

            // Establecer "EstaCaminando" como false si PlayerDetect es true
            animator.SetBool("EstaCaminando", false);
        }
        else
        {
            // Si el jugador no está detectado, desactivar animación y arma
            animator.SetBool("PlayerDetect", false);

            // Desactivar objetos con el tag "Weapon"
            foreach (GameObject weapon in weapons)
            {
                weapon.SetActive(false);
            }

            // Actualizar el parámetro "EstaCaminando" basado en la velocidad del enemigo
            animator.SetBool("EstaCaminando", speed > 0);
        }

        // Reducir el contador de tiempo
        contadorT -= Time.deltaTime;

        // Cambiar la dirección del movimiento después de un tiempo determinado
        if (contadorT <= 0)
        {
            contadorT = tiempoParaCambiar;
            esDerecha = !esDerecha;
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
}

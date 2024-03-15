using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    public float moveSpeed; // Velocidad de movimiento normal
    public float runSpeed; // Velocidad de movimiento al correr
    public float jumpForce; // Fuerza del salto
    public GameObject bulletPrefab; // El prefab de la bala
    public float bulletSpeed = 5; // Velocitat de la bala
    private Camera cam;

    private Rigidbody2D rb;
    private bool isRunning;
    private bool isFacingRight = true;
    private bool isGrounded; // Indica si el personaje está en el suelo

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>(); // Obtener la referencia al Animator
        cam = Camera.main;
    }

    void Update()
    {
        // Movimiento horizontal
        float moveInput = Input.GetAxisRaw("Horizontal");
        Flip(moveInput);
        float moveSpeedCurrent = isRunning ? runSpeed : moveSpeed;
        rb.velocity = new Vector2(moveInput * moveSpeedCurrent, rb.velocity.y);

        // Saltar solo si está en el suelo
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            _animator.SetBool("IsJumping", true);
        }

        // Correr al presionar Shift
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
            _animator.SetBool("IsRunning", true); // Establecer el parámetro en true
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            _animator.SetBool("IsRunning", false); // Establecer el parámetro en false
        }

        // Disparar al hacer clic con el ratón
        if (Input.GetMouseButtonDown(0)) // Botón izquierdo del ratón
        {
            Shoot();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            _animator.SetBool("IsJumping", false); // Establecer el parámetro en false
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            _animator.SetBool("IsJumping", true); // Establecer el parámetro en true
        }
    }

    void OnBecameInvisible()
    {
        if (transform.position.y < Camera.main.transform.position.y)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (transform.position.x > Camera.main.transform.position.x)
        {
            SceneManager.LoadScene(3);
        }
    }

    void Flip(float moveInput)
    {
        if (moveInput > 0 && !isFacingRight || moveInput < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null)
        {
            // Crea l'objecte bala amb l'Instiante
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // Agafa el rigid body del gameobject bullet
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

            // Calcula la direcció de la bala en funcio a si el player esta mirant a la dreta o l'esquerra
            Vector2 shootDirection = isFacingRight ? Vector2.right : Vector2.left;

            // Intsancia la velocitat de la bala i la multiplica en la direcció en la que esta mirant el jugador
            bulletRb.velocity = shootDirection * bulletSpeed;

            Destroy(bullet,2.5f);
        }
    }
}

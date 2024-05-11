using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private AudioSource audioSource;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private GameObject youdied;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject fadein;
    private SpriteRenderer playerRender;
    private float extraFallSpeed = 10f;
    private Color originalColor;
    private Color damageColor = new Color(1f, 0.5f, 0.5f, 1f);
    private Camera cam;
    private Rigidbody2D rb;
    private bool isRunning;
    private bool isFacingRight = true;
    private bool isGrounded;
    [SerializeField] private Image imageToModify;
    [SerializeField] private float widthDecreaseAmount;
    [SerializeField] private float extraWidthDecreaseAmount; // Extra reducción para el misil
    private bool imageWidthZero = false;


    void Start()
    {
        fadein.gameObject.SetActive(true);
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        cam = Camera.main;
        Cursor.visible = false;
        audioSource = GetComponent<AudioSource>();
        playerRender = GetComponent<SpriteRenderer>();
        originalColor = playerRender.color;
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        Flip(moveInput);
        float moveSpeedCurrent = isRunning ? runSpeed : moveSpeed;
        rb.velocity = new Vector2(moveInput * moveSpeedCurrent, rb.velocity.y);

        // Check if the player is grounded and can jump
        if (isGrounded && Input.GetKey(KeyCode.W))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            _animator.SetBool("IsJumping", true);
        }

        // If the player is in the air and the 'S' key is pressed, increase the downward velocity
        if (!isGrounded && Input.GetKey(KeyCode.S))
        {
            rb.velocity += Vector2.down * Time.deltaTime * extraFallSpeed;
        }

        // Check if the player is running
        if (isGrounded && moveInput != 0 && !isRunning)
        {
            isRunning = true;
            _animator.SetBool("IsRunning", true);
        }
        else if ((moveInput == 0 || !isGrounded) && isRunning)
        {
            isRunning = false;
            _animator.SetBool("IsRunning", false);
        }

        if (imageWidthZero)
        {
            enabled = false;
            youdied.SetActive(true);
            hud.SetActive(false);
            Cursor.visible = true;
            audioSource = null;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            _animator.SetBool("IsJumping", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            _animator.SetBool("IsJumping", true);
        }
    }

    void Flip(float moveInput)
    {
        if ((moveInput > 0 && !isFacingRight) || (moveInput < 0 && isFacingRight))
        {
            isFacingRight = !isFacingRight;

            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            foreach (Transform child in transform)
            {
                Vector3 childScale = child.localScale;
                childScale.x *= -1;
                child.localScale = childScale;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            // Obtener el animator del objeto que colisiona con el jugador
            Animator turretAnimator = collision.GetComponent<Animator>();

            if (turretAnimator != null)
            {
                // Ejecutar la animación de explotar en el Animator de la torreta
                turretAnimator.SetTrigger("Explode");
            }
            // Desactivar el Rigidbody de la bala para que se quede quieta
            Rigidbody2D bulletRb = collision.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.velocity = Vector2.zero;
                bulletRb.simulated = false;
            }

            // Llamar a la corrutina para destruir la bala después de medio segundo
            StartCoroutine(DestruirBala(collision.gameObject));

            // Cambiar el color del render a rojo claro durante 0.5 segundos
            StartCoroutine(ColorImpactRender());

            // Reducir el ancho de la imagen
            if (imageToModify != null)
            {
                StartCoroutine(ReducirAncho());
            }
        }
        else if (collision.CompareTag("Misile"))
        {
            Animator turretAnimator = collision.GetComponent<Animator>();

            if (turretAnimator != null)
            {
                turretAnimator.SetTrigger("Explode");
            }

            Rigidbody2D misilerb = collision.GetComponent<Rigidbody2D>();
            if (misilerb != null)
            {
                misilerb.velocity = Vector2.zero;
                misilerb.simulated = false;
            }

            StartCoroutine(DestruirBala(collision.gameObject));


            // Cambiar el color del render a rojo claro durante 0.5 segundos
            StartCoroutine(ColorImpactRender());

            if (imageToModify != null)
            {
                StartCoroutine(ReducirAncho(extraWidthDecreaseAmount)); // Reducción extra por el misil
            }
        }
        else if (collision.CompareTag("CopBullet"))
        {
            Animator turretAnimator = collision.GetComponent<Animator>();

            if (turretAnimator != null)
            {
                turretAnimator.SetTrigger("Explode");
            }

            Rigidbody2D misilerb = collision.GetComponent<Rigidbody2D>();
            if (misilerb != null)
            {
                misilerb.velocity = Vector2.zero;
                misilerb.simulated = false;
            }

            StartCoroutine(DestruirBala(collision.gameObject));


            // Cambiar el color del render a rojo claro durante 0.5 segundos
            StartCoroutine(ColorImpactRender());

            if (imageToModify != null)
            {
                StartCoroutine(ReducirAncho()); // Reducción extra por el misil
            }
        }
        else if (collision.CompareTag("BossAtack"))
        {
            Rigidbody2D misilerb = collision.GetComponent<Rigidbody2D>();
            if (misilerb != null)
            {
                misilerb.velocity = Vector2.zero;
                misilerb.simulated = false;
            }

            Destroy(collision.gameObject);
            

            // Cambiar el color del render a rojo claro durante 0.5 segundos
            StartCoroutine(ColorImpactRender());

            if (imageToModify != null)
            {
                StartCoroutine(ReducirAncho(5)); // Reducción extra por el misil
            }
        }
        else if (collision.CompareTag("PassScene"))
        {
            PassLevel();
        }
    }

    IEnumerator DestruirBala(GameObject bullet)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(bullet);
    }

    IEnumerator ReducirAncho(float extraDecrease = 0f)
    {
        float newFillAmount = imageToModify.fillAmount - (widthDecreaseAmount + extraDecrease) / imageToModify.rectTransform.sizeDelta.x;
        float elapsedTime = 0;
        float duration = 0.1f;

        while (elapsedTime < duration)
        {
            float currentFillAmount = Mathf.Lerp(imageToModify.fillAmount, newFillAmount, elapsedTime / duration);
            imageToModify.fillAmount = currentFillAmount;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        imageToModify.fillAmount = newFillAmount;

        if (newFillAmount <= 0)
        {
            imageWidthZero = true;
        }
    }

    private void PassLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }

    IEnumerator ColorImpactRender()
    {
        // Cambiar el color a flashColor durante 0.5 segundos
        playerRender.color = damageColor;

        yield return new WaitForSeconds(0.5f);

        // Devolver el color original después de 0.5 segundos
        playerRender.color = originalColor;
    }
}
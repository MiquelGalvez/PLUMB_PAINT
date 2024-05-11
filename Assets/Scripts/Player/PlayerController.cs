using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using MongoDB.Bson;
using MongoDB.Driver;
using TMPro;
using System;
using System.Threading.Tasks;

public class PlayerController : MonoBehaviour
{
    private PlayerData playerData;
    private Animator _animator;
    private AudioSource audioSource;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private GameObject youdied;
    [SerializeField] private GameObject hud;
    private SpriteRenderer playerRender;
    private bool levelPassed = false;
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
    [SerializeField] private TextMeshProUGUI scoreCounter;
    private DatabaseAccess databaseaccess;


    void Start()
    {
        Time.timeScale = 1f;
        databaseaccess = new DatabaseAccess();
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        cam = Camera.main;
        Cursor.visible = false;
        // Guardar en GameData
        playerData = GameData.playerData;
        audioSource = GetComponent<AudioSource>();
        playerRender = GetComponent<SpriteRenderer>();
        originalColor = playerRender.color;
        scoreCounter = FindTextMeshProUGUIByTag("ScoreCounter");
        if (scoreCounter == null)
        {
            Debug.LogWarning("No score counter found with tag 'ScoreCounter'.");
        }
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
            Die();
        }
        CheckIfOutOfCameraView();
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

    private async void OnTriggerEnter2D(Collider2D collision)
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
        if (collision.CompareTag("PassScene") && !levelPassed)
        {
            PassLevel();
            levelPassed = true;
        }
    }

    IEnumerator DestruirBala(GameObject bullet)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(bullet);
    }

    private TextMeshProUGUI FindTextMeshProUGUIByTag(string tag)
    {
        TextMeshProUGUI[] textMeshes = FindObjectsOfType<TextMeshProUGUI>();
        foreach (TextMeshProUGUI textMesh in textMeshes)
        {
            if (textMesh.CompareTag(tag))
            {
                return textMesh;
            }
        }
        return null; // If no object with the tag is found
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
        double score = 0;

        if (scoreCounter != null && double.TryParse(scoreCounter.text, out score))
        {
            // El valor de scoreCounter.text se pudo convertir correctamente a un double
            // Actualiza el puntaje del jugador en la base de datos
            databaseaccess.UpdateScore(playerData.playerName, score);
        }
        else
        {
            // El valor de scoreCounter.text no se pudo convertir a un double
            // Muestra un mensaje de advertencia o maneja el caso de error de otra manera
            Debug.LogWarning("No se pudo convertir el puntaje a un valor numérico.");
        }

        // Cambia de escena
        ChangeScene();
    }

    private void ChangeScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;

        if (nextSceneIndex <= 4)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
            // Aquí puedes agregar cualquier otra lógica o acción que desees cuando el índice de la escena sea igual o mayor que 4.
            Debug.Log("Índice de escena igual o mayor que 4. No se carga ninguna nueva escena.");
        }
    }

    void CheckIfOutOfCameraView()
    {
        Camera[] cameras = Camera.allCameras; // Obtener todas las cámaras en la escena

        foreach (Camera cam in cameras)
        {
            if (cam.CompareTag("Level2Cam")) // Reemplaza "SpecificCameraTag" con el tag específico de tu cámara
            {
                if (!cam.pixelRect.Contains(cam.WorldToViewportPoint(transform.position)))
                {
                    // Si la posición del jugador no está dentro de la vista de la cámara
                    Die(); // Llama a la función Die() para que el jugador muera
                }
            }
        }
    }

    private void Die()
    {
        enabled = false;
        youdied.SetActive(true);
        hud.SetActive(false);
        Cursor.visible = true;
        audioSource = null;
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
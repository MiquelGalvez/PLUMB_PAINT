using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    public float moveSpeed; // Velocidad de movimiento normal
    public float runSpeed; // Velocidad de movimiento al correr
    public float jumpForce; // Fuerza del salto
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

    /*void OnBecameInvisible()
     {
         if (transform.position.y < Camera.main.transform.position.y)
         {
             SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
         }
         if (transform.position.x > Camera.main.transform.position.x)
         {
             SceneManager.LoadScene(3);
         }
     }*/

    void Flip(float moveInput)
    {
        if ((moveInput > 0 && !isFacingRight) || (moveInput < 0 && isFacingRight))
        {
            isFacingRight = !isFacingRight;

            // Girar el objeto principal
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            // Girar todos los hijos del objeto
            foreach (Transform child in transform)
            {
                Vector3 childScale = child.localScale;
                childScale.x *= -1;
                child.localScale = childScale;
            }
        }
    }
}

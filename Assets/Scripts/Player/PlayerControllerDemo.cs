using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerControllerDemo : MonoBehaviour
{
    private Animator _animator;
    private AudioSource audioSource;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private GameObject fadein;
    private SpriteRenderer playerRender;
    private float extraFallSpeed = 10f;
    private Camera cam;
    private Rigidbody2D rb;
    private bool isRunning;
    private bool isFacingRight = true;
    private bool isGrounded;


    void Start()
    {
        fadein.gameObject.SetActive(true);
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        cam = Camera.main;
        Cursor.visible = true;
        audioSource = GetComponent<AudioSource>();
        playerRender = GetComponent<SpriteRenderer>();
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
}

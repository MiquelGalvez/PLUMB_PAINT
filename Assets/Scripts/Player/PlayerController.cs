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
    [SerializeField] private float extraWidthDecreaseAmount; // Extra reduction for the missile
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
        // Save in GameData
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
            // Get the animator from the object colliding with the player
            Animator turretAnimator = collision.GetComponent<Animator>();

            if (turretAnimator != null)
            {
                // Execute the explode animation in the turret's Animator
                turretAnimator.SetTrigger("Explode");
            }
            // Disable the Rigidbody of the bullet to make it stay still
            Rigidbody2D bulletRb = collision.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.velocity = Vector2.zero;
                bulletRb.simulated = false;
            }

            // Call the coroutine to destroy the bullet after half a second
            StartCoroutine(DestroyBullet(collision.gameObject));

            // Change the render color to light red for 0.5 seconds
            StartCoroutine(ColorImpactRender());

            // Reduce the width of the image
            if (imageToModify != null)
            {
                StartCoroutine(DecreaseWidth());
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

            StartCoroutine(DestroyBullet(collision.gameObject));


            // Change the render color to light red for 0.5 seconds
            StartCoroutine(ColorImpactRender());

            if (imageToModify != null)
            {
                StartCoroutine(DecreaseWidth(extraWidthDecreaseAmount)); // Extra reduction for the missile
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

            StartCoroutine(DestroyBullet(collision.gameObject));


            // Change the render color to light red for 0.5 seconds
            StartCoroutine(ColorImpactRender());

            if (imageToModify != null)
            {
                StartCoroutine(DecreaseWidth()); // Extra reduction for the missile
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


            // Change the render color to light red for 0.5 seconds
            StartCoroutine(ColorImpactRender());

            if (imageToModify != null)
            {
                StartCoroutine(DecreaseWidth(5)); // Extra reduction for the missile
            }
        }
        if (collision.CompareTag("PassScene") && !levelPassed)
        {
            PassLevel();
            levelPassed = true;
        }
    }

    IEnumerator DestroyBullet(GameObject bullet)
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

    IEnumerator DecreaseWidth(float extraDecrease = 0f)
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
            // The value of scoreCounter.text was successfully converted to a double
            // Update the player's score in the database
            databaseaccess.UpdateScore(playerData.playerName, score);
        }
        else
        {
            // The value of scoreCounter.text could not be converted to a double
            // Show a warning message or handle the error case in another way
            Debug.LogWarning("Score could not be converted to a numerical value.");
        }

        // Change scene
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
            // Here you can add any other logic or action you want when the scene index is equal to or greater than 4.
            Debug.Log("Scene index equal to or greater than 4. No new scene is loaded.");
        }
    }

    void CheckIfOutOfCameraView()
    {
        Camera[] cameras = Camera.allCameras; // Get all cameras in the scene

        foreach (Camera cam in cameras)
        {
            if (cam.CompareTag("Level2Cam")) // Replace "SpecificCameraTag" with the specific tag of your camera
            {
                if (!cam.pixelRect.Contains(cam.WorldToViewportPoint(transform.position)))
                {
                    // If the player's position is not within the camera's view
                    Die(); // Call the Die() function to make the player die
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
        // Change color to flashColor for 0.5 seconds
        playerRender.color = damageColor;

        yield return new WaitForSeconds(0.5f);

        // Return to original color after 0.5 seconds
        playerRender.color = originalColor;
    }
}

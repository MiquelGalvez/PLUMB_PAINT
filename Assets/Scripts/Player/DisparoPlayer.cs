using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisparoPlayer : MonoBehaviour
{
    [SerializeField] private Transform controladorDisparo; // Reference to the shooting controller
    [SerializeField] private GameObject balaPrefab; // Bullet prefab
    [SerializeField] private GameObject ultimatePrefab; // Ultimate ability prefab
    private GameObject ultimate; // Reference to the ultimate ability object
    private Image ultimateImg; // Image component for the ultimate ability UI
    [SerializeField] private AudioClip disparoClip; // Sound clip for regular shooting
    [SerializeField] private AudioClip ultimateClip; // Sound clip for ultimate ability
    private AudioSource source; // Audio source component

    public Vector2 direccionJugador = Vector2.right; // Player shooting direction
    private bool isFilling; // Flag to track if ultimate ability UI is filling up
    private bool isShooting; // Flag to control continuous shooting
    private float cooldown = 0.5f; // Cooldown between each bullet shot
    private float lastShotTime; // Time of the last shot

    private Vector2 posicionInicial; // Initial player position when shooting the ultimate ability

    private void Start()
    {
        // Find and initialize ultimate ability UI elements
        ultimate = GameObject.FindGameObjectWithTag("Ultimate");
        ultimateImg = ultimate.GetComponent<Image>();
        source = GetComponent<AudioSource>(); // Get the AudioSource component
        isFilling = false;
        isShooting = false;
        lastShotTime = -cooldown; // Initialize the last shot time to allow immediate shooting
        // Start the coroutine to make the ultimate ability UI blink
        StartCoroutine(BlinkImage());
    }

    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal"); // Get horizontal movement input
        direccionJugador = moveInput > 0 ? Vector2.right : (moveInput < 0 ? Vector2.left : direccionJugador);

        // Prevent shooting left if not facing left
        if ((Input.GetKey(KeyCode.LeftArrow) && direccionJugador.x > 0) || (Input.GetKey(KeyCode.RightArrow) && direccionJugador.x < 0))
        {
            return;
        }

        // Check if shoot key is held down and cooldown has passed
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) &&
            Time.time >= lastShotTime + cooldown)
        {
            isShooting = true;
        }
        else
        {
            isShooting = false;
        }

        // If continuously shooting
        if (isShooting)
        {
            // If cooldown has passed, shoot and update last shot time
            if (Time.time >= lastShotTime + cooldown)
            {
                lastShotTime = Time.time;
                if (source != null && disparoClip != null)
                {
                    source.PlayOneShot(disparoClip); // Play the shooting sound
                    Disparar(); // Shoot regular bullet
                }
            }
        }

        // Check if ultimate ability UI is fully filled
        if (ultimateImg.fillAmount == 1f)
        {
            isFilling = true;
        }

        // If ultimate ability key is pressed and UI is fully filled
        if (Input.GetKeyDown(KeyCode.E) && ultimateImg.fillAmount == 1f)
        {
            if (source != null && ultimateClip != null)
            {
                source.PlayOneShot(ultimateClip); // Play the ultimate ability sound
                Invoke("DispararUltimate", 1.5f); // Shoot the ultimate ability after a delay
            }

            ultimateImg.fillAmount = 0f; // Reset the ultimate ability UI fill amount
        }
    }

    // Method to shoot regular bullets
    private void Disparar()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, 0); // Default rotation
        Vector2 direccionDisparo = direccionJugador; // Shooting direction
        GameObject balaInstance = null; // Declare bullet instance variable outside if block

        // Detect which arrow key is pressed and adjust shooting direction
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direccionDisparo = Vector2.right; // Change shooting direction upwards
            rotation = Quaternion.Euler(0, 0, 90); // Rotate bullet 90 degrees for upwards shooting
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            direccionDisparo = Vector2.right; // Change shooting direction downwards
            rotation = Quaternion.Euler(0, 0, -90); // Rotate bullet -90 degrees for downwards shooting
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            direccionDisparo = Vector2.left; // Change shooting direction to the left
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            direccionDisparo = Vector2.right; // Change shooting direction to the right
        }

        // Instantiate the bullet with the appropriate shooting direction
        balaInstance = Instantiate(balaPrefab, controladorDisparo.position, rotation);

        // Check if a bullet is instantiated before trying to get the component
        if (balaInstance != null)
        {
            // Get the bullet script and set the shooting direction
            Bala balaScript = balaInstance.GetComponent<Bala>();
            if (balaScript != null)
            {
                balaScript.EstablecerDireccionDeDisparo(direccionDisparo);
            }
            else
            {
                Debug.LogWarning("El prefab de la bala no tiene el componente Bala adjunto.");
            }
        }
        else
        {
            Debug.LogWarning("No se pudo instanciar la bala.");
        }
    }

    // Method to shoot the ultimate ability
    private void DispararUltimate()
    {
        Vector3 posicionCursor = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get cursor position in world space
        posicionCursor.z = 0f; // Set cursor position's Z coordinate to 0 (same as player)

        Vector2 direccionDisparo = direccionJugador; // Shooting direction

        // Save initial position when shooting the ultimate ability
        posicionInicial = transform.position;

        // Check if W key is pressed
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direccionDisparo = Vector2.right; // Change shooting direction upwards

            // If shooting upwards, rotate the instance by 90 degrees on the Z axis
            Quaternion rotation = Quaternion.Euler(0, 0, 90);
            GameObject ultimateInstance = Instantiate(ultimatePrefab, controladorDisparo.position, rotation);
            Bala balaScript = ultimateInstance.GetComponent<Bala>();
            if (balaScript != null)
            {
                balaScript.EstablecerDireccionDeDisparo(direccionDisparo);
            }
            else
            {
                Debug.LogWarning("El prefab de la bala no tiene el componente Bala adjunto.");
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            direccionDisparo = Vector2.right; // Change shooting direction downwards

            // If shooting downwards, rotate the instance by -90 degrees on the Z axis
            Quaternion rotation = Quaternion.Euler(0, 0, -90);
            GameObject ultimateInstance = Instantiate(ultimatePrefab, controladorDisparo.position, rotation);
            Bala balaScript = ultimateInstance.GetComponent<Bala>();
            if (balaScript != null)
            {
                balaScript.EstablecerDireccionDeDisparo(direccionDisparo);
            }
            else
            {
                Debug.LogWarning("El prefab de la bala no tiene el componente Bala adjunto.");
            }
        }
        else
        {
            if (direccionDisparo.x > 0f)
            {
                // If not shooting upwards or downwards, maintain normal rotation
                GameObject ultimateInstance = Instantiate(ultimatePrefab, controladorDisparo.position, Quaternion.identity);
                Bala balaScript = ultimateInstance.GetComponent<Bala>();
                if (balaScript != null)
                {
                    balaScript.EstablecerDireccionDeDisparo(direccionDisparo);
                }
                else
                {
                    Debug.LogWarning("El prefab de la bala no tiene el componente Bala adjunto.");
                }
            }
            else if (direccionDisparo.x < 0f)
            {
                // If not shooting upwards or downwards, maintain normal rotation but inverted
                Quaternion rotation = Quaternion.Euler(0, 0, 180);
                GameObject ultimateInstance = Instantiate(ultimatePrefab, controladorDisparo.position, rotation);
                Bala balaScript = ultimateInstance.GetComponent<Bala>();
                if (balaScript != null)
                {
                    balaScript.EstablecerDireccionDeDisparo(Vector2.right);
                }
                else
                {
                    Debug.LogWarning("El prefab de la bala no tiene el componente Bala adjunto.");
                }
            }

        }

        // Apply recoil when shooting the ultimate ability
        AplicarRecoil();
    }

    // Method to apply recoil after shooting the ultimate ability
    private void AplicarRecoil()
    {
        // Apply recoil to the player from the initial position
        float recoilAmountX = 0.5f; // Adjust recoil amount on the X axis as needed
        float recoilAmountY = 0.0f; // Adjust recoil amount on the Y axis as needed

        // Calculate recoil vector on the X axis
        Vector2 recoilDirectionX = -direccionJugador.normalized; // Invert player direction on the X axis
        Vector2 recoilVectorX = recoilDirectionX * recoilAmountX;

        // Calculate recoil vector on the Y axis
        Vector2 recoilVectorY = Vector2.up * recoilAmountY; // Apply recoil upwards on the Y axis

        // Combine recoil vectors on both axes
        Vector2 totalRecoilVector = recoilVectorX + recoilVectorY;

        // Apply recoil vector to the player's initial position
        transform.position = posicionInicial + totalRecoilVector;
    }

    // Coroutine to make the ultimate ability UI blink
    private IEnumerator BlinkImage()
    {
        Color originalColor = ultimateImg.color; // Original color of the ultimate ability UI
        Color targetColor = Color.yellow; // Target color for blinking
        float blinkSpeed = 0.5f; // Blink speed (in seconds)

        while (true)
        {
            // If filling and fillAmount is 1, alternate between current color and yellow
            if (isFilling && ultimateImg.fillAmount == 1f)
            {
                ultimateImg.color = targetColor;
                yield return new WaitForSeconds(blinkSpeed / 2);
                ultimateImg.color = originalColor;
                yield return new WaitForSeconds(blinkSpeed / 2);
            }
            else
            {
                // If not filling or fillAmount is no longer 1, revert to original color and wait before checking again
                ultimateImg.color = originalColor;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}

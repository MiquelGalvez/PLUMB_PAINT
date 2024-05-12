using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class DisparoPlayerDemo : MonoBehaviour
{
    [SerializeField] private Transform controladorDisparo;
    [SerializeField] private GameObject balaPrefab;
    [SerializeField] private GameObject ultimatePrefab;
    private AudioSource source;
    [SerializeField] private AudioClip ultimateClip;
    private bool isShooting; // Variable to control continuous shooting
    private float cooldown = 0.5f; // Cooldown between each bullet
    private float lastShotTime; // Time of the last shot

    public Vector2 direccionJugador = Vector2.right;
    private bool isFilling;

    private Vector2 posicionInicial; // Stores the initial player position when firing the ultimate

    private void Start()
    {
        source = GetComponent<AudioSource>();
        isFilling = false;
        lastShotTime = -cooldown; // Initialize the last shot time to be able to shoot immediately
    }

    private void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        direccionJugador = moveInput > 0 ? Vector2.right : (moveInput < 0 ? Vector2.left : direccionJugador);


        // Check if the player is facing left before allowing shooting in that direction
        if ((Input.GetKey(KeyCode.LeftArrow) && direccionJugador.x > 0) || (Input.GetKey(KeyCode.RightArrow) && direccionJugador.x < 0))
        {
            // The player cannot shoot left if not facing that direction
            return;
        }

        // If the shoot key is held and the cooldown has passed
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

        // If shooting continuously
        if (isShooting)
        {
            // If the cooldown has passed, shoot and update the time of the last shot
            if (Time.time >= lastShotTime + cooldown)
            {
                lastShotTime = Time.time;
                if (source != null)
                {
                    Disparar();
                }
            }
        }

        // When the E key is pressed, fire the ultimate if available
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (source != null && ultimateClip != null)
            {
                source.PlayOneShot(ultimateClip);
                Invoke("DispararUltimate", 1.5f);
            }

        }
    }

    private void Disparar()
    {
        Vector3 posicionCursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicionCursor.z = 0f;
        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        Vector2 direccionDisparo = direccionJugador;
        GameObject balaInstance = null; // Declare the variable outside the if block

        // Detect which arrow key is pressed and adjust the shooting direction
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direccionDisparo = Vector2.right; // Change the shooting direction upwards
            rotation = Quaternion.Euler(0, 0, 90);

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            direccionDisparo = Vector2.right; // Change the shooting direction downwards
            rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            direccionDisparo = Vector2.left; // Change the shooting direction to the left

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            direccionDisparo = Vector2.right; // Change the shooting direction to the right
        }

        // Instantiate the bullet with the appropriate shooting direction
        balaInstance = Instantiate(balaPrefab, controladorDisparo.position, rotation);

        // Check if a bullet has been instantiated before attempting to get the component
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
                Debug.LogWarning("The bullet prefab does not have the attached Bala component.");
            }
        }
        else
        {
            Debug.LogWarning("The bullet could not be instantiated.");
        }
    }

    private void DispararUltimate()
    {
        Vector3 posicionCursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicionCursor.z = 0f;

        Vector2 direccionDisparo = direccionJugador;

        // Store the initial position when firing the ultimate
        posicionInicial = transform.position;

        // Check if the W key is pressed
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direccionDisparo = Vector2.right; // Change the shooting direction upwards

            // If firing upwards, rotate the instance by 90 degrees on the Z axis
            Quaternion rotation = Quaternion.Euler(0, 0, 90);
            GameObject ultimateInstance = Instantiate(ultimatePrefab, controladorDisparo.position, rotation);
            Bala balaScript = ultimateInstance.GetComponent<Bala>();
            if (balaScript != null)
            {
                balaScript.EstablecerDireccionDeDisparo(direccionDisparo);
            }
            else
            {
                Debug.LogWarning("The bullet prefab does not have the attached Bala component.");
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            direccionDisparo = Vector2.right; // Change the shooting direction downwards

            // If firing downwards, rotate the instance by -90 degrees on the Z axis
            Quaternion rotation = Quaternion.Euler(0, 0, -90);
            GameObject ultimateInstance = Instantiate(ultimatePrefab, controladorDisparo.position, rotation);
            Bala balaScript = ultimateInstance.GetComponent<Bala>();
            if (balaScript != null)
            {
                balaScript.EstablecerDireccionDeDisparo(direccionDisparo);
            }
            else
            {
                Debug.LogWarning("The bullet prefab does not have the attached Bala component.");
            }
        }
        else
        {
            if (direccionDisparo.x > 0f)
            {
                // If not firing upwards or downwards, maintain normal rotation
                GameObject ultimateInstance = Instantiate(ultimatePrefab, controladorDisparo.position, Quaternion.identity);
                Bala balaScript = ultimateInstance.GetComponent<Bala>();
                if (balaScript != null)
                {
                    balaScript.EstablecerDireccionDeDisparo(direccionDisparo);
                }
                else
                {
                    Debug.LogWarning("The bullet prefab does not have the attached Bala component.");
                }
            }
            else if (direccionDisparo.x < 0f)
            {
                // If not firing upwards or downwards, maintain normal rotation but inverted
                Quaternion rotation = Quaternion.Euler(0, 0, 180);
                GameObject ultimateInstance = Instantiate(ultimatePrefab, controladorDisparo.position, rotation);
                Bala balaScript = ultimateInstance.GetComponent<Bala>();
                if (balaScript != null)
                {
                    balaScript.EstablecerDireccionDeDisparo(Vector2.right);
                }
                else
                {
                    Debug.LogWarning("The bullet prefab does not have the attached Bala component.");
                }
            }

        }

        // Apply recoil when firing the ultimate
        AplicarRecoil();
    }

    private void AplicarRecoil()
    {
        // Apply recoil to the player from the initial position
        float recoilAmountX = 0.5f; // Adjust the amount of recoil on the X axis as needed
        float recoilAmountY = 0.0f; // Adjust the amount of recoil on the Y axis as needed

        // Calculate the recoil vector on the X axis
        Vector2 recoilDirectionX = -direccionJugador.normalized; // Invert the player direction on the X axis
        Vector2 recoilVectorX = recoilDirectionX * recoilAmountX;

        // Calculate the recoil vector on the Y axis
        Vector2 recoilVectorY = Vector2.up * recoilAmountY; // Apply upward recoil on the Y axis

        // Combine the recoil vectors on both axes
        Vector2 totalRecoilVector = recoilVectorX + recoilVectorY;

        // Apply the recoil vector to the player's initial position
        transform.position = posicionInicial + totalRecoilVector;
    }
}

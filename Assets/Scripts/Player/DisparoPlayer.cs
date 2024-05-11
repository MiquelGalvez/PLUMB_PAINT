using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class DisparoPlayer : MonoBehaviour
{
    [SerializeField] private Transform controladorDisparo;
    [SerializeField] private GameObject balaPrefab;
    [SerializeField] private GameObject ultimatePrefab;
    private GameObject ultimate;
    private Image ultimateImg;
    [SerializeField] private AudioClip disparoClip;
    [SerializeField] private AudioClip ultimateClip;
    private AudioSource source;

    public Vector2 direccionJugador = Vector2.right;
    private bool isFilling;
    private bool isShooting; // Variable para controlar si se está disparando continuamente
    private float cooldown = 0.5f; // Cooldown entre cada bala
    private float lastShotTime; // Tiempo del último disparo

    private Vector2 posicionInicial; // Guarda la posición inicial del jugador al disparar la ultimate

    private void Start()
    {
        ultimate = GameObject.FindGameObjectWithTag("Ultimate");
        ultimateImg = ultimate.GetComponent<Image>();
        source = GetComponent<AudioSource>();
        isFilling = false;
        isShooting = false;
        lastShotTime = -cooldown; // Inicializa el último tiempo de disparo para que pueda disparar de inmediato
        // Comienza la corutina para hacer parpadear la imagen
        StartCoroutine(BlinkImage());
    }

    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        direccionJugador = moveInput > 0 ? Vector2.right : (moveInput < 0 ? Vector2.left : direccionJugador);


        // Verifica si el jugador está mirando hacia la izquierda antes de permitir el disparo hacia esa dirección
        if ((Input.GetKey(KeyCode.LeftArrow) && direccionJugador.x > 0) || (Input.GetKey(KeyCode.RightArrow) && direccionJugador.x < 0))
        {
            // El jugador no puede disparar hacia la izquierda si no está mirando en esa dirección
            return;
        }


        // Si la tecla de disparo está siendo mantenida y ha pasado el cooldown
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

        // Si se está disparando continuamente
        if (isShooting)
        {
            // Si el cooldown ha pasado, dispara y actualiza el tiempo del último disparo
            if (Time.time >= lastShotTime + cooldown)
            {
                lastShotTime = Time.time;
                if (source != null && disparoClip != null)
                {
                    source.PlayOneShot(disparoClip);
                    Disparar();
                }
            }
        }

        if (ultimateImg.fillAmount == 1f)
        {
            isFilling = true;
        }

        if (Input.GetKeyDown(KeyCode.E) && ultimateImg.fillAmount == 1f)
        {
            if (source != null && ultimateClip != null)
            {
                source.PlayOneShot(ultimateClip);
                Invoke("DispararUltimate", 1.5f);
            }

            ultimateImg.fillAmount = 0f;
        }
    }

    private void Disparar()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        Vector2 direccionDisparo = direccionJugador;
        GameObject balaInstance = null; // Declarar la variable fuera del bloque if

        // Detecta qué flecha se ha presionado y ajusta la dirección de disparo
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direccionDisparo = Vector2.right; // Cambia la dirección de disparo hacia arriba
            rotation = Quaternion.Euler(0, 0, 90);

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            direccionDisparo = Vector2.right; // Cambia la dirección de disparo hacia abajo
            rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            direccionDisparo = Vector2.left; // Cambia la dirección de disparo hacia la izquierda

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            direccionDisparo = Vector2.right; // Cambia la dirección de disparo hacia la derecha
        }

        // Instancia la bala con la dirección de disparo adecuada
        balaInstance = Instantiate(balaPrefab, controladorDisparo.position, rotation);

        // Verifica si se ha instanciado una bala antes de intentar obtener el componente
        if (balaInstance != null)
        {
            // Obtener el script de la bala y establecer la dirección de disparo
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

    private void DispararUltimate()
    {
        Vector3 posicionCursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicionCursor.z = 0f;

        Vector2 direccionDisparo = direccionJugador;

        // Guardar la posición inicial al disparar la ultimate
        posicionInicial = transform.position;

        // Verifica si la tecla W está siendo presionada
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direccionDisparo = Vector2.right; // Cambia la dirección de disparo hacia arriba

            // Si disparamos hacia arriba, giramos la instancia en 90 grados en el eje Z
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
            direccionDisparo = Vector2.right; // Cambia la dirección de disparo hacia abajo

            // Si disparamos hacia abajo, giramos la instancia en -90 grados en el eje Z
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
                // Si no disparamos hacia arriba ni hacia abajo, mantenemos la rotación normal
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
                // Si no disparamos hacia arriba ni hacia abajo, mantenemos la rotación normal pero invertida
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

        // Aplicar retroceso al disparar la ultimate
        AplicarRecoil();
    }

    private void AplicarRecoil()
    {
        // Aplicar retroceso (recoil) al jugador desde la posición inicial
        float recoilAmountX = 0.5f; // Ajusta la cantidad de retroceso en el eje X según sea necesario
        float recoilAmountY = 0.0f; // Ajusta la cantidad de retroceso en el eje Y según sea necesario

        // Calcula el vector de retroceso en el eje X
        Vector2 recoilDirectionX = -direccionJugador.normalized; // Invertimos la dirección del jugador en el eje X
        Vector2 recoilVectorX = recoilDirectionX * recoilAmountX;

        // Calcula el vector de retroceso en el eje Y
        Vector2 recoilVectorY = Vector2.up * recoilAmountY; // Aplicamos el retroceso hacia arriba en el eje Y

        // Combina los vectores de retroceso en ambos ejes
        Vector2 totalRecoilVector = recoilVectorX + recoilVectorY;

        // Aplica el vector de retroceso a la posición inicial del jugador
        transform.position = posicionInicial + totalRecoilVector;
    }

    private IEnumerator BlinkImage()
    {
        Color originalColor = ultimateImg.color;
        Color targetColor = Color.yellow;
        float blinkSpeed = 0.5f; // Velocidad del parpadeo (en segundos)

        while (true)
        {
            // Si está llenándose y el fillAmount es 1, cambia entre el color actual y amarillo
            if (isFilling && ultimateImg.fillAmount == 1f)
            {
                ultimateImg.color = targetColor;
                yield return new WaitForSeconds(blinkSpeed / 2);
                ultimateImg.color = originalColor;
                yield return new WaitForSeconds(blinkSpeed / 2);
            }
            else
            {
                // Si no está llenándose o el fillAmount ya no es 1, vuelve al color original y espera un poco antes de verificar de nuevo
                ultimateImg.color = originalColor;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}

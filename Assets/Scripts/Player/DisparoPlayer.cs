using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisparoPlayer : MonoBehaviour
{
    [SerializeField] private Transform controladorDisparo;
    [SerializeField] private GameObject balaPrefab;
    [SerializeField] private GameObject ultimatePrefab;
    [SerializeField] private Image ultimate;
    [SerializeField] private AudioClip disparoClip;
    [SerializeField] private AudioClip ultimateClip;
    private AudioSource source;

    public Vector2 direccionJugador = Vector2.right;
    private bool isFilling;

    private Vector2 posicionInicial; // Guarda la posición inicial del jugador al disparar la ultimate

    private void Start()
    {
        source = GetComponent<AudioSource>();
        isFilling = false;
        // Comienza la corutina para hacer parpadear la imagen
        StartCoroutine(BlinkImage());
    }

    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        direccionJugador = moveInput > 0 ? Vector2.right : (moveInput < 0 ? Vector2.left : direccionJugador);

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (source != null && disparoClip != null)
            {
                source.PlayOneShot(disparoClip);
                Disparar();
            }
        }

        if (ultimate.fillAmount == 1f)
        {
            isFilling = true;
        }

        if (Input.GetKeyDown(KeyCode.E) && ultimate.fillAmount == 1f)
        {
            if (source != null && ultimateClip != null)
            {
                source.PlayOneShot(ultimateClip);
                Invoke("DispararUltimate", 1f);
            }

            ultimate.fillAmount = 0f;
        }
    }

    private void Disparar()
    {
        Vector3 posicionCursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicionCursor.z = 0f;

        Vector2 direccionDisparo = direccionJugador;
        GameObject balaInstance = null; // Declarar la variable fuera del bloque if

        // Verifica si la tecla W está siendo presionada
        if (Input.GetKey(KeyCode.W))
        {
            direccionDisparo = Vector2.right; // Cambia la dirección de disparo hacia arriba
                                           // Si la dirección del disparo es hacia arriba, rotar la bala 90 grados en Z
            Quaternion rotacion = Quaternion.Euler(0f, 0f, 90f);
            // Instanciar la bala con la rotación adecuada
            balaInstance = Instantiate(balaPrefab, controladorDisparo.position, rotacion);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            direccionDisparo = Vector2.right; // Cambia la dirección de disparo hacia abajo
                                             // Si la dirección del disparo es hacia abajo, rotar la bala -90 grados en Z
            Quaternion rotacion = Quaternion.Euler(0f, 0f, -90f);
            // Instanciar la bala con la rotación adecuada
            balaInstance = Instantiate(balaPrefab, controladorDisparo.position, rotacion);
        }
        else
        {
            // Si no se está presionando ni W ni S, instanciar la bala sin rotación especial
            balaInstance = Instantiate(balaPrefab, controladorDisparo.position, Quaternion.identity);
        }

        // Verificar si se ha instanciado una bala antes de intentar obtener el componente
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
        if (Input.GetKey(KeyCode.W))
        {
            direccionDisparo = Vector2.right; // Cambia la dirección de disparo hacia arriba

            // Si disparamos hacia arriba, giramos la instancia en 90 grados en el eje Y
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
        else
        {
            if (direccionDisparo.x > 0f)
            {
                // Si no disparamos hacia arriba, mantenemos la rotación normal
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
                Quaternion rotation = Quaternion.Euler(0, 0, 180);
                // Si no disparamos hacia arriba, mantenemos la rotación normal
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
        Color originalColor = ultimate.color;
        Color targetColor = Color.yellow;
        float blinkSpeed = 0.5f; // Velocidad del parpadeo (en segundos)

        while (true)
        {
            // Si está llenándose y el fillAmount es 1, cambia entre el color actual y amarillo
            if (isFilling && ultimate.fillAmount == 1f)
            {
                ultimate.color = targetColor;
                yield return new WaitForSeconds(blinkSpeed / 2);
                ultimate.color = originalColor;
                yield return new WaitForSeconds(blinkSpeed / 2);
            }
            else
            {
                // Si no está llenándose o el fillAmount ya no es 1, vuelve al color original y espera un poco antes de verificar de nuevo
                ultimate.color = originalColor;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}

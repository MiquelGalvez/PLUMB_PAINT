using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisparoPlayer : MonoBehaviour
{
    [SerializeField] private Transform controladorDisparo;
    [SerializeField] private GameObject balaPrefab;
    [SerializeField] private GameObject ultimatePrefab;
    [SerializeField] private Image ultimate;
    [SerializeField] private AudioSource disparoSound;
    [SerializeField] private AudioClip disparoClip;
    [SerializeField] private AudioSource ultimateSound;
    [SerializeField] private AudioClip ultimateClip;

    public Vector2 direccionJugador = Vector2.right;
    private bool isFilling;

    private Vector2 posicionInicial; // Guarda la posición inicial del jugador al disparar la ultimate

    private void Start()
    {
        isFilling = false;
        // Comienza la corutina para hacer parpadear la imagen
        StartCoroutine(BlinkImage());
    }

    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        direccionJugador = moveInput > 0 ? Vector2.right : (moveInput < 0 ? Vector2.left : direccionJugador);

        if (Input.GetButtonDown("Fire1"))
        {
            if (disparoSound != null && disparoClip != null)
            {
                disparoSound.PlayOneShot(disparoClip);
                Invoke("Disparar", 0.2f);
            }
        }

        if (ultimate.fillAmount == 1f)
        {
            isFilling = true;
        }

        if (Input.GetKeyDown(KeyCode.E) && ultimate.fillAmount == 1f)
        {
            if (ultimateSound != null && ultimateClip != null)
            {
                ultimateSound.PlayOneShot(ultimateClip);
                Invoke("DispararUltimate", 1.5f);

                // No guardamos la posición inicial aquí, la guardamos cuando se dispara la ultimate
            }

            ultimate.fillAmount = 0f;
        }
    }

    private void Disparar()
    {
        Vector3 posicionCursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicionCursor.z = 0f;

        Vector2 direccionDisparo = direccionJugador;

        // Verifica si la tecla W está siendo presionada
        if (Input.GetKey(KeyCode.W))
        {
            direccionDisparo.y = 1f; // Ajusta la dirección de disparo hacia arriba en el eje Y
            direccionDisparo.x = 90f; // Anula la dirección de disparo en el eje X
        }

        GameObject balaInstance = Instantiate(balaPrefab, controladorDisparo.position, Quaternion.identity);
        Bala balaScript = balaInstance.GetComponent<Bala>();
        if (balaScript != null)
        {
            balaScript.EstablecerDireccionDeDisparo(direccionDisparo);

            // Verifica si la dirección de disparo es hacia arriba
            if (direccionDisparo.y > 0f)
            {
                // Rota la instancia de la bala en el eje Y en 90 grados
                balaInstance.transform.Rotate(0f, 0f, 90f);
            }
        }
        else
        {
            Debug.LogWarning("El prefab de la bala no tiene el componente Bala adjunto.");
        }
    }

    private void DispararUltimate()
    {
        Vector3 posicionCursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicionCursor.z = 0f;

        Vector2 direccionDisparo = direccionJugador;

        // Guardar la posición inicial al disparar la ultimate
        posicionInicial = transform.position;

        GameObject balaInstance = Instantiate(ultimatePrefab, controladorDisparo.position, Quaternion.identity);
        Bala balaScript = balaInstance.GetComponent<Bala>();
        if (balaScript != null)
        {
            balaScript.EstablecerDireccionDeDisparo(direccionDisparo);
        }
        else
        {
            Debug.LogWarning("El prefab de la bala no tiene el componente Bala adjunto.");
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
        Vector2 recoilDirectionX = -transform.right; // Invertimos la dirección del jugador en el eje X
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

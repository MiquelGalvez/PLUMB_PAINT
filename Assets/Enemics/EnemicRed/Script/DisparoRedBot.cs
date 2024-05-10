using UnityEngine;

public class DisparoRedBot : MonoBehaviour
{
    public GameObject balaPrefab; // Prefab de la bala que disparar� el ControladorDisparo
    public Transform puntoDisparo; // Punto desde donde se disparar�n las balas
    public float velocidadBala = 10f; // Velocidad a la que se mover� la bala
    public float cadenciaDisparo = 1f; // Cadencia de disparo en segundos
    public float tiempoDestruccionBala = 2f; // Tiempo en segundos antes de destruir la bala

    private Transform jugador; // Referencia al transform del jugador
    private float tiempoUltimoDisparo; // Tiempo en el que se realiz� el �ltimo disparo
    private Animator animator; // Referencia al Animator

    void Start()
    {
        // Buscar el GameObject del jugador al inicio
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        // Obtener el componente Animator
        animator = GetComponent<Animator>();
        // Inicializar el tiempo del �ltimo disparo al inicio del juego
        tiempoUltimoDisparo = -cadenciaDisparo;
    }

    void Update()
    {
        // Verificar si el jugador est� dentro del campo de visi�n y la animaci�n PlayerDetect est� activa
        if (Vector2.Distance(transform.position, jugador.position) < 10f && animator.GetBool("PlayerDetect"))
        {
            // Verificar si ha pasado suficiente tiempo desde el �ltimo disparo
            if (Time.time - tiempoUltimoDisparo > cadenciaDisparo)
            {
                // Realizar el disparo
                Disparar();
                // Actualizar el tiempo del �ltimo disparo
                tiempoUltimoDisparo = Time.time;
            }
        }
    }

    void Disparar()
    {
        // Determinar la direcci�n de disparo en funci�n de la rotaci�n del enemigo
        Vector2 direccion = Vector2.right; // Por defecto, dispara hacia la derecha
        if (transform.rotation.eulerAngles.y == 0f) // Si la rotaci�n es 0 grados
        {
            direccion = Vector2.left; // Dispara hacia la izquierda
        }

        // Instanciar una nueva bala
        GameObject bala = Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);

        // Obtener el componente Rigidbody2D de la bala instanciada y asignarle velocidad
        Rigidbody2D rb = bala.GetComponent<Rigidbody2D>();
        rb.velocity = direccion * velocidadBala;

        // Destruir la bala despu�s de un cierto tiempo
        Destroy(bala, tiempoDestruccionBala);
    }
}

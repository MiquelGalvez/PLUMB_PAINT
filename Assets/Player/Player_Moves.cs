using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player_Moves : MonoBehaviour
{
    private Rigidbody2D rb2D;
    [Header("Movement")]

    private float movimientoHorizontal = 0f;

    [SerializeField] private float velocidadDeMovimiento;
    [SerializeField] private float suavizadoDeMovimiento;

    private Vector3 velocidad = Vector3.zero;
    private bool mirandoDerecha = true;
    //[Header ("Salto")]

    //[SerializeField] private float fuerzaDeSalto;
   // [SerializeField] private LayerMask queEsSuelo;
   // [SerializeField] private Transform controladorSuelo;
   //[SerializeField] private Vector3 dimensionesCaja;
    //[SerializeField] private bool enSuelo;
    //private bool salto = false;

    //[Header ("Sprint")]
    //[SerializeField] private float velocidadDeMovimientoBase;
    //[SerializeField] private float velocidadExtra;

    //private bool estaCorriendo = false;
    [Header("Animacion")]
    private Animator animator;

    private void Start()
    {
        // Obtiene la referencia al componente Rigidbody2D del jugador y el animator
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Captura el input para el movimiento horizontal del jugador
        movimientoHorizontal = Input.GetAxisRaw("Horizontal") * velocidadDeMovimiento;

        // Actualiza los parámetros en el Animator para la animación de movimiento
        //animator.SetBool("estaCorriendo", estaCorriendo);
        animator.SetFloat("Horizontal", Mathf.Abs(movimientoHorizontal));

        // Captura el input para el salto
       /* if (Input.GetButtonDown("Jump"))
        {
            salto = true;
        }

        // Captura el input para el sprint
        /*if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            velocidadDeMovimiento = velocidadExtra;
            estaCorriendo = true;
        }

        // Restaura la velocidad de movimiento base cuando se suelta la tecla de sprint
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            velocidadDeMovimiento = velocidadDeMovimientoBase;
            estaCorriendo = false;
        }*/
    }

    private void FixedUpdate()
    {
        // Comprobamos si hay algo en la posición del controladorSuelo que pertenezca a la capa queEsSuelo
       // enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);

        
        // Actualizamos la variable "enSuelo" en el animator para controlar la animación de estar en el suelo.
        //animator.SetBool("enSuelo", enSuelo);
        
        //movimiento del personaje
        Mover(movimientoHorizontal * Time.fixedDeltaTime);

        // Reseteamos la variable de salto para evitar saltos continuos.
        //salto = false;


    }

    private void Mover(float mover/*, bool saltar*/)
    {

        // Calculamos la velocidad para suavizar el movimiento horizontal y mantener la velocidad vertical actual.
        Vector3 velocidadObjetivo = new Vector2(mover, rb2D.velocity.y);

        // Aplicamos el movimiento suavizado utilizando SmoothDamp.
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, velocidadObjetivo, ref velocidad, suavizadoDeMovimiento);


        // Giramos el personaje si está moviéndose en la dirección opuesta a la que está mirando.
        if (mover > 0 && !mirandoDerecha)
        {
            Girar();

        }
        // Giramos el personaje si está moviéndose en la dirección opuesta a la que está mirando.
        else if ( mover < 0 && mirandoDerecha)
        {
            Girar();
        }

        // Si el personaje está en el suelo y se solicita un salto, aplicamos una fuerza vertical para simular el salto.
        /*if (enSuelo && saltar)
        {
            enSuelo = false; // Indicamos que ya no estamos en el suelo para evitar saltos continuos.
            rb2D.AddForce(new Vector2(0f, fuerzaDeSalto));
        }*/
    }

    // Función que invierte la dirección en la que mira el personaje y refleja su escala en el eje X.
    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }


    // Función llamada en el editor para visualizar el área de detección de suelo.
   /* private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);

    }
    /*void OnCollisionEnter2D(Collision2D collision)
    {

        // Si la colisión es con un objeto que tiene el tag "deadGround", reiniciamos la escena.
        if (collision.gameObject.CompareTag("deadGround"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recargamos la escena actual.
        }
    }*/

}

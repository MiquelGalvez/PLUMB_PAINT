using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    private float speed;
    private Animator animator;
    private bool isMoving = false;
    private float movementDuration = 0f;
    private float maxMovementDuration = 10f; // Maximum duration the car can move before destruction

    // M�todo para establecer la velocidad del carro
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Start()
    {
        // Obtener el componente Animator del carro
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Mover el carro hacia la derecha con la velocidad especificada
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Si el carro est� movi�ndose y la animaci�n de movimiento no est� activada, activarla
        if (speed > 0 && !isMoving)
        {
            isMoving = true;
            // Activar el par�metro "IsMoving" en el Animator
            animator.SetBool("IsMoving", true);
        }
        // Si el carro no est� movi�ndose y la animaci�n de movimiento est� activada, desactivarla
        else if (speed == 0 && isMoving)
        {
            isMoving = false;
            // Desactivar el par�metro "IsMoving" en el Animator
            animator.SetBool("IsMoving", false);
        }

        // Incrementar la duraci�n de movimiento si el carro est� movi�ndose
        if (speed > 0)
        {
            movementDuration += Time.deltaTime;

            // Destruir el carro si ha estado movi�ndose durante m�s tiempo del m�ximo permitido
            if (movementDuration >= maxMovementDuration)
            {
                Destroy(gameObject);
            }
        }
    }
}

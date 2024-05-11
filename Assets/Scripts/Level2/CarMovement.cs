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

    // Método para establecer la velocidad del carro
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

        // Si el carro está moviéndose y la animación de movimiento no está activada, activarla
        if (speed > 0 && !isMoving)
        {
            isMoving = true;
            // Activar el parámetro "IsMoving" en el Animator
            animator.SetBool("IsMoving", true);
        }
        // Si el carro no está moviéndose y la animación de movimiento está activada, desactivarla
        else if (speed == 0 && isMoving)
        {
            isMoving = false;
            // Desactivar el parámetro "IsMoving" en el Animator
            animator.SetBool("IsMoving", false);
        }

        // Incrementar la duración de movimiento si el carro está moviéndose
        if (speed > 0)
        {
            movementDuration += Time.deltaTime;

            // Destruir el carro si ha estado moviéndose durante más tiempo del máximo permitido
            if (movementDuration >= maxMovementDuration)
            {
                Destroy(gameObject);
            }
        }
    }
}

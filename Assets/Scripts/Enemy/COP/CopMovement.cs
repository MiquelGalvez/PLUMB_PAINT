using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopMovement : MonoBehaviour
{
    private bool facingRight = true; // Variable para mantener la direcci�n del enemigo

    // M�todo para hacer que el enemigo mire hacia la derecha
    public void FaceRight()
    {
        if (!facingRight)
        {
            Flip();
        }
    }

    // M�todo para hacer que el enemigo mire hacia la izquierda
    public void FaceLeft()
    {
        if (facingRight)
        {
            Flip();
        }
    }

    // M�todo para voltear la direcci�n del enemigo
    private void Flip()
    {
        facingRight = !facingRight; // Cambiar la direcci�n

        // Obtener la escala actual del enemigo
        Vector3 scale = transform.localScale;
        // Voltear la escala horizontalmente
        scale.x *= -1;
        // Aplicar la nueva escala al enemigo
        transform.localScale = scale;
    }
}

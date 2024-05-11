using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageBlinking : MonoBehaviour
{
    private float intervaloParpadeo = 0.5f; // Intervalo de tiempo entre cada cambio de visibilidad
    private bool visible = true; // Estado de visibilidad actual de la imagen
    private SpriteRenderer spriteRenderer; // Componente SpriteRenderer de la imagen

    void Start()
    {
        // Obtener el componente SpriteRenderer de la imagen
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Comenzar el proceso de parpadeo
        InvokeRepeating("CambiarVisibilidad", intervaloParpadeo, intervaloParpadeo);
    }

    void CambiarVisibilidad()
    {
        // Invertir el estado de visibilidad
        visible = !visible;

        // Actualizar la visibilidad de la imagen
        spriteRenderer.enabled = visible;
    }
}

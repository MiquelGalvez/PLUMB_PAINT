using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageBlinking : MonoBehaviour
{
    private float intervaloParpadeo = 0.5f; // Time interval between each visibility change
    private bool visible = true; // Current visibility state of the image
    private SpriteRenderer spriteRenderer; // SpriteRenderer component of the image

    void Start()
    {
        // Get the SpriteRenderer component of the image
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Start the blinking process
        InvokeRepeating("CambiarVisibilidad", intervaloParpadeo, intervaloParpadeo);
    }

    void CambiarVisibilidad()
    {
        // Toggle the visibility state
        visible = !visible;

        // Update the visibility of the image
        spriteRenderer.enabled = visible;
    }
}

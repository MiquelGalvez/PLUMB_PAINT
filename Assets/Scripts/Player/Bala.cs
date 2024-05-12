using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    [SerializeField] private float velocidad; // Speed of the bullet
    private Vector2 direccionDeDisparo = Vector2.right; // Default direction of the bullet is right

    // Method to set the shooting direction from the character
    public void EstablecerDireccionDeDisparo(Vector2 direccion)
    {
        direccionDeDisparo = direccion.normalized; // Normalize the direction vector
    }

    private void Update()
    {
        // Move the bullet in the shooting direction
        transform.Translate(direccionDeDisparo * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the bullet collides with an object tagged as "Bullet" and destroy it
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
        }
    }
}

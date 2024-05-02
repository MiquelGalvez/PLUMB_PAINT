using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    [SerializeField] private float velocidad;
    private Vector2 direccionDeDisparo = Vector2.right; // Por defecto, la bala dispara hacia la derecha

    // M�todo para establecer la direcci�n de disparo desde el personaje
    public void EstablecerDireccionDeDisparo(Vector2 direccion)
    {
        direccionDeDisparo = direccion.normalized; // Normalizamos el vector de direcci�n

    }

    private void Update()
    {
        // Movemos la bala en la direcci�n de disparo
        transform.Translate(direccionDeDisparo * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
        }
    }
}

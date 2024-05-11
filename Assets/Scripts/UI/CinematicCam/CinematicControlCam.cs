using System.Collections;
using UnityEngine;

public class CinematicControlCam : MonoBehaviour
{
    public Transform puntoDestino; // Punto al que la cámara se moverá
    public GameObject player; // Objeto que se activará una vez que la cámara llegue al punto destino
    public GameObject UI; // Objeto que se activará una vez que la cámara llegue al punto destino
    public GameObject SpawnCars; // Objeto que se activará una vez que la cámara llegue al punto destino

    public float velocidad = 5f; // Velocidad de desplazamiento de la cámara
    public float tolerancia = 0.1f; // Tolerancia para considerar que la cámara ha llegado al punto destino

    private bool alcanzadoDestino = false; // Bandera para verificar si se ha alcanzado el punto destino

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Verificar si no se ha alcanzado el punto destino
        if (!alcanzadoDestino)
        {
            // Calcular la posición deseada con la misma posición en Y y Z, y la posición deseada en X
            Vector3 posicionDeseada = new Vector3(puntoDestino.position.x, transform.position.y, transform.position.z);

            // Mover la cámara hacia la posición deseada
            transform.position = Vector3.MoveTowards(transform.position, posicionDeseada, velocidad * Time.deltaTime);

            // Verificar si la cámara está lo suficientemente cerca de la posición deseada en el eje X
            if (Mathf.Abs(transform.position.x - puntoDestino.position.x) < tolerancia)
            {
                // Establecer la bandera de alcanzadoDestino como verdadera
                alcanzadoDestino = true;
            }
        }
    }
}

using System.Collections;
using UnityEngine;

public class CinematicControlCam : MonoBehaviour
{
    public Transform puntoDestino; // Punto al que la c�mara se mover�
    public GameObject player; // Objeto que se activar� una vez que la c�mara llegue al punto destino
    public GameObject UI; // Objeto que se activar� una vez que la c�mara llegue al punto destino
    public GameObject SpawnCars; // Objeto que se activar� una vez que la c�mara llegue al punto destino

    public float velocidad = 5f; // Velocidad de desplazamiento de la c�mara
    public float tolerancia = 0.1f; // Tolerancia para considerar que la c�mara ha llegado al punto destino

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
            // Calcular la posici�n deseada con la misma posici�n en Y y Z, y la posici�n deseada en X
            Vector3 posicionDeseada = new Vector3(puntoDestino.position.x, transform.position.y, transform.position.z);

            // Mover la c�mara hacia la posici�n deseada
            transform.position = Vector3.MoveTowards(transform.position, posicionDeseada, velocidad * Time.deltaTime);

            // Verificar si la c�mara est� lo suficientemente cerca de la posici�n deseada en el eje X
            if (Mathf.Abs(transform.position.x - puntoDestino.position.x) < tolerancia)
            {
                // Establecer la bandera de alcanzadoDestino como verdadera
                alcanzadoDestino = true;
            }
        }
    }
}

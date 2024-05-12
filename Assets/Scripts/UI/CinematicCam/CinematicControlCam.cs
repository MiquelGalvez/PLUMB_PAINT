using System.Collections;
using UnityEngine;

public class CinematicControlCam : MonoBehaviour
{
    public Transform puntoDestino; // Point to which the camera will move
    public GameObject player; // Object that will be activated once the camera reaches the destination point
    public GameObject UI; // Object that will be activated once the camera reaches the destination point
    public GameObject SpawnCars; // Object that will be activated once the camera reaches the destination point

    public float velocidad = 5f; // Movement speed of the camera
    public float tolerancia = 0.1f; // Tolerance to consider that the camera has reached the destination point

    private bool alcanzadoDestino = false; // Flag to check if the destination point has been reached

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Check if the destination point has not been reached
        if (!alcanzadoDestino)
        {
            // Calculate the desired position with the same Y and Z position, and the desired X position
            Vector3 posicionDeseada = new Vector3(puntoDestino.position.x, transform.position.y, transform.position.z);

            // Move the camera towards the desired position
            transform.position = Vector3.MoveTowards(transform.position, posicionDeseada, velocidad * Time.deltaTime);

            // Check if the camera is close enough to the desired position on the X axis
            if (Mathf.Abs(transform.position.x - puntoDestino.position.x) < tolerancia)
            {
                // Set the alcanzadoDestino flag to true
                alcanzadoDestino = true;
            }
        }
    }
}

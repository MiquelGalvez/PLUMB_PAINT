using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActivateSettings : MonoBehaviour
{
    // Referencia a la cámara
    private Camera mainCamera;
    [SerializeField] private GameObject canvassettings;

    // Variable para controlar si el canvas de configuración está activo
    private bool isSettingsActive = false;

    // Lista para almacenar los objetos desactivados
    private List<GameObject> deactivatedObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // Obtener la cámara principal
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Si se presiona la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isSettingsActive)
            {
                // Desactivar el canvas de configuración
                canvassettings.SetActive(false);

                Time.timeScale = 1f;

                // Desbloquear el cursor y hacerlo visible
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                

                // Actualizar el estado del canvas de configuración
                isSettingsActive = false;
            }
            else
            {
                // Desbloquear el cursor y hacerlo visible
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                Time.timeScale = 0f;
                // Activar el canvas de configuración
                canvassettings.SetActive(true);

                // Actualizar el estado del canvas de configuración
                isSettingsActive = true;
            }
        }
    }
}

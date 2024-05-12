using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActivateSettings : MonoBehaviour
{
    // Reference to the camera
    private Camera mainCamera;
    [SerializeField] private GameObject canvassettings;

    // Variable to control if the settings canvas is active
    private bool isSettingsActive = false;

    // List to store deactivated objects
    private List<GameObject> deactivatedObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // Get the main camera
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // If the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isSettingsActive)
            {
                // Deactivate the settings canvas
                canvassettings.SetActive(false);

                Time.timeScale = 1f;

                // Unlock the cursor and make it visible
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                // Update the state of the settings canvas
                isSettingsActive = false;
            }
            else
            {
                // Unlock the cursor and make it visible
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                Time.timeScale = 0f;
                // Activate the settings canvas
                canvassettings.SetActive(true);

                // Update the state of the settings canvas
                isSettingsActive = true;
            }
        }
    }
}

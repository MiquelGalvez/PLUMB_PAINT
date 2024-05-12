using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    private GameObject sceneObjects;
    [SerializeField] Canvas settingsCanvas;
    [SerializeField] Button[] menuButtons;
    [SerializeField] GameObject highlightPlay;
    [SerializeField] GameObject highlightSettings;
    [SerializeField] GameObject highlightExit;
    [SerializeField] GameObject highlightTop5;

    private int currentOptionIndex = 0;

    void Start()
    {
        sceneObjects = this.gameObject;
        Cursor.visible = false;
        UpdateButtonColors(); // Set initial button colors
    }

    void Update()
    {
        NavigateMenu(); // Navigate through the menu
    }

    void NavigateMenu()
    {
        // Move selection up when 'W' key is pressed, down when 'S' key is pressed
        if (Input.GetKeyDown(KeyCode.W))
        {
            currentOptionIndex = (currentOptionIndex - 1 + menuButtons.Length) % menuButtons.Length;
            UpdateButtonColors();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            currentOptionIndex = (currentOptionIndex + 1) % menuButtons.Length;
            UpdateButtonColors();
        }

        // Trigger the selected option when space or return key is pressed
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            TriggerOption();
        }
    }

    // Update the color of the menu buttons based on selection
    void UpdateButtonColors()
    {
        for (int i = 0; i < menuButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = menuButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.color = (i == currentOptionIndex) ? new Color(0.776f, 0.698f, 1f) : Color.white; // RGB format: Color.C6B2FF
            }
        }

        UpdateButtonHighlights(); // Update button highlights
    }

    // Update the visibility of button highlights based on selection
    void UpdateButtonHighlights()
    {
        highlightPlay.SetActive(currentOptionIndex == 0);
        highlightSettings.SetActive(currentOptionIndex == 1);
        highlightTop5.SetActive(currentOptionIndex == 2);
        highlightExit.SetActive(currentOptionIndex == 3);
    }

    // Load the play scene when the play button is pressed
    public void OnPlayButton()
    {
        SceneManager.LoadScene(1);
    }

    // Activate the settings canvas and hide the main menu when settings button is pressed
    public void OnSettingsButton()
    {
        sceneObjects.SetActive(false);
        settingsCanvas.gameObject.SetActive(true);
        Cursor.visible = true;
    }

    // Return from settings to the main menu
    public void OnBackSettings()
    {
        sceneObjects.SetActive(true);
        settingsCanvas.gameObject.SetActive(false);
        Cursor.visible = false;
    }

    // Quit the application when the exit button is pressed
    public void OnQuitButton()
    {
        Application.Quit();
    }

    // Load the top 5 scores scene
    public void OnTop5()
    {
        SceneManager.LoadScene(5);
    }

    // Trigger the action associated with the current selected option
    void TriggerOption()
    {
        switch (currentOptionIndex)
        {
            case 0:
                OnPlayButton();
                break;
            case 1:
                OnSettingsButton();
                break;
            case 2:
                OnTop5();
                break;
            case 3:
                OnQuitButton();
                break;
        }
    }
}

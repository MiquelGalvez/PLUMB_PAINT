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

    private int currentOptionIndex = 0;

    void Start()
    {
        sceneObjects = this.gameObject;
        Cursor.visible = false;

        // Configura el color inicial de los textos de los botones
        UpdateButtonColors();
    }

    void Update()
    {
        NavigateMenu();
    }

    void NavigateMenu()
    {
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

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            TriggerOption();
        }
    }

    void UpdateButtonColors()
    {
        for (int i = 0; i < menuButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = menuButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.color = (i == currentOptionIndex) ? new Color(0.776f, 0.698f, 1f) : Color.white; // Color.C6B2FF en formato RGB
            }
        }

        // Actualiza el resaltado de los botones
        UpdateButtonHighlights();
    }

    void UpdateButtonHighlights()
    {
        highlightPlay.SetActive(currentOptionIndex == 0);
        highlightSettings.SetActive(currentOptionIndex == 1);
        highlightExit.SetActive(currentOptionIndex == 2);
    }

    public void OnPlayButton()
    {
       SceneManager.LoadScene(1);
    }

    public void OnSettingsButton()
    {
        sceneObjects.SetActive(false);
        settingsCanvas.gameObject.SetActive(true);
        Cursor.visible = true;
    }

    public void OnBackSettings()
    {
        sceneObjects.SetActive(true);
        settingsCanvas.gameObject.SetActive(false);
        Cursor.visible = false;
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

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
                OnQuitButton();
                break;
        }
    }
}

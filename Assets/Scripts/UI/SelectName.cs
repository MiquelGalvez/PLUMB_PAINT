using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectName : MonoBehaviour
{
    PlayerData dataplayer; // Reference to PlayerData object
    [SerializeField] private TextMeshProUGUI textoCanvas; // Reference to TextMeshProUGUI component on canvas
    [SerializeField] private TMP_InputField inputField; // Reference to the input field
    private DatabaseAccess databaseaccess; // Reference to DatabaseAccess object
    [SerializeField] private GameObject errorname;
    [SerializeField] private TextMeshProUGUI texterror;
    [SerializeField] private GameObject gameObjectToDeactivate;

    // Use Start method to populate the input field with the last entered name
    void Start()
    {

        if (!string.IsNullOrEmpty(PlayerData.lastEnteredName))
        {
            inputField.text = PlayerData.lastEnteredName;
            textoCanvas.text = PlayerData.lastEnteredName;
        }
    }

    // Method called when input field text is updated
    public void ActualizarTexto(TMP_InputField input)
    {
        errorname.SetActive(false);
        // Get the entered text
        string inputText = input.text;
        // Check if the name contains special characters
        if (HasSpecialCharacters(inputText))
        {
            gameObjectToDeactivate.SetActive(false);
            Debug.LogWarning("Name cannot contain special characters!");
            errorname.SetActive(true);
            texterror.text = "Name cannot contain special characters!";
            return; // Exit the method without saving
        }

        if (inputText == "")
        {
            gameObjectToDeactivate.SetActive(false);
            Debug.LogWarning("Name cannot be blank!");
            errorname.SetActive(true);
            texterror.text = "Name cannot be blank!";
            return; // Exit the method without saving
        }

        // Check if the name is longer than 5 characters
        if (inputText.Length > 5)
        {
            gameObjectToDeactivate.SetActive(false);
            Debug.LogWarning("Name cannot be longer than 5 characters!");
            errorname.SetActive(true);
            texterror.text = "Name cannot be longer than 5 characters!";

            return; // Exit the method without saving
        }
        // Update the input field with the modified text
        input.text = inputText;
        // Guardar el nombre en la base de datos
        databaseaccess = databaseaccess ?? new DatabaseAccess();
        bool saveSuccessful = databaseaccess.SaveName(inputText.ToLower());
        Debug.Log(saveSuccessful.ToString());
        // Desactivar el GameObject si la operación de guardar falla
        if (!saveSuccessful)
        {
            // Desactivar el GameObject que se desea desactivar
            gameObjectToDeactivate.SetActive(false);
            errorname.SetActive(true);
            texterror.text = "There is already someone with that name";
            return;
        }


        // Update the text on the canvas
        textoCanvas.text = inputText;

        // Check if the name already exists in PlayerData
        if (PlayerData.NameExists(inputText))
        {
            if (HasSpecialCharacters(inputText))
            {
                gameObjectToDeactivate.SetActive(false);
                Debug.LogWarning("Name cannot contain special characters!");
                errorname.SetActive(true);
                texterror.text = "Name cannot contain special characters!";
                return; // Exit the method without saving
            }

            // Check if the name is longer than 5 characters
            if (inputText.Length > 5)
            {
                gameObjectToDeactivate.SetActive(false);
                Debug.LogWarning("Name cannot be longer than 5 characters!");
                texterror.text = "Name cannot be longer than 5 characters!";
                errorname.SetActive(true);
                return; // Exit the method without saving
            }
            gameObjectToDeactivate.SetActive(false);
            // Handle the case where the name already exists
            Debug.LogWarning("Name already exists!");
            texterror.text = "Name already exists!";
            errorname.SetActive(true);
            return; // Exit the method without saving
        }

        gameObjectToDeactivate.SetActive(true);
        // Create a new PlayerData object with the modified text
        dataplayer = new PlayerData(inputText);
        // Save in GameData
        GameData.playerData = dataplayer;

        // Update the last entered name in PlayerData
        PlayerData.lastEnteredName = inputText;
    }

    // Method to check if the given text contains special characters
    private bool HasSpecialCharacters(string text)
    {
        // Regular expression to match special characters
        Regex regex = new Regex("[^a-zA-Z0-9 ]");
        return regex.IsMatch(text);
    }
}
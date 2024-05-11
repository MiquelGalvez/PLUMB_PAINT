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
    PlayerData dataplayer;
    [SerializeField] private TextMeshProUGUI textoCanvas;
    [SerializeField] private TMP_InputField inputField; // Reference to the input field
    private DatabaseAccess databaseaccess;

    // Use Start method to populate the input field with the last entered name
    void Start()
    {
        if (!string.IsNullOrEmpty(PlayerData.lastEnteredName))
        {
            inputField.text = PlayerData.lastEnteredName;
            textoCanvas.text = PlayerData.lastEnteredName;
        }
    }

    public void ActualizarTexto(TMP_InputField input)
    {
        // Verificar si el texto ingresado cumple con las restricciones
        string inputText = input.text;

        // Actualizar el campo de texto con el texto modificado
        input.text = inputText;

        // Actualizar el texto en el canvas
        textoCanvas.text = inputText;

        // Check if the name already exists in PlayerData
        if (PlayerData.NameExists(inputText))
        {
            // Handle the case where the name already exists
            Debug.LogWarning("Name already exists!");
            return; // Exit the method without saving
        }

        // Crear un nuevo objeto PlayerData con el texto modificado
        dataplayer = new PlayerData(inputText);

        // Guardar en GameDatas
        GameData.playerData = dataplayer;

        // Guardar el nombre en la base de datos
        databaseaccess = databaseaccess ?? new DatabaseAccess();
        databaseaccess.SaveName(inputText);

        // Update the last entered name in PlayerData
        PlayerData.lastEnteredName = inputText;
    }
}

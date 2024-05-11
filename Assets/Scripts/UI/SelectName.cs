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
    private DatabaseAccess databaseaccess;

    public void ActualizarTexto(TMP_InputField input)
    {
        // Verificar si el texto ingresado cumple con las restricciones
        string inputText = input.text;

        // Actualizar el campo de texto con el texto modificado
        input.text = inputText;

        // Actualizar el texto en el canvas
        textoCanvas.text = inputText;

        // Crear un nuevo objeto PlayerData con el texto modificado
        dataplayer = new PlayerData(inputText);

        // Guardar en GameDatas
        GameData.playerData = dataplayer;

        // Guardar el nombre en la base de datos
        databaseaccess = databaseaccess ?? new DatabaseAccess();
        databaseaccess.SaveName(inputText);
    }


}

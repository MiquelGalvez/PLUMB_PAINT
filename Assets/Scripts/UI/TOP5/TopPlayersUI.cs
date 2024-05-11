using UnityEngine;
using UnityEngine.UI;
using MongoDB.Bson;
using System.Collections.Generic;
using TMPro;

public class TopPlayersUI : MonoBehaviour
{
    public TextMeshProUGUI topPlayersText;
    DatabaseAccess databaseAccess;

    void Start()
    {
        databaseAccess = new DatabaseAccess(); // Encuentra el objeto DatabaseAccess en la escena
        ShowTopPlayers();
    }

    void ShowTopPlayers()
{
    List<BsonDocument> topPlayers = databaseAccess.GetTopPlayers();

    if (topPlayers != null && topPlayers.Count > 0)
    {
        string playersInfo = "\n";
        int maxNameLength = 0;
        for (int i = 0; i < topPlayers.Count; i++)
        {
            string playerName = topPlayers[i]["player"].AsString;
            double playerScore = topPlayers[i]["score"].AsDouble;
            maxNameLength = Mathf.Max(maxNameLength, playerName.Length); // Encontrar la longitud máxima del nombre del jugador
        }

        for (int i = 0; i < topPlayers.Count; i++)
        {
            string playerName = topPlayers[i]["player"].AsString;
            double playerScore = topPlayers[i]["score"].AsDouble;
            string scoreString = $"{playerScore}p";
            string formattedLine = $"{i + 1}. {playerName.PadRight(maxNameLength)} {scoreString}\n";
            playersInfo += formattedLine;
        }
        topPlayersText.text = playersInfo;
    }
    else
    {
        topPlayersText.text = "No Players Found";
    }
}
}

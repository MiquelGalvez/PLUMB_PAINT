using UnityEngine;
using UnityEngine.UI;
using MongoDB.Bson;
using System.Collections.Generic;
using TMPro;

public class TopPlayersUI : MonoBehaviour
{
    public TextMeshProUGUI topPlayersText; // Reference to the TextMeshProUGUI component where top players' information will be displayed
    DatabaseAccess databaseAccess; // Reference to the DatabaseAccess script for accessing player data

    void Start()
    {
        databaseAccess = new DatabaseAccess(); // Initialize the DatabaseAccess object
        ShowTopPlayers(); // Call the method to display top players
    }

    // Method to display top players' information
    void ShowTopPlayers()
    {
        List<BsonDocument> topPlayers = databaseAccess.GetTopPlayers(); // Retrieve top players' data from the database

        // Check if there are top players retrieved from the database
        if (topPlayers != null && topPlayers.Count > 0)
        {
            string playersInfo = "\n"; // Initialize a string to store players' information
            int maxNameLength = 0; // Variable to store the maximum length of player names

            // Loop through the top players' data to find the maximum length of player names
            for (int i = 0; i < topPlayers.Count; i++)
            {
                string playerName = topPlayers[i]["player"].AsString; // Get the player's name
                maxNameLength = Mathf.Max(maxNameLength, playerName.Length); // Find the maximum name length
            }

            // Loop through the top players' data to format and add player information to the display string
            for (int i = 0; i < topPlayers.Count; i++)
            {
                string playerName = topPlayers[i]["player"].AsString; // Get the player's name
                double playerScore = topPlayers[i]["score"].AsDouble; // Get the player's score
                string scoreString = $"{playerScore}p"; // Format the player's score string
                string formattedLine = $"{i + 1}. {playerName.PadRight(maxNameLength)} {scoreString}\n"; // Format the player's information line
                playersInfo += formattedLine; // Add the formatted line to the players' information string
            }

            // Update the text of the TextMeshProUGUI component with the players' information
            topPlayersText.text = playersInfo;
        }
        else
        {
            // If no top players are found, display a message indicating no players found
            topPlayersText.text = "No Players Found";
        }
    }
}

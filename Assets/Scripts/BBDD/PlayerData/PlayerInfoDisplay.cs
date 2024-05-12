using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI playerNameText; // Reference to the TextMeshProUGUI component for displaying player name
    private PlayerData playerData; // Reference to the player data

    private DatabaseAccess databaseAccess; // Reference to the database access component

    private void Start()
    {
        databaseAccess = new DatabaseAccess(); // Initialize database access component
        playerData = GameData.playerData; // Get reference to the player data

        // Get the player's score and update the text
        UpdatePlayerInfo();
    }

    public void UpdatePlayerInfo()
    {
        // Get the player's score from the database
        double playerScore = databaseAccess.GetPlayerScore(playerData.playerName);

        // Update the text fields with the retrieved information
        playerNameText.text = "Player:  " + playerData.playerName + "   " + playerScore.ToString();
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;
    private PlayerData playerData;

    private DatabaseAccess databaseAccess;

    private void Start()
    {
        databaseAccess = new DatabaseAccess();
        playerData = GameData.playerData;
        
        // Obtener la puntuación del jugador y actualizar el texto
        UpdatePlayerInfo();
    }

    public void UpdatePlayerInfo()
    {

        // Obtener la puntuación del jugador
        double playerScore = databaseAccess.GetPlayerScore(playerData.playerName);

        // Actualizar los campos de texto con la información obtenida
        playerNameText.text = "Player:  " + playerData.playerName + "   " + playerScore.ToString();
    }
}
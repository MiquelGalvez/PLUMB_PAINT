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
        
        // Obtener la puntuaci�n del jugador y actualizar el texto
        UpdatePlayerInfo();
    }

    public void UpdatePlayerInfo()
    {

        // Obtener la puntuaci�n del jugador
        double playerScore = databaseAccess.GetPlayerScore(playerData.playerName);

        // Actualizar los campos de texto con la informaci�n obtenida
        playerNameText.text = "Player:  " + playerData.playerName + "   " + playerScore.ToString();
    }
}
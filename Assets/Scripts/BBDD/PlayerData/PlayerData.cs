using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public static string lastEnteredName; // Static field to store the last entered name
    public string playerName;

    public PlayerData(string name)
    {
        playerName = name;
        lastEnteredName = name; // Update the last entered name when creating a new PlayerData instance
    }

    // Static method to check if a name exists in the last entered name
    public static bool NameExists(string name)
    {
        return lastEnteredName == name;
    }
}

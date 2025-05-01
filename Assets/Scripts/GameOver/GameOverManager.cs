using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public TMP_Text WinnerText; // TextMeshPro element for winner name
    public TMP_Text LoserText;  // TextMeshPro element for loser name

    void Start()
    {
        // Retrieve winner and loser names from PlayerPrefs
        string winnerName = PlayerPrefs.GetString("WinnerName", "Unknown");
        string loserName = PlayerPrefs.GetString("LoserName", "Unknown");

        // Update the UI
        WinnerText.text = $"{winnerName} wins!";
        LoserText.text = $"{loserName} loses!";
    }
}